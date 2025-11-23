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
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IProductService _productService;
        private readonly IUserService _userService;

        public CartService(ICartRepository cartRepository, 
            ICartItemRepository _cartItemRepository, 
            IProductService productService,
            IUserService userService)
        {
            _cartRepository = cartRepository;
            _cartItemRepository = _cartItemRepository;
            _productService = productService;
            _userService = userService;
        }

        public async Task<CartDTO> GetCartByUserIdAsync(int userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"A felhasználó nem található.");

            var cart = await _cartRepository.GetByUserIdAsync(userId);
            if (cart == null)
                throw new KeyNotFoundException($"A kosár a felhasználóhoz nem található.");

            var cartDto = new CartDTO
            {
                CartId = cart.Id,
                UserId = cart.UserId,
                Items = new List<CartItemDTO>()
            };

            foreach (var item in cart.CartItems)
            {
                int stock = await _productService.GetStockAsync(item.ProductId); //aktuális raktárkészlet
                cartDto.Items.Add(new CartItemDTO
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product?.ProductName,
                    Price = item.Product?.Price ?? 0,
                    Quantity = item.Quantity, //kosárban lévő mennyiség
                    imageUrl = item.Product?.ImageUrl,
                    Stock = stock             //termék tényleges készlete
                });
            }

            return cartDto;
        }

        //public async Task<CartDTO> AddItemToCartAsync(int userId, int productId, int quantity)
        //{
            //var user = await _userService.GetUserByIdAsync(userId);
            //if (user == null)
            //    throw new KeyNotFoundException($"A felhasználó nem található!");

            //var products = await _productService.GetAllProductsAsync();

            //try
            //{
            //    foreach (var item in products)
            //    {
            //        if (item.ProductId == productId){
            //            break;
            //        }
            //    }
            //}
            //catch (KeyNotFoundException ex)
            //{
            //    throw new KeyNotFoundException("A termék nem található!");
            //}

            //var stock = await _productService.GetStockAsync(productId);
            //if(stock == null)
            //throw new KeyNotFoundException($"Nincs készleten!");

            //var cart = await _cartRepository.GetByUserIdAsync(userId);
            //if (cart == null)
            //    throw new KeyNotFoundException($"Nincs készleten!");

            //bool isProductInCart = false;
            //foreach(var item in cart.CartItems)
            //{
            //    if (item.ProductId == productId) 
            //    {
            //        item.Quantity++;
            //        isProductInCart = true;
            //        var stockState = await _productService.CheckStockAsync(productId, item.Quantity);
            //        if(!stockState.IsAvailable)
            //        {
            //            throw new KeyNotFoundException($"Nincs készleten elég! Készleten van: {stockState.AvailableStock}");
            //        }
            //    }
            //}
            //if(!isProductInCart)
            //{
            //    var product = products.FirstOrDefault(x => x.ProductId == productId);

            //    if (product == null)
            //    {
            //        throw new KeyNotFoundException("A termék nem található!");
            //    }

            //    var newCartItem = new CartItem
            //    {
            //        Id = 0,
            //        Quantity = quantity,
            //        Price = product.Price,
            //        ProductId = productId,
            //        Product = product,
            //        imageUrl = product.ImageUrl,
            //        Stock = stock       
            //    };

            //    cart.CartItems.Add(newCartItem);
            //}
            //await _cartRepository.UpdateAsync(cart);

            //var cartDto = new CartDTO
            //{
            //    CartId = cart.Id,
            //    UserId = cart.UserId,
            //    Items = new List<CartItemDTO>()
            //};

            //foreach (var item in cart.CartItems)
            //{
            //    int stock = await _productService.GetStockAsync(item.ProductId); //aktuális raktárkészlet
            //    cartDto.Items.Add(new CartItemDTO
            //    {
            //        ProductId = item.ProductId,
            //        ProductName = item.Product?.ProductName,
            //        Price = item.Product?.Price ?? 0,
            //        Quantity = item.Quantity, //kosárban lévő mennyiség
            //        imageUrl = item.Product?.ImageUrl,
            //        Stock = stock             //termék tényleges készlete
            //    });
            //}

            //return cartDto;

            //throw new NotImplementedException();
        //}

        public async Task<CartDTO> AddItemToCartAsync(int userId, int productId, int quantity)
        {
            //felhasználó ellenőrzése
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"A felhasználó nem található!");

            //termék lekérése
            var product = await _productService.GetByIdAsync(productId);
            if (product == null)
                throw new KeyNotFoundException("A termék nem található!");

            //stock ellenőrzés
            if (quantity > product.Stock)
                throw new InvalidOperationException($"Nincs elég készleten! Készleten van: {product.Stock}");

            //kosár lekérése
            var cart = await _cartRepository.GetByUserIdAsync(userId);
            if (cart == null)
            {
                //ha nincs kosár, létrehozunk egy újat
                cart = new Cart
                {
                    UserId = userId,
                    CartItems = new List<CartItem>()
                };
            }

            //ellenőrizzük, hogy a termék már benne van-e a kosárban
            var cartItem = cart.CartItems.FirstOrDefault(i => i.ProductId == productId);
            if (cartItem != null)
            {
                //frissítjük a mennyiséget
                int newQuantity = cartItem.Quantity + quantity;
                if (newQuantity > product.Stock)
                    throw new InvalidOperationException($"Nincs elég készleten! Készleten van: {product.Stock}");

                cartItem.Quantity = newQuantity;
            }
            else
            {
                //új CartItem létrehozása
                cartItem = new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    Price = product.Price,
                    Product = product
                };
                cart.CartItems.Add(cartItem);
            }

            //kosár mentése az adatbázisba
            if (cart.Id == 0)
                await _cartRepository.AddAsync(cart);   //új kosár
            else
                await _cartRepository.UpdateAsync(cart); //meglévő kosár frissítése

            //DTO visszaadása
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
            //user ellenőrzés
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("A felhasználó nem található.");

            //cart ellenőrzés
            var cart = await _cartRepository.GetByUserIdAsync(userId);
            if (cart == null)
                throw new KeyNotFoundException("A kosár nem található.");

            //item keresése a kosárban
            var item = cart.CartItems.FirstOrDefault(i => i.ProductId == productId);
            if (item == null)
                throw new KeyNotFoundException("A termék nem található a kosárban.");

            //elem eltávolítása
            cart.CartItems.Remove(item);

            //adatbázis frissítése
            await _cartRepository.UpdateAsync(cart);

            //friss DTO visszaadása
            return await GetCartByUserIdAsync(userId);
        }

            public async Task<CartDTO> UpdateCartItemQuantityAsync(int userId, int productId, int quantity)
            {
                //user ellenőrzése
                var user = await _userService.GetUserByIdAsync(userId);
                if (user == null)
                    throw new KeyNotFoundException($"A felhasználó nem található!");

                //termék lekérése
                var product = await _productService.GetByIdAsync(productId);
                if (product == null)
                    throw new KeyNotFoundException("A termék nem található!");

                //stock ellenőrzés
                if (quantity > product.Stock)
                    throw new InvalidOperationException($"Nincs elég készleten! Készleten van: {product.Stock}");

                //kosár lekérése
                var cart = await _cartRepository.GetByUserIdAsync(userId);
                if (cart == null)
                    throw new KeyNotFoundException("A kosár üres.");

                //termék mennyiségének frissítése
                var cartItem = cart.CartItems.FirstOrDefault(i => i.ProductId == productId);
                if (cartItem == null)
                {
                    //ha a termék nincs a kosárban, hozzáadjuk
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
                    //ha már van, beállítjuk az új mennyiséget
                    cartItem.Quantity = quantity;
                }

                //kosár frissítése az adatbázisban
                await _cartRepository.UpdateAsync(cart);

                //DTO visszaadása
                return await GetCartByUserIdAsync(userId);

                //throw new NotImplementedException();
            }

        public async Task<string> ClearCartAsync(int userId)
        {
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