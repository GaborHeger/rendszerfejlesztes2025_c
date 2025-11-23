using webshop_barbie.Service.Interfaces;
using webshop_barbie.DTOs;
using webshop_barbie.Models;
using webshop_barbie.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace webshop_barbie.Service
{
    public class FavoriteService : IFavoriteService
    {
        private readonly WebshopContext _context;

        public FavoriteService(WebshopContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FavoriteDTO>> GetFavoritesByUserIdAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<string> AddFavoriteAsync(int userId, int productId)
        {
            throw new NotImplementedException();
        }

        public async Task<string> RemoveFavoriteAsync(int userId, int productId)
        {
            throw new NotImplementedException();
        }

        public async Task<CartDTO> AddFavoriteToCartAsync(int userId, int productId)
        {
            throw new NotImplementedException();
        }
    }
}
