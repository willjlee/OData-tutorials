using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Odata.Models
{
    public class ProductsContext : DbContext
    {
        public ProductsContext()
            : base("name=ProductsContext")
        {
        }

        static ProductsContext()
        {
            Database.SetInitializer(new ProductsContextInitializer());
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
    }
}