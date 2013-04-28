using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODataClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new ODataService.Container(
            new Uri("http://localhost:55176/odata/"));

            container.SendingRequest2 += (s, e) =>
            {
                Console.WriteLine("{0} {1}",
                    e.RequestMessage.Method, e.RequestMessage.Url);
            };

            // Get all products
            foreach (var product in container.Products)
            {
                Console.WriteLine("{0} {1} {2}", product.Name, product.Price,
                    product.Category);
            }

            // Select products
            var products = from p in container.Products where p.ID == 1 select p;
            var product1 = products.FirstOrDefault();
            if (product1 != null)
            {
                Console.WriteLine("{0} {1} {2}", product1.Name, product1.Price,
                    product1.Category);
            }

            Console.ReadLine(); //hold console open
        }
    }
}
