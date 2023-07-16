using eShop.Services.ShoppingCartAPI.Models.Dto;

namespace eShop.Services.ShoppingCartAPI.Services.IService
{
    public interface ICouponService
    {
        Task<CouponDto> GetCouponAsync(string code);
    }
}
