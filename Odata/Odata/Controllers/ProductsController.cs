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
        ProductsContext _context = new ProductsContext();

        public override IQueryable<Product> Get()
        {
            return _context.Products;
        }

        protected override Product GetEntityByKey(int key)
        {
            return _context.Products.FirstOrDefault(p => p.ID == key);
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }

        protected override Product CreateEntity(Product entity)
        {
            _context.Products.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        protected override int GetKey(Product entity)
        {
            return entity.ID;
        }

        protected override Product UpdateEntity(int key, Product update)
        {
            // Verify that a product with this ID exists.
            if (!_context.Products.Any(p => p.ID == key))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            _context.Products.Attach(update); // Replace the existing entity in the DbSet.
            _context.Entry(update).State = System.Data.EntityState.Modified;
            _context.SaveChanges();
            return update;
        }

        protected override Product PatchEntity(int key, Delta<Product> patch)
        {
            Product product = _context.Products.FirstOrDefault(p => p.ID == key);
            if (product == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            patch.Patch(product);
            _context.SaveChanges();
            return product;
        }

        public override void Delete([FromODataUri] int key)
        {
            Product product = _context.Products.FirstOrDefault(p => p.ID == key);
            if (product == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            _context.Products.Remove(product);
            _context.SaveChanges();
        }
    }
}
