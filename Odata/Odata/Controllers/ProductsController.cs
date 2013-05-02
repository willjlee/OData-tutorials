
using Microsoft.Data.OData;
using Microsoft.Data.OData.Query;
using Odata.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using System.Web.Http.Routing;

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

        // GET /Products(1)/Supplier
        public Supplier GetSupplier([FromODataUri] int key)
        {
            Product product = _context.Products.FirstOrDefault(p => p.ID == key);
            if (product == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return product.Supplier;
        }

        //create a link
        public override void CreateLink([FromODataUri] int key, string navigationProperty,
    [FromBody] Uri link)
        {
            Product product = _context.Products.FirstOrDefault(p => p.ID == key);
            if (product == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            switch (navigationProperty)
            {
                case "Supplier":
                    string supplierKey = GetKeyFromLinkUri<string>(link);
                    Supplier supplier = _context.Suppliers.
                        SingleOrDefault(s => s.Key == supplierKey);
                    if (supplier == null)
                    {
                        throw new HttpResponseException(HttpStatusCode.NotFound);
                    }
                    product.Supplier = supplier;
                    _context.SaveChanges();
                    break;

                default:
                    base.CreateLink(key, navigationProperty, link);
                    break;
            }
        }

        // Helper method to extract the key from an OData link URI.
        private TKey GetKeyFromLinkUri<TKey>(Uri link)
        {
            TKey key = default(TKey);

            // Get the route that was used for this request.
            IHttpRoute route = Request.GetRouteData().Route;

            // Create an equivalent self-hosted route. 
            IHttpRoute newRoute = new HttpRoute(route.RouteTemplate,
                new HttpRouteValueDictionary(route.Defaults),
                new HttpRouteValueDictionary(route.Constraints),
                new HttpRouteValueDictionary(route.DataTokens), route.Handler);

            // Create a fake GET request for the link URI.
            var tmpRequest = new HttpRequestMessage(HttpMethod.Get, link);

            // Send this request through the routing process.
            var routeData = newRoute.GetRouteData(
                Request.GetConfiguration().VirtualPathRoot, tmpRequest);

            // If the GET request matches the route, use the path segments to find the key.
            if (routeData != null)
            {
                ODataPath path = tmpRequest.GetODataPath();
                var segment = path.Segments.OfType<KeyValuePathSegment>().FirstOrDefault();
                if (segment != null)
                {
                    // Convert the segment into the key type.
                    key = (TKey)ODataUriUtils.ConvertFromUriLiteral(
                        segment.Value, ODataVersion.V3);
                }
            }
            return key;
        }

        //delete a link
        public override void DeleteLink([FromODataUri] int key, string navigationProperty,
    [FromBody] Uri link)
        {
            Product product = _context.Products.FirstOrDefault(p => p.ID == key);
            if (product == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            switch (navigationProperty)
            {
                case "Supplier":
                    product.Supplier = null;
                    break;

                default:
                    base.DeleteLink(key, navigationProperty, link);
                    break;

            }
            _context.SaveChanges();
        }
    }
}
