using webshop_barbie.DTOs;
using webshop_barbie.Models;

namespace webshop_barbie.Service.Interfaces
{
    public interface IFavoriteService
    {
        // Lekéri egy felhasználó összes kedvenc termékét
        Task<IEnumerable<FavoriteDTO>> GetFavoritesByUserIdAsync(int userId);

        // Hozzáad egy terméket a kedvencekhez
        Task<string> AddFavoriteAsync(int userId, int productId);

        // Eltávolít egy terméket a kedvencekből
        Task<string> RemoveFavoriteAsync(int userId, int productId);

        // Kedvenc terméket áthelyez a kosárba
        Task<CartDTO> AddFavoriteToCartAsync(int userId, int productId);
    }
}



