using webshop_barbie.DTOs;
using webshop_barbie.Models;

namespace webshop_barbie.Repository.Interfaces
{
    public interface IUserRepository
    {
        /// <summary>
        /// Egy felhasználó lekérése az egyedi azonosítója alapján.
        /// </summary>
        /// <param name="userId">A felhasználó azonosítója.</param>
        /// <returns>A felhasználó, ha létezik; különben null.</returns>
        Task<User?> GetByIdAsync(int userId);

        /// <summary>
        /// Egy felhasználó lekérése az email címe alapján.
        /// </summary>
        /// <param name="email">A felhasználó email címe.</param>
        /// <returns>A felhasználó, ha létezik; különben null.</returns>
        Task<User?> GetByEmailAsync(string email);

        /// <summary>
        /// Új felhasználó hozzáadása a tárolóhoz.
        /// </summary>
        /// <param name="user">A hozzáadandó felhasználó.</param>
        Task AddAsync(User user);

        /// <summary>
        /// Meglévő felhasználó adatainak frissítése.
        /// </summary>
        /// <param name="user">A frissített felhasználó.</param>
        Task<User?> UpdateUserAsync(int id, UserDTO userDto);

        /// <summary>
        /// Felhasználói bejelentkezési adatok ellenőrzése.
        /// </summary>
        /// <param name="email">A felhasználó email címe.</param>
        /// <param name="passwordHash">A felhasználó jelszavának hash értéke.</param>
        /// <returns>A felhasználó, ha a hitelesítés sikeres; különben null.</returns>
        Task<User?> ValidateLoginAsync(string email, string passwordHash);
    }
}
