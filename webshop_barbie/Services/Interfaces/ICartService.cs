using webshop_barbie.Models;
using webshop_barbie.DTOs;

namespace webshop_barbie.Service.Interfaces
{
    public interface ICartService
    {
        /// <summary>
        /// Egy adott felhasználó kosarának lekérése.
        /// A frontend hívja sikeres bejelentkezés után, hogy megjelenítse a felhasználó aktuális kosarát.
        /// </summary>
        /// <param name="userId">A felhasználó azonosítója.</param>
        /// <returns>A felhasználó kosara CartDTO formátumban.</returns>
        Task<CartDTO> GetCartByUserIdAsync(int userId);

        /// <summary>
        /// Termék hozzáadása a felhasználó kosarához.
        /// Akkor hívódik, amikor a felhasználó rákattint a "Kosárba" gombra egy terméknél.
        /// </summary>
        /// <param name="userId">A felhasználó azonosítója.</param>
        /// <param name="productId">A hozzáadandó termék azonosítója.</param>
        /// <param name="quantity">A hozzáadandó mennyiség.</param>
        /// <returns>A frissített CartDTO.</returns>
        Task<CartDTO> AddItemToCartAsync(int userId, int productId, int quantity);

        /// <summary>
        /// Termék eltávolítása a felhasználó kosarából.
        /// Akkor hívódik, amikor a felhasználó a törlés/kuka ikonra kattint egy kosár tételnél.
        /// </summary>
        /// <param name="userId">A felhasználó azonosítója.</param>
        /// <param name="productId">A törlendő termék azonosítója.</param>
        /// <returns>A frissített CartDTO.</returns>
        Task<CartDTO> RemoveItemFromCartAsync(int userId, int productId);

        /// <summary>
        /// Egy termék mennyiségének frissítése a felhasználó kosarában.
        /// Akkor hívódik, amikor a felhasználó a +/- gombokkal módosítja a mennyiséget.
        /// </summary>
        /// <param name="userId">A felhasználó azonosítója.</param>
        /// <param name="productId">A frissítendő termék azonosítója.</param>
        /// <param name="quantity">Az új mennyiség.</param>
        /// <returns>A frissített CartDTO.</returns>
        Task<CartDTO> UpdateCartItemQuantityAsync(int userId, int productId, int quantity);

        /// <summary>
        /// Az összes tétel törlése a felhasználó kosarából.
        /// Akkor hívódik, amikor egy rendelés sikeresen leadásra kerül.
        /// </summary>
        /// <param name="userId">A felhasználó azonosítója.</param>
        /// <returns>Sikeres művelet esetén egy üzenet string formátumban.</returns>
        Task<string> ClearCartAsync(int userId);
    }
}

