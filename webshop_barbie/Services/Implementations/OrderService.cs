using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webshop_barbie.Data;
using webshop_barbie.DTOs;
using webshop_barbie.Models;
using webshop_barbie.Repository;
using webshop_barbie.Repository.Interfaces;
using webshop_barbie.Service.Interfaces;

namespace webshop_barbie.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserService _userService;
        private readonly ICartService _cartService;
        private readonly IProductService _productService;

        public OrderService(IOrderRepository orderRepository, IUserService userService, ICartService cartService, IProductService productService)
        {
            _orderRepository = orderRepository;
            _userService = userService;
            _cartService = cartService;
            _productService = productService;
        }

        public async Task<IEnumerable<OrderDTO>> GetOrdersByUserIdAsync(int userId)
        {
            // Ellenőrizzük, hogy a felhasználó létezik
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"A felhasználó ({userId}) nem található.");

            // Lekérjük a felhasználó rendeléseit
            var orders = await _orderRepository.GetByUserIdAsync(userId);

            var orderDtos = new List<OrderDTO>();

            foreach (var order in orders)
            {
                // Lekérjük a rendeléshez tartozó tételeket
                var orderItems = await _orderRepository.GetByOrderIdAsync(order.Id);

                var orderItemDtos = orderItems.Select(item => new OrderItemDTO
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product?.ProductName ?? "N/A",
                    Quantity = item.Quantity,
                    Price = item.Price
                }).ToList();

                // Összeállítjuk az OrderDTO-t
                orderDtos.Add(new OrderDTO
                {
                    OrderId = order.Id,
                    UserId = order.UserId,
                    ShippingMethod = order.ShippingMethod,
                    PaymentMethod = order.PaymentMethod,
                    ShippingFee = order.ShippingFee ?? 0,
                    TotalAmount = order.TotalAmount,
                    Items = orderItemDtos
                });
                throw new NotImplementedException();
            }

            return orderDtos;
        }

        public async Task<OrderDTO> CreateOrderAsync(int userId, OrderRequestDTO orderRequestDTO)
        {
            //Felhasználó ellenőrzése
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"A felhasználó nem található.");

            //kosár lekérése
            var cart = await _cartService.GetCartByUserIdAsync(userId);
            //Any() megnézi, hogy a collection tartalmaz-e legalább 1 elemet
            if (cart == null || !cart.Items.Any())
                throw new InvalidOperationException("A kosár üres, nem lehet rendelést létrehozni.");

            var updateUserRequestDTO = new UpdateUserRequestDTO
            {
                Id = userId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                PostalCode = user.PostalCode,
                City = user.City,
                AddressDetails = user.AddressDetails,
                SubscribedToNewsletter = user.SubscribedToNewsletter,
                AcceptedTerms = user.AcceptedTerms,
            };
            var userData = await _userService.UpdateUserAsync(updateUserRequestDTO);
            if (string.IsNullOrWhiteSpace(user.FirstName) ||
                string.IsNullOrWhiteSpace(user.LastName) ||
                string.IsNullOrWhiteSpace(user.PhoneNumber) ||
                string.IsNullOrWhiteSpace(user.PostalCode) ||
                string.IsNullOrWhiteSpace(user.City) ||
                string.IsNullOrWhiteSpace(user.AddressDetails) ||
                user.SubscribedToNewsletter == null ||  // fontos, hogy ne legyen null
                !user.AcceptedTerms)                     // kötelező elfogadni
                    {
                        throw new InvalidOperationException("A rendeléshez minden kötelező adatot ki kell tölteni, és el kell fogadni a felhasználási feltételeket.");
                    }

            foreach(var cartItem in cart.Items)
            {
                var stock = await _productService.CheckStockAsync(cartItem.ProductId, cartItem.Quantity);
                if(!stock.IsAvailable)
                    throw new ArgumentOutOfRangeException($"Valamely termékből nincs elég készleten.");
            }

            //Order létrehozása
            var order = new Order
            {
                UserId = userId,
                ShippingMethod = orderRequestDTO.ShippingMethod,
                PaymentMethod = orderRequestDTO.PaymentMethod,
                ShippingFee = orderRequestDTO.ShippingMethod == ShippingMethod.PickupInStore ? 0m : 1500m,
                OrderItems = new List<OrderItem>()
            };
            
            //cartItem-ek átkonvertálása OrderItem-re
            foreach (var cartItem in cart.Items)
            {
                order.OrderItems.Add(new OrderItem
                {
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.Price
                });
            }

            // TotalAmount kiszámítása
            order.TotalAmount = order.OrderItems.Sum(oi => oi.Price * oi.Quantity) + (order.ShippingFee ?? 0);

            try
            {
                await _productService.DecreaseStockAsync(order);
            } 
            catch(Exception ex)
            {
                Console.WriteLine("Stock csökkentés hiba: " + ex.Message);
                throw;
            }

            //rendelés mentés az adatbázisba
            try
            {
                await _orderRepository.AddAsync(order);
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Order mentés DB hiba: " + ex.Message);
                Console.WriteLine("InnerException: " + ex.InnerException?.Message);
                throw;
            }

            //kosár ürítése a rendelés leadása után
            await _cartService.ClearCartAsync(userId);

            //DTO összeállítása visszaadásra
            var orderDto = new OrderDTO
            {
                OrderId = order.Id,
                UserId = order.UserId,
                ShippingMethod = order.ShippingMethod,
                PaymentMethod = order.PaymentMethod,
                ShippingFee = order.ShippingFee,
                TotalAmount = order.TotalAmount,
                Items = order.OrderItems.Select(oi => new OrderItemDTO
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.Product?.ProductName ?? "N/A",
                    Quantity = oi.Quantity,
                    Price = oi.Price
                }).ToList()
            };

            return orderDto;
        }
    }
}
