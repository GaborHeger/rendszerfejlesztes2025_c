using webshop_barbie.DTOs;
using webshop_barbie.Models;

namespace webshop_barbie.Service.Interfaces
{
    public interface IOrderService
    {
        // A felhasználó összes rendelésének lekérése
        Task<IEnumerable<OrderDTO>> GetOrdersByUserIdAsync(int userId);

        // Új rendelés létrehozása a felhasználó kosara alapján
        Task<OrderDTO> CreateOrderAsync(int userId, OrderRequestDTO orderRequestDTO);

        // Rendelés érvényességének ellenőrzése
        Task ValidateOrderAsync(int userId, OrderRequestDTO orderRequestDTO);
    }
}

