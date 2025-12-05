using webshop_barbie.DTOs;
using webshop_barbie.Models;

namespace webshop_barbie.Repository.Interfaces
{
    public interface IUserRepository
    {
        // Felhasználó lekérése ID alapján
        Task<User?> GetByIdAsync(int userId);

        // Felhasználó lekérése email alapján
        Task<User?> GetByEmailAsync(string email);

        // Új felhasználó hozzáadása
        Task AddAsync(User user);

        // Felhasználó adatainak frissítése DTO alapján
        Task<User?> UpdateUserAsync(int id, UserDTO userDto);

        // Felhasználó ellenőrzése bejelentkezéshez
        Task<User?> ValidateLoginAsync(string email, string passwordHash);
    }
}
