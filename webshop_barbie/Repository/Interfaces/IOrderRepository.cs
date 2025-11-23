using webshop_barbie.DTOs;
using webshop_barbie.Models;

namespace webshop_barbie.Repository.Interfaces
{
    public interface IOrderRepository
    {
        Task<string> PlaceOrder(OrderRequestDTO orderRequest, int userId);

        /// <summary>
        /// Egy adott felhasználó összes rendelésének lekérése.
        /// </summary>
        /// <param name="userId">A felhasználó azonosítója.</param>
        /// <returns>A felhasználó rendeléseinek gyűjteménye.</returns>
        Task<IEnumerable<Order>> GetByUserIdAsync(int userId);

        Task<IEnumerable<OrderItem>> GetByOrderIdAsync(int orderId);

        /// <summary>
        /// Új rendelés hozzáadása.
        /// </summary>
        /// <param name="order">A hozzáadandó rendelés.</param>
        Task AddAsync(Order order);

        /// <summary>
        /// Meglévő rendelés törlése.
        /// </summary>
        /// <param name="order">A törlendő rendelés.</param>
        Task DeleteAsync(Order order);
    }
}


