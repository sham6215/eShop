using eShop.Services.ShoppingCartAPI.Models;

namespace eShop.Services.ShoppingCartAPI.Service.IService
{
    public interface IShoppingCartService
    {
        Task<CartDetails?> GetCartProductDetailsAsync(int productId, int cartHeaderId);
        public Task<IEnumerable<CartDetails>?> GetCartDetailsAsync(int cartHeaderId);
        Task<CartHeader?> GetCartHeaderAsync(string? userId);
        Task<CartHeader?> AddCartHeaderAsync(CartHeader cartHeader);
        Task<CartDetails?> AddCartDetailsAsync(CartDetails cartDetails);
        Task<CartDetails?> UpdateDetailsAsync(CartDetails details);
        Task ApplyCouponAsync(int headerId, string couponCode);
        Task RemoveCouponAsync(int headerId);
        Task RemoveDetailsAsync(int detailsId, bool deleteHeaderIfEmpty = true);
    }
}
