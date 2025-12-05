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
            var query = _repository.GetAll();

            if (!string.IsNullOrEmpty(category))
            {
                if (!Enum.TryParse<Category>(category, true, out var categoryEnum))
                    throw new ArgumentException($"A '{category}' kategória nem érvényes.");

                query = query.Where(p => p.Category == categoryEnum);
            }

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

        // Ellenőrzi, hogy van-e elég készlet a kért mennyiséghez
        public async Task<(bool IsAvailable, int AvailableStock)> CheckStockAsync(int productId, int requestedQuantity)
        {
            var product = await _repository.GetByIdAsync(productId);

            if (product == null)
                throw new KeyNotFoundException($"A termék ({productId}) nem található.");

            bool isAvailable = product.Stock >= requestedQuantity;

            return (isAvailable, product.Stock);
        }

        // Csökkenti az egyes termékek készletét a rendelés mennyisége szerint
        public async Task DecreaseStockAsync(Order order)
        {
            foreach (var item in order.OrderItems)
            {
                var product = await _repository.GetByIdAsync(item.ProductId);

                if (product == null)
                    throw new KeyNotFoundException($"A termék azonosítóval {item.ProductId} nem található.");

                product.Stock -= item.Quantity;

                await _repository.UpdateStockAsync(product);
            }
        }
    }
}