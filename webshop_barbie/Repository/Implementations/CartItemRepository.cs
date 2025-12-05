using webshop_barbie.Models;
using webshop_barbie.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using webshop_barbie.Data;

namespace webshop_barbie.Repository
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly WebshopContext _context;

        public CartItemRepository (WebshopContext context)
        {
            _context = context;
        }

        // Kosárhoz tétel hozzáadása
        public async Task AddAsync(CartItem item)
        {
            await _context.CartItems.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        // Kosár tétel frissítése
        public async Task UpdateAsync(CartItem cartItem)
        {
            _context.CartItems.Update(cartItem);
            await _context.SaveChangesAsync();
        }

        // Kosár tétel törlése ID alapján
        public async Task DeleteAsync(int cartItemId)
        {
            // Megkeressük a törlendő tételt
            var cartItem = await _context.CartItems.FindAsync(cartItemId);

            if(cartItem != null )
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
        }
    }
}