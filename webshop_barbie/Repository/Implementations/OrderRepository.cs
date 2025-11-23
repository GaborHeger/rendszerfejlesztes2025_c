using webshop_barbie.Models;
using webshop_barbie.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using webshop_barbie.Data;

namespace webshop_barbie.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly WebshopContext _context;

        public OrderRepository(WebshopContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetByUserIdAsync(int userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Order order)
        {
            var existingOrder = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == order.Id && o.UserId == order.UserId);

            if (existingOrder != null)
            {
                _context.Orders.Remove(existingOrder);
                await _context.SaveChangesAsync();
            }
        }
    }
}