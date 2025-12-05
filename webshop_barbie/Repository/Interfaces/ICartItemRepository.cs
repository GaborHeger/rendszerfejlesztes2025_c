using webshop_barbie.Models;

namespace webshop_barbie.Repository.Interfaces
{
    public interface ICartItemRepository
    {
        // Kosár tétel hozzáadása
        Task AddAsync(CartItem item);

        // Kosár tétel frissítése
        Task UpdateAsync(CartItem cartItem);

        // Kosár tétel törlése ID alapján
        Task DeleteAsync(int cartItemId);
    }
}
