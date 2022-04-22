using System;
using System.Linq;
using System.Threading.Tasks;
using DemoProject.Api.Controllers;
using DemoProject.ApplicationCore.DTO;
using DemoProject.ApplicationCore.Entities;
using DemoProject.ApplicationCore.Interfaces;
using DemoProject.Tests.Helpers;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Xunit;
using Moq;


namespace DemoProject.Tests.Controllers
{
    public class CustomerControllerTests
    {
        private readonly Mock<ILogger<CustomerController>> _loggerMock = new();
        private readonly Mock<ICustomerService> _customerServiceMock = new();

        private readonly CustomerController _controller;

        public CustomerControllerTests()
        {
            _controller = new CustomerController(_loggerMock.Object, _customerServiceMock.Object);
        }

        [Fact]
        public void CustomerController_Constructor_missing_logger_throws_error()
        {
            var sut = Assert.Throws<ArgumentNullException>(() => new CustomerController(null, _customerServiceMock.Object));
        }

        [Fact]
        public void CustomerController_Constructor_missing_Customer_throws_error()
        {
            var sut = Assert.Throws<ArgumentNullException>(() => new CustomerController(_loggerMock.Object, null));
        }

        [Fact]
        public async Task GetList_successAsync()
        {
            //Arrange
            _customerServiceMock.Setup(x => x.GetCustomers()).ReturnsAsync(CustomerData.GetCustomers());
            //Act
            var sut = await _controller.Get();

            //Assert
            sut.Should().BeEquivalentTo(CustomerData.GetCustomers());

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
            var customer = CustomerData.GetCustomers().First();
            _customerServiceMock.Setup(x => x.GetCustomer(It.IsAny<int>())).ReturnsAsync(customer);
            //Act
            var sut = await _controller.Get(1);

            //Assert
            sut.Should().BeEquivalentTo(customer);

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
            CustomerDto newCustomerDto = new()
            {
                FirstName = "test",
                Surname = "DTO"
            };

            Customer newCustomer = new()
            {
                CustomerId = 4,
                FirstName = "test",
                Surname = "DTO"
            };

            _customerServiceMock.Setup(x => x.AddCustomer(It.IsAny<CustomerDto>())).ReturnsAsync(newCustomer);
            //Act
            var sut = await _controller.Post(newCustomerDto);

            //Assert
            sut.Should().BeEquivalentTo(newCustomer);

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
            CustomerDto updateCustomerDto = new()
            {
                FirstName = "test",
                Surname = "newValue"
            };

            Customer customer = new()
            {
                CustomerId = 4,
                FirstName = "test",
                Surname = "newValue"
            };

            _customerServiceMock.Setup(x => x.UpdateCustomer(It.IsAny<int>(),It.IsAny<CustomerDto>())).ReturnsAsync(customer);
            //Act
            var sut = await _controller.Put(4,updateCustomerDto);

            //Assert
            sut.Should().BeEquivalentTo(customer);
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
            _customerServiceMock.Setup(x => x.DeleteCustomer(It.IsAny<int>())).Returns(Task.CompletedTask);
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
