using Microsoft.AspNetCore.Identity;
using webshop_barbie.Models;

namespace webshop_barbie.Data.SeedData.ProductData
{
    public class BarbieData
    {
        public static List<Product> GetBarbieProducts()
        {
            return new List<Product>
            {
                new Product { ProductName = "Csillogó Parti Barbie", Price = 19990m, Stock = 5, ImageUrl = "Images/Products/barbie_1.jpg", Category = Category.Barbie },
                new Product { ProductName = "TOMI Ken", Price = 100m, Stock = 15, ImageUrl = "Images/Products/barbie_2.jpg", Category = Category.Barbie },
                new Product { ProductName = "Pink Kockás Ruhás Barbie", Price = 18990m, Stock = 20, ImageUrl = "Images/Products/barbie_3.jpg", Category = Category.Barbie },
                new Product { ProductName = "Pink Pántos Ruha és Szőrme Sálas Barbie", Price = 12990m, Stock = 7, ImageUrl = "Images/Products/barbie_4.jpg", Category = Category.Barbie },
                new Product { ProductName = "Pink Ruhás és Lábprotézises Barbie", Price = 11900m, Stock = 10, ImageUrl = "Images/Products/barbie_5.jpg", Category = Category.Barbie },
                new Product { ProductName = "Cowboy Ruhás Barbie", Price = 20990m, Stock = 25, ImageUrl = "Images/Products/barbie_6.jpg", Category = Category.Barbie },
                new Product { ProductName = "Tréner Barbie", Price = 19900m, Stock = 18, ImageUrl = "Images/Products/barbie_7.jpg", Category = Category.Barbie },
                new Product { ProductName = "Pink Melegítő Overálos Barbie", Price = 18900m, Stock = 4, ImageUrl = "Images/Products/barbie_8.jpg", Category = Category.Barbie },
                new Product { ProductName = "Partiruhás Barbie", Price = 8990m, Stock = 6, ImageUrl = "Images/Products/barbie_9.jpg", Category = Category.Barbie },
                new Product { ProductName = "Tréner Ken", Price = 22000m, Stock = 3, ImageUrl = "Images/Products/barbie_10.jpg", Category = Category.Barbie }
            };
        }
    }
}
