using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoProject.Api.Controllers;
using DemoProject.ApplicationCore.DTO;
using DemoProject.ApplicationCore.Entities;
using DemoProject.ApplicationCore.Interfaces;
using DemoProject.Tests.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DemoProject.Tests.Controllers
{
    public class OrderControllerTests
    {
        private readonly Mock<ILogger<OrderController>> _loggerMock = new();
        public readonly Mock<IOrderService> OrderServiceMock = new();

        private readonly OrderController _controller;

        public OrderControllerTests()
        {
            _controller = new OrderController(_loggerMock.Object, OrderServiceMock.Object);
        }

        [Fact]
        public void OrderController_Constructor_missing_logger_throws_error()
        {
            var sut = Assert.Throws<ArgumentNullException>(() => new OrderController(null, OrderServiceMock.Object));
        }

        [Fact]
        public void OrderController_Constructor_missing_Order_throws_error()
        {
            var sut = Assert.Throws<ArgumentNullException>(() => new OrderController(_loggerMock.Object, null));
        }

        [Fact]
        public async Task GetList_successAsync()
        {
            //Arrange
            OrderServiceMock.Setup(x => x.GetOrders()).ReturnsAsync(OrderData.GetOrders());
            //Act
            var okObjectResult = await _controller.Get() as OkObjectResult;
            var sut = okObjectResult.Value as List<Order>;

            //Assert
            okObjectResult.Should().NotBeNull();
            sut.Should().BeEquivalentTo(OrderData.GetOrders());

            _loggerMock.Verify(x => x.Log(LogLevel.Information,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()));
        }

        [Fact]
        public async Task Get_by_id_successAsync()
        {
            //Arrange
            var order = OrderData.GetOrders().First();
            OrderServiceMock.Setup(x => x.GetOrder(It.IsAny<int>())).ReturnsAsync(order);
            //Act
            var okObjectResult = await _controller.Get(1) as OkObjectResult;
            var sut = okObjectResult.Value as Order;

            //Assert
            okObjectResult.Should().NotBeNull();
            sut.Should().BeEquivalentTo(order);

            _loggerMock.Verify(x => x.Log(LogLevel.Information,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()));
        }

        [Fact]
        public async Task POST_successAsync()
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

            OrderServiceMock.Setup(x => x.AddOrder(It.IsAny<OrderDto>())).ReturnsAsync(newOrder);
            //Act
            var createdObjectResult = await _controller.Post(newOrderDto) as ObjectResult;
            var sut = createdObjectResult.Value as Order;

            //Assert
            createdObjectResult.Should().NotBeNull();
            sut.Should().BeEquivalentTo(newOrder);

            _loggerMock.Verify(x => x.Log(LogLevel.Information,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()));
        }

        [Fact]
        public async Task PUT_successAsync()
        {
            //Arrange
            OrderDto updateOrderDto = new()
            {
                CustomerId = 1,
                ProductId = 3
            };

            Order order = new()
            {
                Id = 4,
                CustomerId = 1,
                ProductId = 3
            };

            OrderServiceMock.Setup(x => x.UpdateOrder(It.IsAny<int>(), It.IsAny<OrderDto>())).ReturnsAsync(order);
            //Act
            var okObjectResult = await _controller.Put(4, updateOrderDto) as OkObjectResult;
            var sut = okObjectResult.Value as Order;


            //Assert
            okObjectResult.Should().NotBeNull();
            sut.Should().BeEquivalentTo(order);
            _loggerMock.Verify(x => x.Log(LogLevel.Information,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()));
        }
        [Fact]
        public void Delete_call_success()
        {
            //Arrange
            OrderServiceMock.Setup(x => x.DeleteOrder(It.IsAny<int>())).Returns(Task.CompletedTask);

            //Act
            var res = _controller.Delete(1);

            //Assert
            res.Status.Should().Be(TaskStatus.RanToCompletion);

            _loggerMock.Verify(x => x.Log(LogLevel.Information,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()));
        }
    }
}
