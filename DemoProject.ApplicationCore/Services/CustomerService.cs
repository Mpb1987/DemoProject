using DemoProject.ApplicationCore.DTO;
using DemoProject.ApplicationCore.Entities;
using DemoProject.ApplicationCore.Interfaces;
using Microsoft.Extensions.Logging;

namespace DemoProject.ApplicationCore.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ILogger _logger;
        private readonly IBaseRepository<Customer> _customerRepository;

        public CustomerService(ILogger<CustomerService> logger, IBaseRepository<Customer> customerRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        }

        public async Task<List<Customer>> GetCustomers()
        {
            try
            {
                return await _customerRepository.ListAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<Customer> GetCustomer(int id)
        {
            try
            {
                return await _customerRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<Customer> AddCustomer(CustomerDto customer)
        {
            try
            {
                return await _customerRepository.AddAsync(new Customer
                {
                    FirstName = customer.FirstName,
                    Surname = customer.Surname
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<Customer?> UpdateCustomer(int id, CustomerDto customer)
        {
            try
            {
                var currentCustomer = await _customerRepository.GetByIdAsync(id);
                currentCustomer.FirstName = customer.FirstName;
                currentCustomer.Surname = customer.Surname;
                await _customerRepository.UpdateAsync(currentCustomer);

                return currentCustomer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task DeleteCustomer(int id)
        {
            try
            {
                var customer = await _customerRepository.GetByIdAsync(id);
                await _customerRepository.DeleteAsync(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
