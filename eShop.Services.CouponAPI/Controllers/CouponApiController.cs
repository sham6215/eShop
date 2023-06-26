using eShop.Services.CouponAPI.Data;
using eShop.Services.CouponAPI.Models;
using eShop.Services.CouponAPI.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Services.CouponAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponApiController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ResponseDto _response;

        public CouponApiController(AppDbContext db)
        {
            _db = db;
            _response = new ResponseDto();
        }

        [HttpGet]
        public object GetCoupons()
        {
            try
            {
                _response.Result= _db.Coupons.ToList();
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet]
        [Route("{id:int}")]
        public object? GetCoupon(int id)
        {
            try
            {
                _response.Result = _db.Coupons.FirstOrDefault(coupon => coupon.CouponId == id);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet]
        [Route("best")]
        public object? GetBestCoupon()
        {
            try
            {
                double? discMax = _db.Coupons.Select(c => c.DiscountAmount).Max();
                _response.Result = _db.Coupons.FirstOrDefault(coupon => coupon.DiscountAmount == discMax);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
    }
}
