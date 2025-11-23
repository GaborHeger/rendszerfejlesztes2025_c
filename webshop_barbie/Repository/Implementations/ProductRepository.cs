using webshop_barbie.Models;
using webshop_barbie.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using webshop_barbie.Data;
using System.Linq;

namespace webshop_barbie.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly WebshopContext _context;

        public ProductRepository(WebshopContext context)
        {
            _context = context;
        }

        // Összes termék lekérdezése
        public IQueryable<Product> GetAll()
        {
            // IQueryable visszaadás → a service-ben lehet szűrni, DTO-t konvertálni
            return _context.Products.AsQueryable();
        }

        // Termék lekérdezése ID alapján
        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id); // ha nincs találat → null
        }

        // Stock frissítése
        public async Task UpdateStockAsync(Product product)
        {
            _context.Products.Update(product); // módosítás jelzése
            await _context.SaveChangesAsync(); // adatbázisba mentés
        }
    }
}


