using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace webshop_barbie.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string ProductName { get; set; } = "";

        [Required]
        [Range(0.0, double.MaxValue, ErrorMessage = "Price cannot be negative")]
        public decimal Price { get; set; }

        [Required]
        public string ImageUrl { get; set; } = "";

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative")]
        public int Stock {  get; set; }

        public Category Category { get; set; }


        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
