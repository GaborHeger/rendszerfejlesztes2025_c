using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using webshop_barbie.Data;
using webshop_barbie.DTOs;
using webshop_barbie.Models;
using webshop_barbie.Repository.Interfaces;
using webshop_barbie.Service.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace webshop_barbie.Service
{
    public class UserService : IUserService
    {
        private readonly PasswordHasher<string> _passwordHasher = new PasswordHasher<string>();
        private readonly IUserRepository _repository;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }

        public async Task<UserDTO> GetUserByIdAsync(int userId)
        {
            var u = await _repository.GetByIdAsync(userId);

            if (u == null)
                throw new KeyNotFoundException($"A felhasználó ({userId}) nem található.");

            // DTO konverzió
            var user = new UserDTO
            {
                UserId = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                PhoneNumber = u.PhoneNumber,
                PostalCode = u.PostalCode,
                City = u.City,
                AddressDetails = u.AddressDetails,
                SubscribedToNewsletter = u.SubscribedToNewsletter,
                AcceptedTerms = u.AcceptedTerms
            };

            return user;
        }


        public async Task<UserDTO> GetUserByEmailAsync(string email)
        {
            var u = await _repository.GetByEmailAsync(email);

            if (u == null)
            {
                Console.WriteLine("Repository visszaadott null-t!");
                return null;
            }
            Console.WriteLine($"Repository visszaadott User-t: {u.Id}");

            var user = new UserDTO
            {
                UserId = u.Id,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                PhoneNumber = u.PhoneNumber,
                PostalCode = u.PostalCode,
                City = u.City,
                AddressDetails = u.AddressDetails,
                SubscribedToNewsletter = u.SubscribedToNewsletter,
                AcceptedTerms = u.AcceptedTerms
            };

            return user;
        }

        public async Task<UserDTO> AddUserAsync(RegisterRequestDTO dto)
        {
            //E-mail ellenőrzés
            var existingUser = await _repository.GetByEmailAsync(dto.Email);
            if (existingUser != null)
                throw new ArgumentException("Ez az email már foglalt.");

            //entitás létrehozása
            var user = new User
            {
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                SubscribedToNewsletter = dto.SubscribedToNewsletter,
                AcceptedTerms = dto.AcceptedTerms
            };

            //jelszó hash-elése
            user.PasswordHash = _passwordHasher.HashPassword(user.Email, dto.Password);

            try
            {
                //adatbázisba mentés
                await _repository.AddAsync(user);
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("----- DB ERROR -----");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException?.Message);  // EZ A LÉNYEG!

                throw; // hagyjuk, hogy a globális hibakezelő elkapja
            }

            //DTO visszaadása
            return new UserDTO
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                PostalCode = user.PostalCode,
                City = user.City,
                AddressDetails = user.AddressDetails,
                SubscribedToNewsletter = user.SubscribedToNewsletter,
                AcceptedTerms = user.AcceptedTerms
            };
        }

        public async Task<UserDTO> UpdateUserAsync(UpdateUserRequestDTO user)
        {
            //DTO létrehozása a repository számára
            var userDto = new UserDTO
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                PostalCode = user.PostalCode,
                City = user.City,
                AddressDetails = user.AddressDetails,
                SubscribedToNewsletter = user.SubscribedToNewsletter,
                AcceptedTerms = user.AcceptedTerms
            };

            //repository hívása
            var updatedUser = await _repository.UpdateUserAsync(user.Id, userDto);

            //ha nincs változás, dobhatunk hibát
            if (updatedUser == null)
                throw new ArgumentException("Az adatok nem változtak!");

            return userDto;
        }

        private string GenerateJwtToken(User user)
        {
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim("userId", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiresInMinutes"])),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<LoginResponseDTO> ValidateLoginAsync(string email, string password)
        {
            var user = await _repository.GetByEmailAsync(email);

            if (user == null)
                throw new KeyNotFoundException($"A felhasználó az '{email}' email címmel nem található.");

            var isPasswordValid = _passwordHasher.VerifyHashedPassword(null, user.PasswordHash, password);
            if (isPasswordValid != PasswordVerificationResult.Success)
                throw new UnauthorizedAccessException("Hibás jelszó.");

            var userDto = new UserDTO
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                PostalCode = user.PostalCode,
                City = user.City,
                AddressDetails = user.AddressDetails,
                SubscribedToNewsletter = user.SubscribedToNewsletter,
                AcceptedTerms = user.AcceptedTerms
            };

            var token = GenerateJwtToken(user);

            return new LoginResponseDTO
            {
                Token = token,
                UserId = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                PostalCode = user.PostalCode,
                City = user.City,
                AddressDetails = user.AddressDetails,
                SubscribedToNewsletter = user.SubscribedToNewsletter,
                AcceptedTerms = user.AcceptedTerms
            };
        }

        public async Task<string> RequestPasswordResetAsync(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<string> ResetPasswordAsync(string token, string newPassword)
        {
            throw new NotImplementedException();
        }
    }
}

