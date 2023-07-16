using eShop.Services.ShoppingCartAPI.Models.Dto;

namespace eShop.Services.ShoppingCartAPI.Services.IService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProducts();
    }
}
