using Microsoft.AspNetCore.SignalR;

namespace webshop_barbie.Models
{
    public class Favorite
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
    }
}
