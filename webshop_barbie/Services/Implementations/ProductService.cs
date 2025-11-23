using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using webshop_barbie.Data;
using webshop_barbie.DTOs;
using webshop_barbie.Models;
using webshop_barbie.Repository.Interfaces;
using webshop_barbie.Service.Interfaces;
using System.Linq;
using System;

namespace webshop_barbie.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        // Összes termék lekérése, opcionális kategória szűréssel
        public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync(string? category = null)
        {
            var query = _repository.GetAll(); // IQueryable<Product>

            if (!string.IsNullOrEmpty(category))
            {
                // Enum konverzió ellenőrzése
                if (!Enum.TryParse<Category>(category, true, out var categoryEnum))
                    throw new ArgumentException($"A '{category}' kategória nem érvényes.");

                query = query.Where(p => p.Category == categoryEnum);
            }

            // DTO konverzió és lista lekérése aszinkron
            var products = await query
                .Select(p => new ProductDTO
                {
                    ProductId = p.Id,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    Stock = p.Stock,
                    Category = p.Category
                })
                .ToListAsync();

            return products;
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        // Egy termék készletének lekérése
        public async Task<int> GetStockAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
                throw new KeyNotFoundException($"A termék ({id}) nem található.");

            return product.Stock;
        }

        // Ellenőrzi a készletet, és visszaadja, hogy van e elég és, hogy hány termék van
        public async Task<(bool IsAvailable, int AvailableStock)> CheckStockAsync(int productId, int requestedQuantity)
        {
            var product = await _repository.GetByIdAsync(productId);

            if (product == null)
                throw new KeyNotFoundException($"A termék ({productId}) nem található.");

            bool isAvailable = product.Stock >= requestedQuantity;

            // Nem módosítjuk a stock-ot, csak visszaadjuk az elérhető mennyiséget
            return (isAvailable, product.Stock);
        }

    }
}