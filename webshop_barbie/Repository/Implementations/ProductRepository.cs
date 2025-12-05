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
        // IQueryable visszaadás -> a service rétegben lehet szűrni vagy DTO-ra konvertálni
        public IQueryable<Product> GetAll()
        {
            return _context.Products.AsQueryable();
        }

        // Termék lekérdezése ID alapján
        // Ha nincs találat, visszatér null-lal
        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        // Termék készletének frissítése
        public async Task UpdateStockAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
    }
}
