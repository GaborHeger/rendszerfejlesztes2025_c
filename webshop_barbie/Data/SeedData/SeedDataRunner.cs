using webshop_barbie.Data.SeedData;
using webshop_barbie.Data.SeedData.ProductData;

namespace webshop_barbie.Data.SeedData
{
    public class SeedDataRunner
    {
        public static void SeedAll(WebshopContext context)
        {
            TestData.SeedUsers(context);

            if (!context.Products.Any())
            {
                context.Products.AddRange(BarbieData.GetBarbieProducts());
                context.Products.AddRange(AccessoryData.GetAccessories());
                context.SaveChanges();
            }

            TestData.SeedCarts(context);

            TestData.SeedOrders(context);
        }
    }
}
