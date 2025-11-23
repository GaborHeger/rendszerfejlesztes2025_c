using System.ComponentModel.DataAnnotations;

namespace webshop_barbie.DTOs
{
    public class RegisterRequestDTO
    {
        [Required(ErrorMessage = "Az email megadása kötelező!")]
        [EmailAddress(ErrorMessage = "Érvénytelen email formátum!")]
        [MaxLength(100, ErrorMessage = "Az email nem haladhatja meg a 100 karaktert!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A jelszó megadása kötelező!")]
        [MinLength(8, ErrorMessage = "A jelszónak legalább 8 karakter hosszúnak kell lennie!")]
        public string Password { get; set; }

        [Required(ErrorMessage = "A keresztnév megadása kötelező!")]
        [MaxLength(50, ErrorMessage = "A keresztnév nem lehet hosszabb 50 karakternél!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "A vezetéknév megadása kötelező!")]
        [MaxLength(50, ErrorMessage = "A vezetéknévnév nem lehet hosszabb 50 karakternél!")]
        public string LastName { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessage = "A feltételek elfogadása kötelező!")]
        public bool AcceptedTerms { get; set; }

        public bool SubscribedToNewsletter { get; set; }
    }
}

