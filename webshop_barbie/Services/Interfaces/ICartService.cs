using webshop_barbie.Models;
using webshop_barbie.DTOs;

namespace webshop_barbie.Service.Interfaces
{
    public interface ICartService
    {
        // Lekéri egy felhasználó kosarát
        Task<CartDTO> GetCartByUserIdAsync(int userId);

        // Hozzáad egy terméket a kosárhoz
        Task<CartDTO> AddItemToCartAsync(int userId, int productId, int quantity);

        // Eltávolít egy terméket a kosárból
        Task<CartDTO> RemoveItemFromCartAsync(int userId, int productId);

        // Frissíti egy termék mennyiségét a kosárban
        Task<CartDTO> UpdateCartItemQuantityAsync(int userId, int productId, int quantity);

        // Üríti a felhasználó kosarát
        Task<string> ClearCartAsync(int userId);
    }
}

