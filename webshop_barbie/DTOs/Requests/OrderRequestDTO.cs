using System.ComponentModel.DataAnnotations;
using webshop_barbie.Models;

namespace webshop_barbie.DTOs
{
    public class OrderRequestDTO
    {
        [Required(ErrorMessage = "Az átvételi mód megadása kötelező!")]
        public ShippingMethod ShippingMethod { get; set; }

        [Required(ErrorMessage = "A fizetési mód megadása kötelező!")]
        public PaymentMethod PaymentMethod { get; set; }
    }
}

