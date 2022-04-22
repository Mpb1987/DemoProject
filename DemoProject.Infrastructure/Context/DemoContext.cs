using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoProject.ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace DemoProject.Infrastructure.Context
{
    public class DemoContext : DbContext
    {
        public DemoContext(DbContextOptions<DemoContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().HasData(new List<Customer>()
            {
                new()
                {
                    CustomerId = 1,
                    FirstName = "John",
                    Surname = "Smith"
                },
                new()
                {
                    CustomerId = 2,
                    FirstName = "Donna",
                    Surname = "Smith"
                },
                new ()
                {
                    CustomerId = 3,
                    FirstName = "Mark",
                    Surname = "Conner"

                }
            });

            modelBuilder.Entity<Product>().HasData(new List<Product>()
            {
                new()
                {
                    ProductId = 1,
                    ProductDescription = "laptop"
                },
                new ()
                {
                    ProductId = 2,
                    ProductDescription = "mobile phone"
                },
                new()
                {
                    ProductId = 3,
                    ProductDescription = "tablet"
                }
            });

            modelBuilder.Entity<Order>()
                .Property(d => d.CreatedOn)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Order>().HasData(new List<Order>()
            {
                new()
                {
                    Id = 1,
                    CustomerId = 1,
                    ProductId = 1
                },
                new()
                {
                    Id = 2,
                    CustomerId = 1,
                    ProductId = 2
                },
                new()
                {
                    Id = 3,
                    CustomerId = 2,
                    ProductId = 3
                }
            });

        }
    }
}
