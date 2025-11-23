using System.ComponentModel.DataAnnotations;

namespace webshop_barbie.Models
{
    public class User
    {
        public int Id { get; set; }

        [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = "";

        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        public string PasswordHash { get; set; } = "";

        [MaxLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
        public string FirstName { get; set; } = "";

        [MaxLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
        public string LastName { get; set; } = "";

        [Range(typeof(bool), "true", "true", ErrorMessage = "You must accept the terms")]
        public bool AcceptedTerms { get; set; }

        public bool SubscribedToNewsletter { get; set; }

        [Phone(ErrorMessage = "Invalid phone number")]
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
