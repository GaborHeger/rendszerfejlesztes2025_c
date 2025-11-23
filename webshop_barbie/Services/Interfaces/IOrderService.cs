using webshop_barbie.DTOs;
using webshop_barbie.Models;

namespace webshop_barbie.Service.Interfaces
{
    public interface IOrderService
    {
        /// <summary>
        /// Egy adott felhasználó összes rendelésének lekérése.
        /// A frontend hívja, amikor betöltődik a felhasználó rendeléseit tartalmazó oldal.
        /// </summary>
        /// <param name="userId">A felhasználó azonosítója, akinek a rendelései kellenek.</param>
        /// <returns>A felhasználó rendeléseit tartalmazó OrderDTO gyűjtemény.</returns>
        Task<IEnumerable<OrderDTO>> GetOrdersByUserIdAsync(int userId);

        /// <summary>
        /// Új rendelés létrehozása egy felhasználó számára.
        /// A frontend hívja, amikor a felhasználó elküldi a rendelési űrlapot.
        /// </summary>
        /// <param name="userId">A rendelést leadó felhasználó azonosítója.</param>
        /// <param name="orderRequest">A felhasználó által megadott szállítási és fizetési adatok.</param>
        /// <returns>A létrehozott rendelés OrderDTO formátumban.</returns>
        Task<OrderDTO> CreateOrderAsync(int userId, OrderRequestDTO orderRequestDTO);
    }
}

