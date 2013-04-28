using Odata.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.OData;

namespace Odata.Controllers
{
    public class ProductsController : EntitySetController<Product, int>
    {
        static List<Product> products = new List<Product>() //normally this would be a database
        {
            new Product() { ID = 1, Name = "Hat", Price = 15, Category = "Apparel" },
            new Product() { ID = 2, Name = "Socks", Price = 5, Category = "Apparel" },
            new Product() { ID = 3, Name = "Scarf", Price = 12, Category = "Apparel" },
            new Product() { ID = 4, Name = "Yo-yo", Price = 4.95M, Category = "Toys" },
            new Product() { ID = 5, Name = "Puzzle", Price = 8, Category = "Toys" },
        };

        public override IQueryable<Product> Get()
        {
            return products.AsQueryable();
        }

        protected override Product GetEntityByKey(int key)
        {
            return products.FirstOrDefault(p => p.ID == key);
        }
    }
}
