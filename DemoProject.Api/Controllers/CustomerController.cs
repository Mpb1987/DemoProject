using DemoProject.ApplicationCore.DTO;
using DemoProject.ApplicationCore.Entities;
using DemoProject.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DemoProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ICustomerService _customerService;

        public CustomerController(ILogger<CustomerController> logger, ICustomerService customerService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _customerService = customerService ?? throw new ArgumentNullException(nameof(customerService));
        }

        // GET: api/<CustomerController>
        [HttpGet]
        public async Task<List<Customer>> Get()
        {
            var result= await _customerService.GetCustomers();
            _logger.LogInformation($"Get customer list: {result}");
            return result;
        }

        // GET api/<CustomerController>/5
        [HttpGet("{id}")]
        public async Task<Customer> Get(int id)
        {
            var result = await _customerService.GetCustomer(id);
            _logger.LogInformation($"Get customer by id: {result}");
            return result;
        }

        // POST api/<CustomerController>
        [HttpPost]
        public async Task<Customer> Post([FromBody] CustomerDto customer)
        {
            var result = await _customerService.AddCustomer(customer);
            _logger.LogInformation($"Post customer: {result}");
            return result;
        }

        // PUT api/<CustomerController>/5
        [HttpPut("{id}")]
        public async Task<Customer?> Put(int id, [FromBody] CustomerDto customer)
        {
            var updatedCustomer = await _customerService.UpdateCustomer(id, customer);
            _logger.LogInformation($"Put customer: {updatedCustomer}");

            return updatedCustomer;
        }

        // DELETE api/<CustomerController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _customerService.DeleteCustomer(id);
            _logger.LogInformation($"Customer Deleted id:{id}");
        }
    }
}
