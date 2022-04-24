using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DemoProject.ApplicationCore.DTO;
using DemoProject.ApplicationCore.Entities;
using DemoProject.ApplicationCore.Interfaces;
using DemoProject.ApplicationCore.Services;
using DemoProject.Tests.Helpers;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DemoProject.Tests.Services
{
    public class OrderServiceTests
    {
        private readonly Mock<ILogger<OrderService>> _loggerMock = new();
        private readonly Mock<IBaseRepository<Order>> _orderRepoMock = new();
        private readonly Mock<IBaseRepository<Customer>> _customerRepoMock = new();
        private readonly Mock<IBaseRepository<Product>> _productRepoMock = new();

        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            _orderService = new OrderService(_loggerMock.Object, _orderRepoMock.Object,_customerRepoMock.Object,_productRepoMock.Object);
        }

        [Fact]
        public void OrderService_Constructor_missing_logger_throws_error()
        {
            //Arrange
            //Act
            var sut = Assert.Throws<ArgumentNullException>(() => new OrderService(null, _orderRepoMock.Object, _customerRepoMock.Object, _productRepoMock.Object));
            //Assert
            Assert.Equal("Value cannot be null. (Parameter 'logger')", sut.Message);
        }

        [Fact]
        public void OrderService_Constructor_missing_order_repo_throws_error()
        {
            //Arrange
            //Act
            var sut = Assert.Throws<ArgumentNullException>(() => new OrderService(_loggerMock.Object, null, _customerRepoMock.Object, _productRepoMock.Object));
            //Assert
            Assert.Equal("Value cannot be null. (Parameter 'orderRepository')", sut.Message);
        }

        [Fact]
        public void OrderService_Constructor_missing_customer_repo_throws_error()
        {
            //Arrange
            //Act
            var sut = Assert.Throws<ArgumentNullException>(() => new OrderService(_loggerMock.Object, _orderRepoMock.Object,null, _productRepoMock.Object));
            //Assert
            Assert.Equal("Value cannot be null. (Parameter 'customerRepository')", sut.Message);
        }

        [Fact]
        public void OrderService_Constructor_missing_product_repo_throws_error()
        {
            //Arrange
            //Act
            var sut = Assert.Throws<ArgumentNullException>(() => new OrderService(_loggerMock.Object, _orderRepoMock.Object, _customerRepoMock.Object, null));
            //Assert
            Assert.Equal("Value cannot be null. (Parameter 'productRepository')", sut.Message);
        }


        [Fact]
        public async Task GetOrders_success()
        {
            //Arrange
            _orderRepoMock.Setup(x => x.GetAllReadOnlyAsync(It.IsAny<Expression<Func<Order, object>>[]>())).ReturnsAsync(OrderData.GetOrders());
            //Act
            var sut = await _orderService.GetOrders();
            //Assert
            sut.Should().BeEquivalentTo(OrderData.GetOrders());
        }

        [Fact]
        public async Task GetOrder_success()
        {
            //Arrange
            var Order = OrderData.GetOrders().First();
            _orderRepoMock.Setup(x => x.FindByReadOnlyAsync(It.IsAny<Expression<Func<Order, bool>>>(),
                It.IsAny<Expression<Func<Order, object>>[]>())).ReturnsAsync(OrderData.GetOrders());
            //Act
            var sut = await _orderService.GetOrder(1);
            //Assert
            sut.Should().BeEquivalentTo(Order);
        }


        [Fact]
        public async Task AddOrder_success()
        {
            //Arrange
            OrderDto newOrderDto = new()
            {
                CustomerId = 2,
                ProductId = 3
            };

            Order newOrder = new()
            {
                Id = 4,
                CustomerId = 2,
                ProductId = 3
            };
            _customerRepoMock.Setup(x => x.CheckExistsByPredicate(It.IsAny<Expression<Func<Customer, bool>>>())).ReturnsAsync(true);
            _productRepoMock.Setup(x => x.CheckExistsByPredicate(It.IsAny<Expression<Func<Product, bool>>>())).ReturnsAsync(true);
            _orderRepoMock.Setup(x => x.AddAsync(It.IsAny<Order>())).ReturnsAsync(newOrder);

            //Act
            var sut = await _orderService.AddOrder(newOrderDto);

            //Assert
            sut.Should().BeEquivalentTo(newOrder);
        }

        [Fact]
        public async Task UpdateOrder_success()
        {
            //Arrange
            OrderDto updateOrderDto = new()
            {
                CustomerId = 1,
                ProductId = 3
            };

            Order updateOrder = new()
            {
                Id = 1,
                CustomerId = 1,
                Customer = CustomerData.GetCustomers()[0],
                ProductId = 3,
                Product = ProductData.GetProducts()[2],
                CreatedOn = new DateTime(2022, 04, 22)
            };

            _orderRepoMock.Setup(x => x.FindByReadOnlyAsync(It.IsAny<Expression<Func<Order, bool>>>(),
                It.IsAny<Expression<Func<Order, object>>[]>())).ReturnsAsync(new List<Order>{ updateOrder });
            _customerRepoMock.Setup(x => x.CheckExistsByPredicate(It.IsAny<Expression<Func<Customer, bool>>>())).ReturnsAsync(true);
            _productRepoMock.Setup(x => x.CheckExistsByPredicate(It.IsAny<Expression<Func<Product, bool>>>())).ReturnsAsync(true);
            _orderRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(OrderData.GetOrders()[0]);
            _orderRepoMock.Setup(x => x.UpdateAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);

            //Act
            var sut = await _orderService.UpdateOrder(1, updateOrderDto);

            //Assert
            sut.Should().BeEquivalentTo(updateOrder);
        }

        [Fact]
        public async Task UpdateOrder_returns_null()
        {
            //Arrange
            OrderDto updateOrderDto = new()
            {
                CustomerId = 1,
                ProductId = 3
            };
            _orderRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).Throws(new Exception());

            //Act
            var sut =  await _orderService.UpdateOrder(1, updateOrderDto);

            //Assert
            sut.Should().BeNull();

            _loggerMock.Verify(x => x.Log(LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()));
        }

        [Fact]
        public async Task DeleteOrder_success()
        {
            //Arrange
            var Order = OrderData.GetOrders().First();
            _orderRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(Order);
            _orderRepoMock.Setup(x => x.DeleteAsync(It.IsAny<Order>())).Returns(Task.CompletedTask);

            //Act
            await _orderService.DeleteOrder(1);
            //Assert
            _orderRepoMock.Verify(x => x.DeleteAsync(It.IsAny<Order>()), Times.Once);
        }
    }
}

