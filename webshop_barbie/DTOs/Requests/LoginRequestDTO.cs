using System.ComponentModel.DataAnnotations;

namespace webshop_barbie.DTOs
{
    public class LoginRequestDTO
    {
        [Required(ErrorMessage = "Az email megadása kötelező!")]
        [EmailAddress(ErrorMessage = "Érvénytelen email formátum!")]
        [MaxLength(100, ErrorMessage = "Az email nem haladhatja meg a 100 karaktert!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A jelszó megadása kötelező!")]
        [MinLength(8, ErrorMessage = "A jelszónak legalább 8 karakter hosszúnak kell lennie!")]
        public string Password { get; set; }
    }
}
