using webshop_barbie.Models;

namespace webshop_barbie.Repository.Interfaces
{
    public interface ICartItemRepository
    {
        /// <summary>
        /// Új elem hozzáadása a kosárhoz.
        /// </summary>
        /// <param name="item">A hozzáadandó CartItem.</param>
        Task AddAsync(CartItem item);

        /// <summary>
        /// Meglévő kosár elem frissítése.
        /// </summary>
        /// <param name="cartItem">A frissített CartItem.</param>
        Task UpdateAsync(CartItem cartItem);

        /// <summary>
        /// Kosár elem törlése az ID alapján.
        /// </summary>
        /// <param name="cartItemId">A törlendő CartItem azonosítója.</param>
        Task DeleteAsync(int cartItemId);
    }
}
