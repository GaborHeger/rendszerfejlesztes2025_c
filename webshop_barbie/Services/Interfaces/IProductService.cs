using webshop_barbie.Models;
using webshop_barbie.DTOs;

namespace webshop_barbie.Service.Interfaces
{
    public interface IProductService
    {
        /// <summary>
        /// Az összes termék lekérése, opcionálisan kategória alapján szűrve.
        /// A frontend hívja a terméklista betöltésekor vagy kategória szerinti szűréskor.
        /// </summary>
        /// <param name="category">Opcionális kategória szűrő, pl. "Barbie" vagy "Kiegészítő".</param>
        /// <returns>A termékeket tartalmazó ProductDTO gyűjtemény.</returns>
        Task<IEnumerable<ProductDTO>> GetAllProductsAsync(string? category = null);

        Task<Product?> GetByIdAsync(int id);

        /// <summary>
        /// Egy adott termék aktuális készletének lekérése.
        /// A frontend használja a készletinformáció megjelenítésére vagy a hozzáadás kosárhoz előtti ellenőrzésre.
        /// </summary>
        /// <param name="productId">A termék azonosítója.</param>
        /// <returns>Az elérhető készlet mennyisége.</returns>
        Task<int> GetStockAsync(int productId);

        /// <summary>
        /// Ellenőrzi, hogy van-e elegendő készlet egy adott mennyiséghez.
        /// A frontend használja a kosárba helyezés vagy rendelés létrehozása előtt.
        /// </summary>
        /// <param name="productId">A termék azonosítója.</param>
        /// <param name="quantity">A kért mennyiség.</param>
        /// <returns>Igaz, ha elegendő készlet van, különben hamis.</returns>
        Task<(bool IsAvailable, int AvailableStock)> CheckStockAsync(int productId, int requestedQuantity);

        Task DecreaseStockAsync(Order order);
    }
}

