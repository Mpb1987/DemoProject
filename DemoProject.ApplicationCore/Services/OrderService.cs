using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoProject.ApplicationCore.DTO;
using DemoProject.ApplicationCore.Entities;
using DemoProject.ApplicationCore.Interfaces;
using Microsoft.Extensions.Logging;

namespace DemoProject.ApplicationCore.Services
{
    public class OrderService : IOrderService
    {
        private readonly ILogger _logger;
        private readonly IBaseRepository<Order> _orderRepository;
        private readonly IBaseRepository<Customer> _customerRepository;
        private readonly IBaseRepository<Product> _productRepository;
        
        public OrderService(ILogger<OrderService> logger,
            IBaseRepository<Order> orderRepository,
            IBaseRepository<Customer> customerRepository,
            IBaseRepository<Product> productRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        public async Task<List<Order>> GetOrders()
        {
            try
            {
                return await _orderRepository.GetAllReadOnlyAsync(i => i.Customer, i => i.Product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<Order> GetOrder(int id)
        {
            try
            {
                var record = await _orderRepository.FindByReadOnlyAsync(x => x.Id == id, i => i.Customer, i => i.Product);
                return record.First();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<Order> AddOrder(OrderDto order)
        {
            try
            {
                var checkCustomer = await _customerRepository.CheckExistsByPredicate(x => x.CustomerId == order.CustomerId)!;
                var checkProduct = await _productRepository.CheckExistsByPredicate(x => x.ProductId == order.ProductId)!;

                if (!checkProduct || !checkCustomer) throw new ArgumentException("invalid parameter");

                return await _orderRepository.AddAsync(new Order
                {
                    CustomerId = order.CustomerId,
                    ProductId = order.ProductId
                });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<Order?> UpdateOrder(int id, OrderDto order)
        {
            try
            {
                var checkCustomer = await _customerRepository.CheckExistsByPredicate(x => x.CustomerId == order.CustomerId)!;
                var checkProduct = await _productRepository.CheckExistsByPredicate(x => x.ProductId == order.ProductId)!;

                if (!checkProduct || !checkCustomer) throw new ArgumentException("invalid parameter");

                var currentOrder = await _orderRepository.GetByIdAsync(id);
                currentOrder.CustomerId = order.CustomerId;
                currentOrder.ProductId = order.ProductId;
                await _orderRepository.UpdateAsync(currentOrder);

                return currentOrder;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task DeleteOrder(int id)
        {
            try
            {
                var order = await _orderRepository.GetByIdAsync(id);
                await _orderRepository.DeleteAsync(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
