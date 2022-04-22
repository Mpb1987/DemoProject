using DemoProject.ApplicationCore.DTO;
using DemoProject.ApplicationCore.Entities;

namespace DemoProject.ApplicationCore.Interfaces
{
    public interface IProductService 
    {
        Task<List<Product>> GetProducts();
        Task<Product> GetProduct(int id);
        Task<Product> AddProduct(string productDescription);
        Task<Product?> UpdateProduct(int id, string productDescription);
        Task DeleteProduct(int id);
    }
}
