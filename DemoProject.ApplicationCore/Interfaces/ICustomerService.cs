using DemoProject.ApplicationCore.DTO;
using DemoProject.ApplicationCore.Entities;

namespace DemoProject.ApplicationCore.Interfaces
{
    public interface ICustomerService 
    {
        Task<List<Customer>> GetCustomers();
        Task<Customer?> GetCustomer(int id);
        Task<Customer?> AddCustomer(CustomerDto customer);
        Task<Customer?> UpdateCustomer(int id, CustomerDto customer);
        Task DeleteCustomer(int id);
    }
}
