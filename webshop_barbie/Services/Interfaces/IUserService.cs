using webshop_barbie.Models;
using webshop_barbie.DTOs;

namespace webshop_barbie.Service.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Egy felhasználó adatainak lekérése az egyedi azonosítója alapján.
        /// </summary>
        /// <param name="userId">A felhasználó azonosítója.</param>
        /// <returns>Olyan DTO, amely tartalmazza a felhasználó adatait.</returns>
        Task<UserDTO> GetUserByIdAsync(int userId);

        /// <summary>
        /// Egy felhasználó adatainak lekérése az email címe alapján.
        /// </summary>
        /// <param name="email">A felhasználó email címe.</param>
        /// <returns>Olyan DTO, amely tartalmazza a felhasználó adatait.</returns>
        Task<UserDTO> GetUserByEmailAsync(string email);

        /// <summary>
        /// Új felhasználó létrehozása a rendszerben.
        /// </summary>
        /// <param name="user">A létrehozandó felhasználó entitás.</param>
        /// <returns>DTO a létrehozott felhasználóról.</returns>
        Task<UserDTO> AddUserAsync(RegisterRequestDTO user);

        /// <summary>
        /// Meglévő felhasználó adatainak frissítése.
        /// </summary>
        /// <param name="user">A frissített felhasználó entitás.</param>
        /// <returns>DTO a frissített felhasználóról.</returns>
        Task<UserDTO> UpdateUserAsync(UpdateUserRequestDTO user);

        /// <summary>
        /// Felhasználói bejelentkezési adatok ellenőrzése és JWT token generálása.
        /// </summary>
        /// <param name="email">A felhasználó email címe.</param>
        /// <param name="password">A felhasználó jelszava (plain-text).</param>
        /// <returns>Válasz DTO, amely tartalmazza a hitelesített felhasználó adatait és a generált JWT tokent.</returns>
        Task<LoginResponseDTO> ValidateLoginAsync(string email, string password);

        /// <summary>
        /// Jelszó visszaállítás kezdeményezése: token generálása és elküldése a felhasználó email címére.
        /// </summary>
        /// <param name="email">A felhasználóhoz tartozó email cím.</param>
        /// <returns>Visszaigazoló üzenet vagy hiba oka.</returns>
        Task<string> RequestPasswordResetAsync(string email);

        /// <summary>
        /// Felhasználói jelszó visszaállítása érvényes jelszó-visszaállító token segítségével.
        /// </summary>
        /// <param name="token">Korábban generált jelszó-visszaállító token.</param>
        /// <param name="newPassword">Az új jelszó.</param>
        /// <returns>Visszaigazoló üzenet a sikeres vagy sikertelen műveletről.</returns>
        Task<string> ResetPasswordAsync(string token, string newPassword);
    }
}



