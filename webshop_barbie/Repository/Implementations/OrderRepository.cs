using Microsoft.EntityFrameworkCore;
using webshop_barbie.Data;
using webshop_barbie.DTOs;
using webshop_barbie.Models;
using webshop_barbie.Repository.Interfaces;

namespace webshop_barbie.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly WebshopContext _context;

        public OrderRepository(WebshopContext context)
        {
            _context = context;
        }

        // Az összes rendelés lekérdezése adott felhasználóhoz
        // Tartalmazza a rendelés tételeit és azok termékeit
        public async Task<IEnumerable<Order>> GetByUserIdAsync(int userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .ToListAsync();
        }

        // Egy rendelés összes tételének lekérdezése rendelés ID alapján
        // Betölti a termék adatait is
        public async Task<IEnumerable<OrderItem>> GetByOrderIdAsync(int orderId)
        {
            return await _context.OrderItems
                .Include(oi => oi.Product)
                .Where(oi => oi.OrderId == orderId)
                .ToListAsync();
        }

        // Új rendelés hozzáadása
        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        // Rendelés törlése felhasználó és rendelés ID alapján
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