using webshop_barbie.Models;

namespace webshop_barbie.Repository.Interfaces
{
    public interface IOrderItemRepository
    {
        // Egy rendelés összes tételének lekérése
        Task<IEnumerable<OrderItem>> GetByOrderIdAsync(int orderId);

        // Új rendelés tétel hozzáadása
        Task AddAsync(OrderItem orderItem);
    }
}


