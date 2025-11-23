using webshop_barbie.Service.Interfaces;
using webshop_barbie.DTOs;
using webshop_barbie.Models;
using webshop_barbie.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace webshop_barbie.Service
{
    public class OrderService : IOrderService
    {
        private readonly WebshopContext _context;

        public OrderService(WebshopContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderDTO>> GetOrdersByUserIdAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<OrderDTO> CreateOrderAsync(int userId, OrderRequestDTO orderRequestDTO)
        {
            throw new NotImplementedException();
        }
    }
}
