using webshop_barbie.Models;
using webshop_barbie.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using webshop_barbie.Data;

namespace webshop_barbie.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly WebshopContext _context;

        public CartRepository(WebshopContext context)
        {
            _context = context;
        }

        public async Task<Cart?> GetByUserIdAsync(int userId)
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task AddAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Cart cart)
        {
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Cart cart)
        {
            var existingCart = await _context.Carts
                .FirstOrDefaultAsync(c => c.UserId == cart.UserId && c.Id == cart.Id);

            if(existingCart != null)
            {
                _context.Carts.Remove(existingCart);
                await _context.SaveChangesAsync();
            }
        }
    }
}
