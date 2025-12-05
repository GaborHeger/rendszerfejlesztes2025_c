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

        public OrderService(IOrderRepository orderRepository, IUserService userService, 
            ICartService cartService, IProductService productService)
        {
            _orderRepository = orderRepository;
            _userService = userService;
            _cartService = cartService;
            _productService = productService;
        }

        public async Task<IEnumerable<OrderDTO>> GetOrdersByUserIdAsync(int userId)
        {
            // Felhasználó ellenőrzése
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"A felhasználó ({userId}) nem található.");

            var orders = await _orderRepository.GetByUserIdAsync(userId);

            var orderDtos = new List<OrderDTO>();

            foreach (var order in orders)
            {
                var orderItems = await _orderRepository.GetByOrderIdAsync(order.Id);

                var orderItemDtos = orderItems.Select(item => new OrderItemDTO
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product?.ProductName ?? "N/A",
                    Quantity = item.Quantity,
                    Price = item.Price
                }).ToList();

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
            }

            return orderDtos;
        }

        public async Task ValidateOrderAsync(int userId, OrderRequestDTO orderRequestDTO)
        {
            // Ellenőrizzük, hogy a felhasználó létezik
            var userInput = await _userService.GetUserByIdAsync(userId);
            if (userInput == null)
                throw new KeyNotFoundException($"A felhasználó nem található.");

            // A felhasználó személyes adatai kötelezőek
            if (userInput == null)
                throw new KeyNotFoundException("A felhasználó nem található.");

            if (string.IsNullOrWhiteSpace(userInput.FirstName))
                throw new ArgumentException("A keresztnév hiányzik.");

            if (string.IsNullOrWhiteSpace(userInput.LastName))
                throw new ArgumentException("A vezetéknév hiányzik.");

            if (string.IsNullOrWhiteSpace(userInput.PhoneNumber))
                throw new ArgumentException("A telefonszám hiányzik.");

            if (string.IsNullOrWhiteSpace(userInput.PostalCode))
                throw new ArgumentException("A irányítószám hiányzik.");

            if (string.IsNullOrWhiteSpace(userInput.City))
                throw new ArgumentException("A város hiányzik.");

            if (string.IsNullOrWhiteSpace(userInput.AddressDetails))
                throw new ArgumentException("A cím részletei hiányzik.");

            // A felhasználónak el kell fogadnia a felhasználási feltételeket
            if (!userInput.AcceptedTerms)
                throw new ArgumentException("A felhasználási feltételek elfogadása kötelező.");

            // Ellenőrizzük a szállítási és fizetési módok érvényességét
            if (!Enum.IsDefined(typeof(ShippingMethod), orderRequestDTO.ShippingMethod))
                throw new ArgumentException("Érvénytelen szállítási mód.");

            if (!Enum.IsDefined(typeof(PaymentMethod), orderRequestDTO.PaymentMethod))
                throw new ArgumentException("Érvénytelen fizetési mód.");

            // Ellenőrizzük, hogy a kosár ne legyen üres
            var cart = await _cartService.GetCartByUserIdAsync(userId);
            if (cart == null || !cart.Items.Any())
                throw new InvalidOperationException("A kosár üres, nem lehet rendelést létrehozni.");

            foreach (var cartItem in cart.Items)
            {
                // Mennyiség nem lehet nulla vagy negatív
                if (cartItem.Quantity <= 0)
                    throw new InvalidOperationException($"A {cartItem.ProductName ?? cartItem.ProductId.ToString()} mennyisége nem lehet nulla vagy negatív.");

                // Ár nem lehet negatív
                if (cartItem.Price < 0)
                    throw new InvalidOperationException($"A {cartItem.ProductName ?? cartItem.ProductId.ToString()} ára nem lehet negatív.");

                // Ellenőrizzük a készletet a rendelés leadása előtt
                var stock = await _productService.CheckStockAsync(cartItem.ProductId, cartItem.Quantity);
                if (!stock.IsAvailable)
                    throw new ArgumentOutOfRangeException($"Valamely termékből nincs elég készleten.");
            }
        }

        public async Task<OrderDTO> CreateOrderAsync(int userId, OrderRequestDTO orderRequestDTO)
        {
            // Ellenőrizzük, hogy a felhasználó azonosító érvényes
            if (userId <= 0)
                throw new ArgumentException("A rendeléshez nem tartozik érvényes felhasználó.");

            await ValidateOrderAsync(userId, orderRequestDTO);

            var order = new Order
            {
                UserId = userId,
                ShippingMethod = orderRequestDTO.ShippingMethod,
                PaymentMethod = orderRequestDTO.PaymentMethod,
                ShippingFee = orderRequestDTO.ShippingMethod == ShippingMethod.PickupInStore ? 0m : 1500m,
                OrderItems = new List<OrderItem>()
            };

            var cart = await _cartService.GetCartByUserIdAsync(userId);

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

            await _productService.DecreaseStockAsync(order);

            await _orderRepository.AddAsync(order);

            await _cartService.ClearCartAsync(userId);

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
