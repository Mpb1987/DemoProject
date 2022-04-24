using System.Net.Mime;
using DemoProject.ApplicationCore.DTO;
using DemoProject.ApplicationCore.Entities;
using DemoProject.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Customer>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get()
        {
            var customers = await _customerService.GetCustomers();

            if (customers.Count <= 0) return NotFound();

            _logger.LogInformation($"Get customer list count: {customers.Count}");
            _logger.LogInformation($"Get customer list: {JsonConvert.SerializeObject(customers)}");
            return Ok(customers);

        }

        // GET api/<CustomerController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Customer))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var customer = await _customerService.GetCustomer(id);

            if (customer == null) return NotFound();
            _logger.LogInformation($"Get customer by id: {JsonConvert.SerializeObject(customer)}");
            return Ok(customer);

        }

        // POST api/<CustomerController>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Customer))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] CustomerDto customer)
        {
            var createdCustomer = await _customerService.AddCustomer(customer);
            if (createdCustomer == null) return BadRequest();

            _logger.LogInformation($"Post customer: {JsonConvert.SerializeObject(createdCustomer)}");
            return StatusCode(StatusCodes.Status201Created, createdCustomer);

        }

        // PUT api/<CustomerController>/5
        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Customer))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(int id, [FromBody] CustomerDto customer)
        {
            var updatedCustomer = await _customerService.UpdateCustomer(id, customer);
                if (updatedCustomer == null) return BadRequest();

            _logger.LogInformation($"Put customer: {JsonConvert.SerializeObject(updatedCustomer)}");
            return Ok(updatedCustomer);

        }

        // DELETE api/<CustomerController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> Delete(int id)
        {
            await _customerService.DeleteCustomer(id);
            _logger.LogInformation($"Customer Deleted id:{id}");
            return Ok();
        }
    }
}
