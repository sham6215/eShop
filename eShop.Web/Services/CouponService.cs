using eShop.Web.Models.Dto;
using eShop.Web.Services.IService;
using eShop.Web.Utilities;
using static eShop.Web.Utilities.StaticDetails;

namespace eShop.Web.Services
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService _baseService;
        public CouponService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> DeleteCouponAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.DELETE,
                Url = StaticDetails.CouponApiBase + $"/api/Coupon/{id}"
            });
        }

        public async Task<ResponseDto?> GetCouponAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.GET,
                Url = StaticDetails.CouponApiBase + $"/api/Coupon/{id}"
            });
        }

        public async Task<ResponseDto?> GetCouponAsync(string couponCode)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.GET,
                Url = StaticDetails.CouponApiBase + $"/api/Coupon/GetByCode/{couponCode}"
            });
        }

        public async Task<ResponseDto?> GetCouponsAsync()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.GET,
                Url = StaticDetails.CouponApiBase + $"/api/Coupon"
            });
        }

        public async Task<ResponseDto?> PostCouponAsync(CouponDto couponDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Url = StaticDetails.CouponApiBase + $"/api/Coupon",
                Data = couponDto
            });
        }

        public async Task<ResponseDto?> PutCouponAsync(CouponDto couponDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.PUT,
                Url = StaticDetails.CouponApiBase + $"/api/Coupon",
                Data = couponDto
            });
        }
    }
}
