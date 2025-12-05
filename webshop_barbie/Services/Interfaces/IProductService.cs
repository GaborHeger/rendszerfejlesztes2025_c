using webshop_barbie.Models;
using webshop_barbie.DTOs;

namespace webshop_barbie.Service.Interfaces
{
    public interface IProductService
    {
        // Összes termék lekérése, opcionális kategória szűréssel
        Task<IEnumerable<ProductDTO>> GetAllProductsAsync(string? category = null);

        // Termék lekérése ID alapján
        Task<Product?> GetByIdAsync(int id);

        // Egy adott termék aktuális készletének lekérése
        Task<int> GetStockAsync(int productId);

        // Ellenőrzi, hogy van-e elegendő készlet a kérdezett mennyiséghez
        Task<(bool IsAvailable, int AvailableStock)> CheckStockAsync(int productId, int requestedQuantity);

        // Csökkenti a termékek készletét a rendelés mennyisége szerint
        Task DecreaseStockAsync(Order order);
    }
}

