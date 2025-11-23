using webshop_barbie.Models;
using System.Linq;

namespace webshop_barbie.Repository.Interfaces
{
    public interface IProductRepository
    {
        /// <summary>
        /// Az összes termék lekérése.
        /// A service felel a szűrésért és DTO konverzióért.
        /// </summary>
        IQueryable<Product> GetAll();

        /// <summary>
        /// Egy termék lekérése az egyedi azonosítója alapján.
        /// </summary>
        /// <param name="id">A termék azonosítója.</param>
        /// <returns>A termék, ha létezik; különben null.</returns>
        Task<Product?> GetByIdAsync(int id);

        /// <summary>
        /// Egy termék mentése / készlet frissítése.
        /// </summary>
        /// <param name="product">A módosított termék entitás.</param>
        Task UpdateStockAsync(Product product);
    }
}
    