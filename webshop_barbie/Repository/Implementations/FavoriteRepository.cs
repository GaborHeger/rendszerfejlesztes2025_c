using webshop_barbie.Models;
using webshop_barbie.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using webshop_barbie.Data;

namespace webshop_barbie.Repository
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly WebshopContext _context;

        public FavoriteRepository(WebshopContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Favorite>> GetAllAsync(int userId)
        {
            return await _context.Favorites
                .Where(f  => f.UserId == userId)
                .ToListAsync();
        }

        public async Task<Favorite?> GetByIdAsync(int userId, int productId)
        {
            return await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId);
        }

        public async Task<Favorite> AddAsync(Favorite favorite)
        {
            await _context.Favorites.AddAsync(favorite);
            await _context.SaveChangesAsync();

            return favorite;
        }

        public async Task<Favorite?> DeleteAsync(int userId, int productId)
        {
            var favorite = await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId);

            if (favorite == null) return null;
            
            _context.Favorites.Remove(favorite);
            await _context.SaveChangesAsync();

            return favorite;
        }
    }
}