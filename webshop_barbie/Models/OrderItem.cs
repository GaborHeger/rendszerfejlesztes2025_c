using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;

namespace webshop_barbie.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

    }
}
