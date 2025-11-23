using System.ComponentModel.DataAnnotations;

namespace webshop_barbie.DTOs
{
    public class UpdateUserRequestDTO
    {
        [Required(ErrorMessage = "Az id megadása kötelező!")]
        public int Id { get; set; }

        [MaxLength(50, ErrorMessage = "A keresztnév nem lehet hosszabb 50 karakternél!")]
        public string FirstName { get; set; }

        [MaxLength(50, ErrorMessage = "A vezetéknévnév nem lehet hosszabb 50 karakternél!")]
        public string LastName { get; set; }

        [Phone(ErrorMessage = "Érvénytelen telefonszám!")]
        public string PhoneNumber { get; set; }

        [MaxLength(20)]
        public string PostalCode { get; set; }

        [MaxLength(50)]
        public string City { get; set; }

        [MaxLength(200)]
        public string AddressDetails { get; set; }

        public bool SubscribedToNewsletter { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessage = "A feltételek elfogadása kötelező!")]
        public bool AcceptedTerms { get; set; }
    }
}