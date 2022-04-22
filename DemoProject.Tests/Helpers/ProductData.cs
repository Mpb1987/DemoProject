using System.Collections.Generic;
using DemoProject.ApplicationCore.Entities;

namespace DemoProject.Tests.Helpers
{
    public static class ProductData
    {
        public static List<Product> GetProducts()
        {
            return new List<Product>()
            {
                new()
                {
                    ProductId = 1,
                    ProductDescription = "Laptop"
                },
                new()
                {
                    ProductId = 1,
                    ProductDescription = "Mobile Phone"
                },
                new()
                {
                    ProductId = 1,
                    ProductDescription = "TV"
                }
            };
        }
    }
}
