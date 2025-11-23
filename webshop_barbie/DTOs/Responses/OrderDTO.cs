using webshop_barbie.Models;

namespace webshop_barbie.DTOs
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }

        public ShippingMethod ShippingMethod { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public decimal? ShippingFee { get; set; }
        public decimal TotalAmount { get; set; }

        public List<OrderItemDTO> Items { get; set; }
    }

    public class OrderItemDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}