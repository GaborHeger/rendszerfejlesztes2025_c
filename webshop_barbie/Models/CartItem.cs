using System.ComponentModel.DataAnnotations;

namespace webshop_barbie.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        [Required]
        [Range(0.0, double.MaxValue, ErrorMessage = "Price cannot be negative")]
        public decimal Price { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int CartId { get; set; }
        public Cart Cart { get; set; } = null!;
    }
}
