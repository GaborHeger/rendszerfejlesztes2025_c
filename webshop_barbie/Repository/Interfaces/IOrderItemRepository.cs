using webshop_barbie.Models;

namespace webshop_barbie.Repository.Interfaces
{
    public interface IOrderItemRepository
    {
        /// <summary>
        /// Egy adott rendelés összes rendelési tételének lekérése.
        /// </summary>
        /// <param name="orderId">A rendelés azonosítója.</param>
        /// <returns>A rendelési tételek gyűjteménye.</returns>
        Task<IEnumerable<OrderItem>> GetByOrderIdAsync(int orderId);

        /// <summary>
        /// Új rendelési tétel hozzáadása.
        /// </summary>
        /// <param name="orderItem">A hozzáadandó rendelési tétel.</param>
        Task AddAsync(OrderItem orderItem);
    }
}


