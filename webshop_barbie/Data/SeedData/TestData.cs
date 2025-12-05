using Microsoft.AspNetCore.Identity;
using webshop_barbie.Models;

namespace webshop_barbie.Data.SeedData
{
    public class TestData
    {
        public static void SeedUsers(WebshopContext context)
        {
            var passwordHasher = new PasswordHasher<User>();

            if (!context.Users.Any())
            {
                var user1 = new User
                {
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
        }

        public static void SeedCarts(WebshopContext context)
        {
            if (!context.Carts.Any())
            {
                var cart1 = new Cart
                {
                    UserId = 1,
                    CartItems = new List<CartItem>
                    {
                        new CartItem { ProductId = 1, Quantity = 2, Price = context.Products.First(p => p.Id == 1).Price },
                        new CartItem { ProductId = 2, Quantity = 1, Price = context.Products.First(p => p.Id == 2).Price }
                    }
                };

                var cart2 = new Cart
                {
                    UserId = 2,
                    CartItems = new List<CartItem>
                    {
                        new CartItem { ProductId = 2, Quantity = 3, Price = context.Products.First(p => p.Id == 2).Price }
                    }
                };

                context.Carts.AddRange(cart1, cart2);
                context.SaveChanges();
            }
        }

        public static void SeedOrders(WebshopContext context)
        {
            if (!context.Orders.Any())
            {
                var order1Items = new List<OrderItem>
                {
                    new OrderItem { ProductId = 1, Quantity = 2, Price = context.Products.First(p => p.Id == 1).Price },
                    new OrderItem { ProductId = 2, Quantity = 1, Price = context.Products.First(p => p.Id == 2).Price }
                };

                var order1 = new Order
                {
                    UserId = 1,
                    ShippingMethod = ShippingMethod.PickupInStore,
                    PaymentMethod = PaymentMethod.CashOnDelivery,
                    ShippingFee = 500m,
                    TotalAmount = order1Items.Sum(i => i.Price * i.Quantity) + 500m,
                    OrderItems = order1Items
                };
                order1Items.ForEach(i => i.OrderId = order1.Id);

                var order2Items = new List<OrderItem>
                {
                    new OrderItem { ProductId = 2, Quantity = 3, Price = context.Products.First(p => p.Id == 2).Price }
                };

                var order2 = new Order
                {
                    UserId = 2,
                    ShippingMethod = ShippingMethod.HomeDelivery,
                    PaymentMethod = PaymentMethod.OnlineCardPayment,
                    ShippingFee = 800m,
                    TotalAmount = order2Items.Sum(i => i.Price * i.Quantity) + 800m,
                    OrderItems = order2Items
                };
                order2Items.ForEach(i => i.OrderId = order2.Id);

                context.Orders.AddRange(order1, order2);
                context.OrderItems.AddRange(order1Items);
                context.OrderItems.AddRange(order2Items);
                context.SaveChanges();
            }
        }
    }
}
