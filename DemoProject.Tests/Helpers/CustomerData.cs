using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoProject.ApplicationCore.Entities;

namespace DemoProject.Tests.Helpers
{
    public static class CustomerData
    {
        public static List<Customer> GetCustomers()
        {
            return new List<Customer>()
            {
                new()
                {
                    CustomerId = 1,
                    FirstName = "Test1",
                    Surname = "Jones"
                },
                new()
                {
                    CustomerId = 2,
                    FirstName = "Test2",
                    Surname = "Smith"
                },
                new()
                {
                    CustomerId = 3,
                    FirstName = "Test3",
                    Surname = "Collins"
                },
            };
        }
    }
}
    