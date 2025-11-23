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
            if (!context.Products.Any())
            {
                context.Products.AddRange(
                    new Product
                    {
                        Id = 1,
                        ProductName = "Barbie1",
                        Price = 5000m,
                        ImageUrl = "Images/Products/barbie1.png",
                        Stock = 10,
                        Category = Category.Barbie
                    },
                    new Product
                    {
                        Id = 2,
                        ProductName = "Accessory1",
                        Price = 1500m,
                        ImageUrl = "Images/Products/accessory1.png",
                        Stock = 5,
                        Category = Category.Accessory
                    }
                );
                context.SaveChanges();
            }

            // Teszt kosarak
            if (!context.Carts.Any())
            {
                var cart1 = new Cart
                {
                    Id = 1,
                    UserId = 1,
                    CartItems = new List<CartItem>
                    {
                        new CartItem
                        {
                            Id = 1,
                            CartId = 1,
                            ProductId = 1,
                            Quantity = 2,
                            Price = context.Products.First(p => p.Id == 1).Price
                        },
                        new CartItem
                        {
                            Id = 2,
                            CartId = 1,
                            ProductId = 2,
                            Quantity = 1,
                            Price = context.Products.First(p => p.Id == 2).Price
                        }
                    }
                };

                var cart2 = new Cart
                {
                    Id = 2,
                    UserId = 2,
                    CartItems = new List<CartItem>
                    {
                        new CartItem
                        {
                            Id = 3,
                            CartId = 2,
                            ProductId = 2,
                            Quantity = 3,
                            Price = context.Products.First(p => p.Id == 2).Price
                        }
                    }
                };

                context.Carts.AddRange(cart1, cart2);
                context.SaveChanges();
            }

            // Teszt rendelések
            if (!context.Orders.Any())
            {
                var order1Items = new List<OrderItem>
                {
                    new OrderItem
                    {
                        Id = 1,
                        ProductId = 1,
                        Quantity = 2,
                        Price = context.Products.First(p => p.Id == 1).Price
                    },
                    new OrderItem
                    {
                        Id = 2,
                        ProductId = 2,
                        Quantity = 1,
                        Price = context.Products.First(p => p.Id == 2).Price
                    }
                };

                var order1 = new Order
                {
                    Id = 1,
                    UserId = 1,
                    ShippingMethod = ShippingMethod.PickupInStore,
                    PaymentMethod = PaymentMethod.CashOnDelivery,
                    ShippingFee = 500m,
                    TotalAmount = order1Items.Sum(i => i.Price * i.Quantity) + 500m,
                    OrderItems = order1Items
                };

                // Kapcsolatok beállítása
                order1Items.ForEach(i => i.OrderId = order1.Id);

                var order2Items = new List<OrderItem>
                {
                    new OrderItem
                    {
                        Id = 3,
                        ProductId = 2,
                        Quantity = 3,
                        Price = context.Products.First(p => p.Id == 2).Price
                    }
                };

                var order2 = new Order
                {
                    Id = 2,
                    UserId = 2,
                    ShippingMethod = ShippingMethod.HomeDelivery,
                    PaymentMethod = PaymentMethod.OnlineCardPayment,
                    ShippingFee = 800m,
                    TotalAmount = order2Items.Sum(i => i.Price * i.Quantity) + 800m,
                    OrderItems = order2Items
                };

                // Kapcsolatok beállítása
                order2Items.ForEach(i => i.OrderId = order2.Id);


                // DB-be mentés
                context.Orders.AddRange(order1, order2);
                context.OrderItems.AddRange(order1Items);
                context.OrderItems.AddRange(order2Items);
                context.SaveChanges();
            }
        }
    }
}




