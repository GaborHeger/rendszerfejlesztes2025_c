using System.ComponentModel.DataAnnotations;

namespace webshop_barbie.DTOs.Requests
{
    public class UpdateCartItemRequestDTO
    {
        [Required(ErrorMessage = "A productId megadása kötelező!")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "A darabszám megadása kötelező!")]
        public int Quantity { get; set; }
    }
}
