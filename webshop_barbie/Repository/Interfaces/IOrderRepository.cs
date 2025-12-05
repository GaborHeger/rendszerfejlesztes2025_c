using webshop_barbie.DTOs;
using webshop_barbie.Models;

namespace webshop_barbie.Repository.Interfaces
{
    public interface IOrderRepository
    {

        // Egy felhasználó összes rendelésének lekérése
        Task<IEnumerable<Order>> GetByUserIdAsync(int userId);

        // Egy rendelés összes tételének lekérése
        Task<IEnumerable<OrderItem>> GetByOrderIdAsync(int orderId);

        // Új rendelés hozzáadása
        Task AddAsync(Order order);

        // Rendelés törlése
        Task DeleteAsync(Order order);
    }
}


