using webshop_barbie.Models;
using System.Linq;

namespace webshop_barbie.Repository.Interfaces
{
    public interface IProductRepository
    {
        // Összes termék lekérése (szűrés és DTO konverzió a service-ben)
        IQueryable<Product> GetAll();

        // Termék lekérése ID alapján
        Task<Product?> GetByIdAsync(int id);

        // Termék mentése és a készlet frissítése
        Task UpdateStockAsync(Product product);
    }
}
    