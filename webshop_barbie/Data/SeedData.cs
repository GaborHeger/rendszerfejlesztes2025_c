using Microsoft.AspNetCore.Identity;
using webshop_barbie.Models;

namespace webshop_barbie.Data
{
    public static class SeedData
    {
        public static void Initialize(WebshopContext context)
        {
            var passwordHasher = new PasswordHasher<User>();

            // Teszt felhasználók
            if (!context.Users.Any())
            {
                var user1 = new User
                {
                    Id = 1,
                    Email = "teszt1@example.com",
                    FirstName = "Anna",
                    LastName = "Kovács",
                    PhoneNumber = "0612345678",
                    PostalCode = "1111",
                    City = "Budapest",
                    AddressDetails = "Teszt utca 1",
                    SubscribedToNewsletter = true,
                    AcceptedTerms = true
                };
                user1.PasswordHash = passwordHasher.HashPassword(user1, "Teszt123!");

                var user2 = new User
                {
                    Id = 2,
                    Email = "teszt2@example.com",
                    FirstName = "Béla",
                    LastName = "Nagy",
                    PhoneNumber = "0698765432",
                    PostalCode = "2222",
                    City = "Debrecen",
                    AddressDetails = "Minta utca 2",
                    SubscribedToNewsletter = false,
                    AcceptedTerms = true
                };
                user2.PasswordHash = passwordHasher.HashPassword(user2, "Jelszo456!");

                context.Users.AddRange(user1, user2);
                context.SaveChanges();
            }

            // Teszt termékek
            // Teszt kosarak és CartItem-ek
            if (!context.Carts.Any())
            {
                var cart1 = new Cart
                {
                    Id = 1, //cartId
                    UserId = 1,
                    CartItems = new List<CartItem>
                    {
                        new CartItem
                        {
                            Id = 1, //cartItemId
                            CartId = 1,
                            ProductId = 1,
                            Quantity = 2
                        },
                        new CartItem
                        {
                            Id = 2,
                            CartId = 1,
                            ProductId = 2,
                            Quantity = 1
                        }
                    }
                };

                var cart2 = new Cart
                {
                    Id = 2, //cartId
                    UserId = 2,
                    CartItems = new List<CartItem>
                    {
                        new CartItem
                        {
                            Id = 3,
                            CartId = 2,
                            ProductId = 2,
                            Quantity = 3
                        }
                    }
                };

                context.Carts.AddRange(cart1, cart2);
                context.SaveChanges();
            }
        }
    }
}

