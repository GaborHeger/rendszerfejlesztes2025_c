namespace webshop_barbie.DTOs
{
    public class CartDTO
    {
        public int CartId { get; set; }
        public int UserId { get; set; }

        public List<CartItemDTO> Items { get; set; }
    }

    public class CartItemDTO
    {
        public int ProductId { get; set; }

        public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string? imageUrl { get; set; }
        public int Stock {  get; set; }
    }
}
