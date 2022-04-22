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
        public async Task<List<Product>> Get()
        {
            var result = await _productService.GetProducts();
            _logger.LogInformation($"Get Product list count: {result.Count}");
            _logger.LogInformation($"Get Product list: {JsonConvert.SerializeObject(result)}");
            return result;
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public async Task<Product> Get(int id)
        {
            var result = await _productService.GetProduct(id);
            _logger.LogInformation($"Get Product by id: {JsonConvert.SerializeObject(result)}");
            return result;
        }

        // POST api/<ProductController>
        [HttpPost]
        public async Task<Product> Post([FromBody] string description)
        {
            var result = await _productService.AddProduct(description);
            _logger.LogInformation($"Post Product: {JsonConvert.SerializeObject(result)}");
            return result;
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public async Task<Product?> Put(int id, [FromBody] string description)
        {
            var updatedProduct = await _productService.UpdateProduct(id, description);
            _logger.LogInformation($"Put Product: {JsonConvert.SerializeObject(updatedProduct)}");
            return updatedProduct;
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _productService.DeleteProduct(id);
            _logger.LogInformation($"Product Deleted id:{id}");
        }
    }
}
