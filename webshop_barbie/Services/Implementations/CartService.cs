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
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductService _productService;
        private readonly IUserService _userService;

        public CartService(ICartRepository cartRepository, 
            IProductService productService, IUserService userService)
        {
            _cartRepository = cartRepository;
            _productService = productService;
            _userService = userService;
        }

        public async Task<CartDTO> GetCartByUserIdAsync(int userId)
        {
            // Felhasználó és a kosár ellenőrzése
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"A felhasználó nem található.");

            var cart = await _cartRepository.GetByUserIdAsync(userId);
            if (cart == null)
                throw new KeyNotFoundException($"A kosár a felhasználóhoz nem található.");

            // DTO létrehozása: minden kosár elem DTO-vá alakítása
            var cartDto = new CartDTO
            {
                CartId = cart.Id,
                UserId = cart.UserId,
                Items = cart.CartItems.Select(item => new CartItemDTO
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product?.ProductName ?? "",
                    Price = item.Product?.Price ?? 0,
                    Quantity = item.Quantity,
                    imageUrl = item.Product?.ImageUrl ?? "",
                    Stock = item.Product?.Stock ?? 0
                }).ToList()
            };
            return cartDto;
        }

        public async Task<CartDTO> AddItemToCartAsync(int userId, int productId, int quantity)
        {
            // Felhasználó és termék ellenőrzése
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"A felhasználó nem található!");

            var product = await _productService.GetByIdAsync(productId);
            if (product == null)
                throw new KeyNotFoundException("A termék nem található!");

            if (quantity > product.Stock)
                throw new InvalidOperationException($"Nincs elég készleten! Készleten van: {product.Stock}");

            // Lekérjük a kosarat, ha nincs, létrehozunk egy újat
            var cart = await _cartRepository.GetByUserIdAsync(userId);
            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CartItems = new List<CartItem>()
                };
            }

            // Ellenőrizzük, hogy a termék már a kosárban van-e
            var cartItem = cart.CartItems.FirstOrDefault(i => i.ProductId == productId);
            if (cartItem != null)
            {
                int newQuantity = cartItem.Quantity + quantity;
                if (newQuantity > product.Stock)
                    throw new InvalidOperationException($"Nincs elég készleten! Készleten van: {product.Stock}");

                cartItem.Quantity = newQuantity;
            }
            else
            {
                cartItem = new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    Price = product.Price,
                    Product = product
                };
                cart.CartItems.Add(cartItem);
            }

            if (cart.Id == 0)
                await _cartRepository.AddAsync(cart);
            else
                await _cartRepository.UpdateAsync(cart);

            var cartDto = new CartDTO
            {
                CartId = cart.Id,
                UserId = cart.UserId,
                Items = cart.CartItems.Select(i => new CartItemDTO
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product?.ProductName ?? "",
                    Price = i.Price,
                    Quantity = i.Quantity,
                    imageUrl = i.Product?.ImageUrl ?? "",
                    Stock = i.Product?.Stock ?? 0
                }).ToList()
            };

            return cartDto;
        }


        public async Task<CartDTO> RemoveItemFromCartAsync(int userId, int productId)
        {
            // Felhasználó és a kosarának ellenőrzése
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("A felhasználó nem található.");

            var cart = await _cartRepository.GetByUserIdAsync(userId);
            if (cart == null)
                throw new KeyNotFoundException("A kosár nem található.");

            // Megkeressük a terméket a kosárban
            var item = cart.CartItems.FirstOrDefault(i => i.ProductId == productId);
            if (item == null)
                throw new KeyNotFoundException("A termék nem található a kosárban.");

            cart.CartItems.Remove(item);

            await _cartRepository.UpdateAsync(cart);

            return await GetCartByUserIdAsync(userId);
        }

            public async Task<CartDTO> UpdateCartItemQuantityAsync(int userId, int productId, int quantity)
            {
                // A felhasználó, termékek, készlet ellenőrzése
                var user = await _userService.GetUserByIdAsync(userId);
                if (user == null)
                    throw new KeyNotFoundException($"A felhasználó nem található!");

                var product = await _productService.GetByIdAsync(productId);
                if (product == null)
                    throw new KeyNotFoundException("A termék nem található!");

                if (quantity > product.Stock)
                    throw new InvalidOperationException($"Nincs elég készleten! Készleten van: {product.Stock}");

                var cart = await _cartRepository.GetByUserIdAsync(userId);
                if (cart == null)
                    throw new KeyNotFoundException("A kosár üres.");

                // Termék mennyiségének frissítése vagy hozzáadása, ha nincs a kosárban
                var cartItem = cart.CartItems.FirstOrDefault(i => i.ProductId == productId);
                if (cartItem == null)
                {
                    cartItem = new CartItem
                    {
                        ProductId = productId,
                        Quantity = quantity,
                        Price = product.Price,
                        CartId = cart.Id
                    };
                    cart.CartItems.Add(cartItem);
                }
                else
                {
                    cartItem.Quantity = quantity;
                }

                await _cartRepository.UpdateAsync(cart);

                return await GetCartByUserIdAsync(userId);
            }

        public async Task<string> ClearCartAsync(int userId)
        {
            // Felhasználó és a kosár ellenőrzése
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"A felhasználó nem található!");

            var cart = await _cartRepository.GetByUserIdAsync(userId);
            if (cart == null)
                throw new KeyNotFoundException("Nincs termék a felhasználó kosarában!");

            await _cartRepository.DeleteAsync(cart);

            return "Sikeres a kosár ürítése!";
        }
    }
}