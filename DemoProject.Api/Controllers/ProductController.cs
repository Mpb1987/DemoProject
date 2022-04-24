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
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;

        public ProductController(ILogger<ProductController> logger, IProductService productService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }

        // GET: api/<ProductController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Product>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get()
        {
            var products = await _productService.GetProducts();

            if (products.Count <= 0) return NotFound();
            
            _logger.LogInformation($"Get Product list count: {products.Count}");
            _logger.LogInformation($"Get Product list: {JsonConvert.SerializeObject(products)}");
            return Ok(products);
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var product = await _productService.GetProduct(id);
            if (product == null) return NotFound();

            _logger.LogInformation($"Get Product by id: {JsonConvert.SerializeObject(product)}");
            return Ok(product);
        }

        // POST api/<ProductController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] string description)
        {
            var product = await _productService.AddProduct(description);

            if(product ==null) return NotFound();

            _logger.LogInformation($"Post Product: {JsonConvert.SerializeObject(product)}");
            return StatusCode(StatusCodes.Status201Created, product);
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(int id, [FromBody] string description)
        {
            var updatedProduct = await _productService.UpdateProduct(id, description);
            if (updatedProduct == null) return BadRequest();

            _logger.LogInformation($"Put Product: {JsonConvert.SerializeObject(updatedProduct)}");
            return Ok(updatedProduct);
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteProduct(id);
            _logger.LogInformation($"Product Deleted id:{id}");
            return Ok();
        }
    }
}
