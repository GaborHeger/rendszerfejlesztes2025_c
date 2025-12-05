using System.ComponentModel.DataAnnotations;

namespace webshop_barbie.Models
{
    public class User
    {
        public int Id { get; set; }

        [MaxLength(100, ErrorMessage = "Az e-mail cím nem haladhatja meg a 100 karaktert!")]
        [EmailAddress(ErrorMessage = "Érvénytelen e-mail cím!")]
        public string Email { get; set; } = "";

        [MinLength(8, ErrorMessage = "A jelszónak legalább 8 karakter hosszúnak kell lennie!")]
        public string PasswordHash { get; set; } = "";

        [MaxLength(50, ErrorMessage = "A keresztnév nem lehet hosszabb 50 karakternél!")]
        public string FirstName { get; set; } = "";

        [MaxLength(50, ErrorMessage = "A vezetéknév nem haladhatja meg az 50 karaktert!")]
        public string LastName { get; set; } = "";

        [Range(typeof(bool), "true", "true", ErrorMessage = "El kell fogadnia a feltételeket!")]
        public bool AcceptedTerms { get; set; }

        public bool SubscribedToNewsletter { get; set; }

        [Phone(ErrorMessage = "Érvénytelen telefonszám!")]
        public string? PhoneNumber { get; set; }

        [MaxLength(20)]
        public string? PostalCode { get; set; }

        [MaxLength(50)]
        public string? City { get; set; }

        [MaxLength(200)]
        public string? AddressDetails { get; set; }

        public Cart? Cart { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
    }
}
