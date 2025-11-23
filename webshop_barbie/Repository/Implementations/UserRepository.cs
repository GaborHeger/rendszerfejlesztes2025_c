using Microsoft.EntityFrameworkCore;
using webshop_barbie.Data;
using webshop_barbie.DTOs;
using webshop_barbie.Models;
using webshop_barbie.Repository.Interfaces;

namespace webshop_barbie.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly WebshopContext _context;

        public UserRepository(WebshopContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByIdAsync(int userId)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> UpdateUserAsync(int id, UserDTO userDto)
        {
            //lekérjük az entitást
            var existingUser = await _context.Users.FindAsync(id);

            //ha nincs találat, repository visszaad null-t
            if (existingUser == null)
                return null;

            //DTO mezők másolása az entitásra, null is lehet
            existingUser.FirstName = userDto.FirstName;
            existingUser.LastName = userDto.LastName;
            existingUser.PhoneNumber = userDto.PhoneNumber;
            existingUser.PostalCode = userDto.PostalCode;
            existingUser.City = userDto.City;
            existingUser.AddressDetails = userDto.AddressDetails;
            existingUser.SubscribedToNewsletter = userDto.SubscribedToNewsletter;

            //ellenőrzi, hogy van-e változás
            bool hasChanges = _context.Entry(existingUser).Properties.Any(p => p.IsModified);

            if (!hasChanges)
                return null; //nincs változás, nem mentünk, null-t adunk vissza

            await _context.SaveChangesAsync(); //mentés, null értékek is felülírják az adatbázist

            return existingUser;
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> ValidateLoginAsync(string email, string passwordHash)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.PasswordHash == passwordHash);
        }
    }
}

