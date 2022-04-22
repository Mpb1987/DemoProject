using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoProject.ApplicationCore.DTO;
using DemoProject.ApplicationCore.Entities;

namespace DemoProject.ApplicationCore.Interfaces
{
    public interface IOrderService
    {
        Task<List<Order>> GetOrders();
        Task<Order> GetOrder(int id);
        Task<Order> AddOrder(OrderDto order);
        Task<Order?> UpdateOrder(int id, OrderDto order);
        Task DeleteOrder(int id);
    }
}
