using System;
using System.Collections.Generic;
using DemoProject.ApplicationCore.Entities;

namespace DemoProject.Tests.Helpers
{
    public static class OrderData
    {
        public static List<Order> GetOrders()
        {
            return new List<Order>()
            {
                new()
                {
                    Id = 1,
                    CustomerId = 1,
                    Customer = CustomerData.GetCustomers()[0],
                    ProductId = 1,
                    Product = ProductData.GetProducts()[0],
                    CreatedOn = new DateTime(2022,04,22)
                },
                new()
                {
                    Id = 2,
                    CustomerId = 2,
                    Customer = CustomerData.GetCustomers()[1],
                    ProductId = 2,
                    Product = ProductData.GetProducts()[1],
                    CreatedOn = new DateTime(2022,04,22)
                },
                new()
                {
                    Id = 3,
                    CustomerId = 1,
                    Customer = CustomerData.GetCustomers()[0],
                    ProductId = 2,
                    Product = ProductData.GetProducts()[1],
                    CreatedOn = new DateTime(2022,04,22)
                }
            };
        }
    }
}
