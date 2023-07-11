using eShop.Services.ShoppingCartAPI.Models;

namespace eShop.Services.ShoppingCartAPI.Service.IService
{
    public interface IShoppingCartService
    {
        Task<CartDetails?> GetCartDetailsAsync(int productId, int cartHeaderId);
        Task<CartHeader?> GetCartHeaderAsync(string? userId);
        Task<CartHeader?> AddCartHeaderAsync(CartHeader cartHeader);
        Task<CartDetails?> AddCartDetailsAsync(CartDetails cartDetails);
        Task<CartDetails?> UpdateDetailsAsync(CartDetails details);
        Task RemoveDetailsAsync(int detailsId, bool deleteHeaderIfEmpty = true);
    }
}
