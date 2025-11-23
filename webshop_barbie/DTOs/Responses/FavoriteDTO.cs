using webshop_barbie.Models;

namespace webshop_barbie.DTOs
{
    public class FavoriteDTO
    {
        public int FavoriteId { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }

        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public Category Category { get; set; }
    }
}