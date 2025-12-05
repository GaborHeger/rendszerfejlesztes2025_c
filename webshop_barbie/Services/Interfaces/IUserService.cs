using webshop_barbie.Models;
using webshop_barbie.DTOs;

namespace webshop_barbie.Service.Interfaces
{
    public interface IUserService
    {
        // Egy felhasználó adatainak lekérése ID alapján
        Task<UserDTO> GetUserByIdAsync(int userId);

        // Egy felhasználó adatainak lekérése email alapján
        Task<UserDTO> GetUserByEmailAsync(string email);

        // Új felhasználó létrehozása
        Task<UserDTO> AddUserAsync(RegisterRequestDTO user);

        // Meglévő felhasználó adatainak frissítése
        Task<UserDTO> UpdateUserAsync(UpdateUserRequestDTO user);

        // Bejelentkezési adatok ellenőrzése és JWT generálása
        Task<LoginResponseDTO> ValidateLoginAsync(string email, string password);

        // Jelszó visszaállítás kezdeményezése (token küldése emailben)
        Task<string> RequestPasswordResetAsync(string email);

        // Jelszó visszaállítása érvényes token segítségével
        Task<string> ResetPasswordAsync(string token, string newPassword);
    }
}



