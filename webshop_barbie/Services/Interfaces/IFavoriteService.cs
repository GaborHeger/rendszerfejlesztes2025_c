using webshop_barbie.DTOs;
using webshop_barbie.Models;

namespace webshop_barbie.Service.Interfaces
{
    public interface IFavoriteService
    {
        /// <summary>
        /// Egy adott felhasználó összes kedvenc termékének lekérése.
        /// </summary>
        /// <param name="userId">A felhasználó azonosítója.</param>
        /// <returns>A kedvenc termékeket tartalmazó FavoriteDTO lista.</returns>
        Task<IEnumerable<FavoriteDTO>> GetFavoritesByUserIdAsync(int userId);

        /// <summary>
        /// Termék hozzáadása a felhasználó kedvenceihez.
        /// </summary>
        /// <param name="userId">A felhasználó azonosítója.</param>
        /// <param name="productId">A hozzáadandó termék azonosítója.</param>
        /// <returns>Üzenet a sikeres vagy sikertelen műveletről.</returns>
        Task<string> AddFavoriteAsync(int userId, int productId);

        /// <summary>
        /// Termék eltávolítása a felhasználó kedvenceiből.
        /// </summary>
        /// <param name="userId">A felhasználó azonosítója.</param>
        /// <param name="productId">Az eltávolítandó termék azonosítója.</param>
        /// <returns>Üzenet a sikeres vagy sikertelen műveletről.</returns>
        Task<string> RemoveFavoriteAsync(int userId, int productId);

        /// <summary>
        /// Kedvenc termék áthelyezése a felhasználó kosarába.
        /// </summary>
        /// <param name="userId">A felhasználó azonosítója.</param>
        /// <param name="productId">A kosárba helyezendő termék azonosítója.</param>
        /// <returns>A frissített CartDTO.</returns>
        Task<CartDTO> AddFavoriteToCartAsync(int userId, int productId);
    }
}



