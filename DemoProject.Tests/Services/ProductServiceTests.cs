using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class ProductServiceTests
    {
        private readonly Mock<ILogger<ProductService>> _loggerMock = new();
        private readonly Mock<IBaseRepository<Product>> _productRepoMock = new();

        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _productService = new ProductService(_loggerMock.Object, _productRepoMock.Object);
        }

        [Fact]
        public void ProductService_Constructor_missing_logger_throws_error()
        {
            //Arrange
            //Act
            var sut = Assert.Throws<ArgumentNullException>(() => new ProductService(null, _productRepoMock.Object));
            //Assert
            Assert.Equal("Value cannot be null. (Parameter 'logger')", sut.Message);
        }

        [Fact]
        public void ProductService_Constructor_missing_repo_throws_error()
        {
            //Arrange
            //Act
            var sut = Assert.Throws<ArgumentNullException>(() => new ProductService(_loggerMock.Object, null));
            //Assert
            Assert.Equal("Value cannot be null. (Parameter 'productRepository')", sut.Message);
        }


        [Fact]
        public async Task GetProducts_success()
        {
            //Arrange
            _productRepoMock.Setup(x => x.ListAllAsync()).ReturnsAsync(ProductData.GetProducts());
            //Act
            var sut = await _productService.GetProducts();
            //Assert
            sut.Should().BeEquivalentTo(ProductData.GetProducts());
        }

        [Fact]
        public async Task GetProduct_success()
        {
            //Arrange
            var Product = ProductData.GetProducts().First();
            _productRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(Product);
            //Act
            var sut = await _productService.GetProduct(1);
            //Assert
            sut.Should().BeEquivalentTo(Product);
        }


        [Fact]
        public async Task AddProduct_success()
        {
            //Arrange

            Product newProduct = new()
            {
                ProductId = 4,
                ProductDescription = "new product"
            };

            _productRepoMock.Setup(x => x.AddAsync(It.IsAny<Product>())).ReturnsAsync(newProduct);

            //Act
            var sut = await _productService.AddProduct("new product");

            //Assert
            sut.Should().BeEquivalentTo(newProduct);
        }

        [Fact]
        public async Task UpdateProduct_success()
        {
            //Arrange
            Product updateProduct = new()
            {
                ProductId = 1,
                ProductDescription = "update product"
            };

            _productRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(ProductData.GetProducts()[0]);
            _productRepoMock.Setup(x => x.UpdateAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

            //Act
            var sut = await _productService.UpdateProduct(1, "update product");

            //Assert
            sut.Should().BeEquivalentTo(updateProduct);
        }

        [Fact] public async Task UpdateProduct_returns_null()
        {
            //Arrange
            _productRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).Throws(new Exception());

            //Act
           var sut =await _productService.UpdateProduct(1, "Update product");

            //Assert
            sut.Should().BeNull();

            _loggerMock.Verify(x => x.Log(LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()));
        }

        [Fact]
        public async Task DeleteProduct_success()
        {
            //Arrange
            var Product = ProductData.GetProducts().First();
            _productRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(Product);
            _productRepoMock.Setup(x => x.DeleteAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

            //Act
            await _productService.DeleteProduct(1);
            //Assert
            _productRepoMock.Verify(x => x.DeleteAsync(It.IsAny<Product>()), Times.Once);
        }
    }
}
