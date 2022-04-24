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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Order>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get()
        {
            var orders = await _orderService.GetOrders();

            if (orders.Count <= 0) return NotFound();

            _logger.LogInformation($"Get order list count: {orders.Count}");
            _logger.LogInformation($"Get order list: {JsonConvert.SerializeObject(orders)}");
            return Ok(orders);
        }

        // GET api/<OrderController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Order))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var order = await _orderService.GetOrder(id);
            if (order == null) return NotFound();

            _logger.LogInformation($"Get order by id: {JsonConvert.SerializeObject(order)}");
            return Ok(order);
        }

        // POST api/<OrderController>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Order))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] OrderDto order)
        {
            var createdOrder = await _orderService.AddOrder(order);
            if (createdOrder == null) return BadRequest();

            _logger.LogInformation($"Post order: {JsonConvert.SerializeObject(createdOrder)}");
            return StatusCode(StatusCodes.Status201Created, createdOrder);
        }

        // PUT api/<OrderController>/5
        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Customer))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(int id, [FromBody] OrderDto order)
        {
            var updatedOrder = await _orderService.UpdateOrder(id, order);
            if (updatedOrder == null) return BadRequest();

            _logger.LogInformation($"Put order: {JsonConvert.SerializeObject(updatedOrder)}");
            return Ok(updatedOrder);
        }

        // DELETE api/<OrderController>/5
        [HttpDelete("{id}")]
        public async  Task<IActionResult> Delete(int id)
        {
            await _orderService.DeleteOrder(id);
            _logger.LogInformation($"Order Deleted id:{id}");
            return Ok();
        }
    }
}
