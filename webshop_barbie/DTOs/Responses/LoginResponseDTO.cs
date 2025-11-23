namespace webshop_barbie.DTOs
{
    public class LoginResponseDTO
    {
        public string Token { get; set; }

        public int UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string AddressDetails { get; set; }
        public bool SubscribedToNewsletter { get; set; }
        public bool AcceptedTerms { get; set; }
    }
}