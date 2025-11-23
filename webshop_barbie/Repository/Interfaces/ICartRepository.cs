using webshop_barbie.Models;

namespace webshop_barbie.Repository.Interfaces
{
    public interface ICartRepository
    {
        /// <summary>
        /// Egy adott felhasználó kosarának lekérése az azonosítója alapján.
        /// </summary>
        /// <param name="userId">A felhasználó azonosítója.</param>
        /// <returns>A kosár, ha létezik; különben null.</returns>
        Task<Cart?> GetByUserIdAsync(int userId);

        /// <summary>
        /// Új kosár hozzáadása.
        /// </summary>
        /// <param name="cart">A hozzáadandó kosár.</param>
        Task AddAsync(Cart cart);

        /// <summary>
        /// Meglévő kosár frissítése.
        /// </summary>
        /// <param name="cart">A frissített kosár.</param>
        Task UpdateAsync(Cart cart);

        /// <summary>
        /// Kosár törlése.
        /// </summary>
        /// <param name="cart">A törlendő kosár.</param>
        Task DeleteAsync(Cart cart);
    }
}


