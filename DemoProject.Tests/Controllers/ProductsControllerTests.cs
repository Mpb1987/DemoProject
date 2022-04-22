using System;
using System.Linq;
using System.Threading.Tasks;
using DemoProject.Api.Controllers;
using DemoProject.ApplicationCore.Entities;
using DemoProject.ApplicationCore.Interfaces;
using DemoProject.Tests.Helpers;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DemoProject.Tests.Controllers
{
    public class ProductsControllerTests
    {
        private readonly Mock<ILogger<ProductController>> _loggerMock = new();
        private readonly Mock<IProductService> _productServiceMock = new();

        private readonly ProductController _controller;

        public ProductsControllerTests()
        {
            _controller = new ProductController(_loggerMock.Object, _productServiceMock.Object);
        }

        [Fact]
        public void ProductController_Constructor_missing_logger_throws_error()
        {
            var sut = Assert.Throws<ArgumentNullException>(() => new ProductController(null, _productServiceMock.Object));
        }

        [Fact]
        public void ProductController_Constructor_missing_Product_throws_error()
        {
            var sut = Assert.Throws<ArgumentNullException>(() => new ProductController(_loggerMock.Object, null));
        }

        [Fact]
        public async Task GetList_successAsync()
        {
            //Arrange
            _productServiceMock.Setup(x => x.GetProducts()).ReturnsAsync(ProductData.GetProducts());
            //Act
            var sut = await _controller.Get();

            //Assert
            sut.Should().BeEquivalentTo(ProductData.GetProducts());

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
            var Product = ProductData.GetProducts().First();
            _productServiceMock.Setup(x => x.GetProduct(It.IsAny<int>())).ReturnsAsync(Product);
            //Act
            var sut = await _controller.Get(1);

            //Assert
            sut.Should().BeEquivalentTo(Product);

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
            Product newProduct = new()
            {
                ProductId = 4,
                ProductDescription = "new product"
            };

            _productServiceMock.Setup(x => x.AddProduct(It.IsAny<string>())).ReturnsAsync(newProduct);
            //Act
            var sut = await _controller.Post("new product");

            //Assert
            sut.Should().BeEquivalentTo(newProduct);

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

            Product Product = new()
            {
                ProductId = 4,
                ProductDescription = "update product"
            };

            _productServiceMock.Setup(x => x.UpdateProduct(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(Product);
            //Act
            var sut = await _controller.Put(4, "update product");

            //Assert
            sut.Should().BeEquivalentTo(Product);
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
            _productServiceMock.Setup(x => x.DeleteProduct(It.IsAny<int>())).Returns(Task.CompletedTask);
            //Act
            var res = _controller.Delete(1);
            //Assert
            //Assert.Equal(TaskStatus.RanToCompletion, res.Status);
            res.Status.Should().Be(TaskStatus.RanToCompletion);

            _loggerMock.Verify(x => x.Log(LogLevel.Information,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()));
        }
    }
}
