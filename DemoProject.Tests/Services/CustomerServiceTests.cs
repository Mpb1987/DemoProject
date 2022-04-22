﻿using System;
using System.Linq;
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
    public class CustomerServiceTests
    {
        private readonly Mock<ILogger<CustomerService>> _loggerMock = new();
        public readonly Mock<IBaseRepository<Customer>> CustomerRepoMock = new();

        private readonly CustomerService _customerService;

        public CustomerServiceTests()
        {
            _customerService = new CustomerService(_loggerMock.Object, CustomerRepoMock.Object);
        }

        [Fact]
        public void CustomerService_Constructor_missing_logger_throws_error()
        {
            //Arrange
            //Act
            var sut = Assert.Throws<ArgumentNullException>(() => new CustomerService(null, CustomerRepoMock.Object));
            //Assert
            Assert.Equal("Value cannot be null. (Parameter 'logger')", sut.Message);
        }

        [Fact]
        public void CustomerService_Constructor_missing_repo_throws_error()
        {
            //Arrange
            //Act
            var sut = Assert.Throws<ArgumentNullException>(() => new CustomerService(_loggerMock.Object, null));
            //Assert
            Assert.Equal("Value cannot be null. (Parameter 'customerRepository')", sut.Message);
        }


        [Fact]
        public async Task GetCustomers_success()
        {
            //Arrange
            CustomerRepoMock.Setup(x => x.ListAllAsync()).ReturnsAsync(CustomerData.GetCustomers());
            //Act
            var sut = await _customerService.GetCustomers();
            //Assert
            sut.Should().BeEquivalentTo(CustomerData.GetCustomers());
        }

        [Fact]
        public async Task GetCustomer_success()
        {
            //Arrange
            var customer = CustomerData.GetCustomers().First();
            CustomerRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(customer);
            //Act
            var sut = await _customerService.GetCustomer(1);
            //Assert
            sut.Should().BeEquivalentTo(customer);
        }


        [Fact]
        public async Task AddCustomer_success()
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

            CustomerRepoMock.Setup(x => x.AddAsync(It.IsAny<Customer>())).ReturnsAsync(newCustomer);

            //Act
            var sut = await _customerService.AddCustomer(newCustomerDto);

            //Assert
            sut.Should().BeEquivalentTo(newCustomer);
        }

        [Fact]
        public async Task UpdateCustomer_success()
        {
            //Arrange
            CustomerDto updateCustomerDto = new()
            {
                FirstName = "changing",
                Surname = "customer"
            };

            Customer updateCustomer = new()
            {
                CustomerId = 1,
                FirstName = "changing",
                Surname = "customer"
            };

            CustomerRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(CustomerData.GetCustomers()[0]);
            CustomerRepoMock.Setup(x => x.UpdateAsync(It.IsAny<Customer>())).Returns(Task.CompletedTask);

            //Act
            var sut = await _customerService.UpdateCustomer(1, updateCustomerDto);

            //Assert
            sut.Should().BeEquivalentTo(updateCustomer);
        }

        [Fact]
        public async Task UpdateCustomer_throws_exception()
        {
            //Arrange
            CustomerDto updateCustomerDto = new()
            {
                FirstName = "changing",
                Surname = "customer"
            };
            CustomerRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).Throws(new Exception());

            //Act
            Func<Task> sut = async () => await _customerService.UpdateCustomer(1, updateCustomerDto);

            //Assert
            await sut.Should().ThrowAsync<Exception>();

            _loggerMock.Verify(x => x.Log(LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()));
        }

        [Fact]
        public async Task DeleteCustomer_success()
        {
            //Arrange
            var customer = CustomerData.GetCustomers().First();
            CustomerRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(customer);
            CustomerRepoMock.Setup(x => x.DeleteAsync(It.IsAny<Customer>())).Returns(Task.CompletedTask);

            //Act
            await _customerService.DeleteCustomer(1);
            //Assert
            CustomerRepoMock.Verify(x=>x.DeleteAsync(It.IsAny<Customer>()),Times.Once);
        }
    }
}
