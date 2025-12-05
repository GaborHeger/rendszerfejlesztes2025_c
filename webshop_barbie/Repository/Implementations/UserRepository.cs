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

        // Felhasználó lekérdezése ID alapján
        public async Task<User?> GetByIdAsync(int userId)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        // Felhasználó lekérdezése email alapján
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        // Új felhasználó hozzáadása
        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        // Felhasználó adatainak frissítése DTO alapján
        // Visszatér null-lal, ha nincs találat vagy nincs változás
        public async Task<User?> UpdateUserAsync(int id, UserDTO userDto)
        {
            var existingUser = await _context.Users.FindAsync(id);

            if (existingUser == null)
                return null;

            existingUser.FirstName = userDto.FirstName;
            existingUser.LastName = userDto.LastName;
            existingUser.PhoneNumber = userDto.PhoneNumber;
            existingUser.PostalCode = userDto.PostalCode;
            existingUser.City = userDto.City;
            existingUser.AddressDetails = userDto.AddressDetails;
            existingUser.SubscribedToNewsletter = userDto.SubscribedToNewsletter;

            bool hasChanges = _context.Entry(existingUser).Properties.Any(p => p.IsModified);

            if (!hasChanges)
                return null;

            await _context.SaveChangesAsync();

            return existingUser;
        }

        // Felhasználó ellenőrzése bejelentkezéshez (email + jelszó)
        public async Task<User?> ValidateLoginAsync(string email, string passwordHash)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.PasswordHash == passwordHash);
        }
    }
}

