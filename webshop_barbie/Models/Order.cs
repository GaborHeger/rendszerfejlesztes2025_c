using System.ComponentModel.DataAnnotations;

namespace webshop_barbie.Models
{
    public class Order
    {
        public int Id { get; set; }

        public ShippingMethod ShippingMethod { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public decimal? ShippingFee { get; set; }

        public decimal TotalAmount { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
