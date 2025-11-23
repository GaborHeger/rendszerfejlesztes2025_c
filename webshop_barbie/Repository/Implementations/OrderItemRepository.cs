using webshop_barbie.Models;
using webshop_barbie.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using webshop_barbie.Data;

namespace webshop_barbie.Repository
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly WebshopContext _context;

        public OrderItemRepository(WebshopContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderItem>> GetByOrderIdAsync(int orderId)
        {
            return await _context.OrderItems
                .Where(oi => oi.OrderId == orderId)
                .ToListAsync();
        }

        public async Task AddAsync(OrderItem orderItem)
        {
            await _context.OrderItems.AddAsync(orderItem);
            await _context.SaveChangesAsync();
        }
    }
}
