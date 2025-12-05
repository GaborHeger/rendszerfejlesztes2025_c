using Microsoft.AspNetCore.Identity;
using webshop_barbie.Models;

namespace webshop_barbie.Data.SeedData.ProductData
{
    public class AccessoryData
    {
        public static List<Product> GetAccessories()
        {
            return new List<Product>
            {
                new Product { ProductName = "Dupla Rózsaszín Divatszett Kiegészítőkkel", Price = 2890m, Stock = 15, ImageUrl = "Images/Products/accessory_1.jpg", Category = Category.Accessory },
                new Product { ProductName = "Pegasus kiegészítőkkel", Price = 9990m, Stock = 20, ImageUrl = "Images/Products/accessory_2.jpg", Category = Category.Accessory },
                new Product { ProductName = "Rózsaszín Barbie cabrio autó", Price = 11500m, Stock = 18, ImageUrl = "Images/Products/accessory_3.png", Category = Category.Accessory },
                new Product { ProductName = "Barbie Dreamhouse", Price = 29900m, Stock = 10, ImageUrl = "Images/Products/accessory_4.jpg", Category = Category.Accessory },
                new Product { ProductName = "Barbie Kiegészítő Szett – Reggeli", Price = 4550m, Stock = 12, ImageUrl = "Images/Products/accessory_5.jpg", Category = Category.Accessory },
                new Product { ProductName = "Kiegészítő Szett – Cipők és Ékszerek", Price = 2990m, Stock = 8, ImageUrl = "Images/Products/accessory_6.jpg", Category = Category.Accessory },
                new Product { ProductName = "Chelsea baba Utazó szett – Kiskutyával", Price = 5490m, Stock = 14, ImageUrl = "Images/Products/accessory_7.jpg", Category = Category.Accessory },
                new Product { ProductName = "Kiegészítő Szett – Táska és Elektronikai Eszközök", Price = 2990m, Stock = 16, ImageUrl = "Images/Products/accessory_8.jpg", Category = Category.Accessory },
                new Product { ProductName = "Barbie Babaház – Lila és Pink Ház", Price = 35000m, Stock = 10, ImageUrl = "Images/Products/accessory_9.jpg", Category = Category.Accessory },
                new Product { ProductName = "Baba Ápolási Kiegészítő Szett", Price = 2990m, Stock = 20, ImageUrl = "Images/Products/accessory_10.jpg", Category = Category.Accessory },
            };
        }
    }
}
