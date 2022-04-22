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
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;

        public OrderController(ILogger<OrderController> logger, IOrderService orderService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
        }

        // GET: api/<OrderController>
        [HttpGet]
        public async Task<List<Order>> Get()
        {
            var result = await _orderService.GetOrders();
            _logger.LogInformation($"Get order list count: {result.Count}");
            _logger.LogInformation($"Get order list: {JsonConvert.SerializeObject(result)}");
            return result;
        }

        // GET api/<OrderController>/5
        [HttpGet("{id}")]
        public async Task<Order> Get(int id)
        {
            var result = await _orderService.GetOrder(id);
            _logger.LogInformation($"Get order by id: {JsonConvert.SerializeObject(result)}");
            
            return result;
        }

        // POST api/<OrderController>
        [HttpPost]
        public async Task<Order> Post([FromBody] OrderDto order)
        {
            var result = await _orderService.AddOrder(order);
            _logger.LogInformation($"Post order: {JsonConvert.SerializeObject(result)}");
            return result;
        }

        // PUT api/<OrderController>/5
        [HttpPut("{id}")]
        public async Task<Order?> Put(int id, [FromBody] OrderDto order)
        {
            var updatedOrder = await _orderService.UpdateOrder(id, order);
            _logger.LogInformation($"Put order: {JsonConvert.SerializeObject(updatedOrder)}");

            return updatedOrder;
        }

        // DELETE api/<OrderController>/5
        [HttpDelete("{id}")]
        public async  Task Delete(int id)
        {
            await _orderService.DeleteOrder(id);
            _logger.LogInformation($"Order Deleted id:{id}");
        }
    }
}
