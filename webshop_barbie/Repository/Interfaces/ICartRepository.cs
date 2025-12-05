using webshop_barbie.Models;

namespace webshop_barbie.Repository.Interfaces
{
    public interface ICartRepository
    {
        // Egy adott felhasználó kosarának lekérése
        Task<Cart?> GetByUserIdAsync(int userId);

        // Új kosár hozzáadása
        Task AddAsync(Cart cart);

        // Meglévő kosár frissítése
        Task UpdateAsync(Cart cart);

        // Kosár törlése
        Task DeleteAsync(Cart cart);
    }
}


