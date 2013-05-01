using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Odata.Models
{
    public class ProductsContextInitializer :
        DropCreateDatabaseIfModelChanges<ProductsContext>
    {
        protected override void Seed(ProductsContext context)
        {
            var products = new List<Product>()
            {
                new Product() { Name = "Hat", Price = 15, Category = "Apparel" },
                new Product() { Name = "Socks", Price = 5, Category = "Apparel" },
                new Product() { Name = "Scarf", Price = 12, Category = "Apparel" },
                new Product() { Name = "Yo-yo", Price = 4.95M, Category = "Toys" },
                new Product() { Name = "Puzzle", Price = 8, Category = "Toys" },
            };

            products.ForEach(p => context.Products.Add(p));
        }
    }
}