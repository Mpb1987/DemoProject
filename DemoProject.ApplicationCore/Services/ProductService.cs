using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoProject.ApplicationCore.DTO;
using DemoProject.ApplicationCore.Entities;
using DemoProject.ApplicationCore.Interfaces;
using Microsoft.Extensions.Logging;

namespace DemoProject.ApplicationCore.Services
{
    public class ProductService : IProductService
    {
        private readonly ILogger _logger;
        private readonly IBaseRepository<Product> _productRepository;

        public ProductService(ILogger<ProductService> logger, IBaseRepository<Product> productRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        public async Task<List<Product>> GetProducts()
        {
            try
            {
                return await _productRepository.ListAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<Product> GetProduct(int id)
        {
            try
            {
                return await _productRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<Product> AddProduct(string productDescription)
        {
            try
            {
                return await _productRepository.AddAsync(new Product
                {
                    ProductDescription = productDescription
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<Product?> UpdateProduct(int id, string productDescription)
        {
            try
            {
                var currentProduct = await _productRepository.GetByIdAsync(id);

                currentProduct.ProductDescription = productDescription;
                await _productRepository.UpdateAsync(currentProduct);

                return currentProduct;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task DeleteProduct(int id)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(id);
                await _productRepository.DeleteAsync(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}