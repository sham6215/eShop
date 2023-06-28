using eShop.Web.Models.Dto;

namespace eShop.Web.Services.IService
{
    public interface ICouponService
    {
        Task<ResponseDto?> GetCouponsAsync();
        Task<ResponseDto?> GetCouponAsync(int id);
        Task<ResponseDto?> GetCouponAsync(string couponCode);
        Task<ResponseDto?> PostCouponAsync(CouponDto couponDto);
        Task<ResponseDto?> PutCouponAsync(CouponDto couponDto);
        Task<ResponseDto?> DeleteCouponAsync(int id);
    }
}
