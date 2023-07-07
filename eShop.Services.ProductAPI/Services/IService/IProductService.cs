using eShop.Services.ProductAPI.Models;
using eShop.Services.ProductAPI.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;

namespace eShop.Services.ProductAPI.Service.IService
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<Product?> GetProductAsync(int id);
        Task<Product?> CreateProductAsync(Product product);
        Task<Product?> UpdateProductAsync(Product product);
        Task<Product?> PatchProductAsync(int id, JsonPatchDocument<Product> patchProduct);
        Task DeleteProductAsync(int id);
    }
}
