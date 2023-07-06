using eShop.Services.ProductAPI.Models;

namespace eShop.Services.ProductAPI.Service.IService
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<Product?> GetProductAsync(int id);
        Task<Product?> CreateProductAsync(Product product);
        Task<Product?> UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);
    }
}
