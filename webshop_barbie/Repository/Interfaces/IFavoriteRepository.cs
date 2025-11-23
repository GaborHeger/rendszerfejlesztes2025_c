using webshop_barbie.Models;

namespace webshop_barbie.Repository.Interfaces
{
    public interface IFavoriteRepository
    {
        /// <summary>
        /// Egy adott felhasználó összes kedvenc elemének lekérése.
        /// </summary>
        /// <param name="userId">A felhasználó azonosítója.</param>
        /// <returns>A felhasználó kedvenc elemeinek gyűjteménye.</returns>
        Task<IEnumerable<Favorite>> GetAllAsync(int userId);

        /// <summary>
        /// Egy konkrét kedvenc elem lekérése felhasználó és termék alapján.
        /// </summary>
        /// <param name="userId">A felhasználó azonosítója.</param>
        /// <param name="productId">A termék azonosítója.</param>
        /// <returns>A kedvenc elem, ha létezik; különben null.</returns>
        Task<Favorite?> GetByIdAsync(int userId, int productId);

        /// <summary>
        /// Új kedvenc elem hozzáadása egy felhasználó számára.
        /// </summary>
        /// <param name="favorite">A hozzáadandó kedvenc elem.</param>
        /// <returns>A hozzáadott kedvenc elem.</returns>
        Task<Favorite> AddAsync(Favorite favorite);

        /// <summary>
        /// Kedvenc elem törlése egy adott felhasználó és termék alapján.
        /// </summary>
        /// <param name="userId">A felhasználó azonosítója.</param>
        /// <param name="productId">A termék azonosítója.</param>
        /// <returns>A törölt kedvenc elem, ha létezett; különben null.</returns>
        Task<Favorite?> DeleteAsync(int userId, int productId);
    }
}


