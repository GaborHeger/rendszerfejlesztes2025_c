using webshop_barbie.Models;

namespace webshop_barbie.Repository.Interfaces
{
    public interface IFavoriteRepository
    {
        // Egy felhasználó összes kedvencének lekérése
        Task<IEnumerable<Favorite>> GetAllAsync(int userId);

        // Konkrét kedvenc lekérése felhasználó és termék alapján
        Task<Favorite?> GetByIdAsync(int userId, int productId);

        // Új kedvenc hozzáadása
        Task<Favorite> AddAsync(Favorite favorite);

        // Kedvenc törlése felhasználó és termék alapján
        Task<Favorite?> DeleteAsync(int userId, int productId);
    }
}


