using eShop.Web.Models.Dto;

namespace eShop.Services.ProductAPI.Service.IService
{
    public interface IProductService
    {
        Task<ResponseDto?> GetProductsAsync();
        Task<ResponseDto?> GetProductAsync(int id);
        Task<ResponseDto?> CreateProductAsync(ProductDto product);
        Task<ResponseDto?> UpdateProductAsync(ResponseDto product);
        Task<ResponseDto?> DeleteProductAsync(int id);
    }
}
