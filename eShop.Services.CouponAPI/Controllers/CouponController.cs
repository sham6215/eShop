using AutoMapper;
using eShop.Services.CouponAPI.Data;
using eShop.Services.CouponAPI.Models;
using eShop.Services.CouponAPI.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Services.CouponAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IMapper _mapper;
        private ResponseDto _response;

        public CouponController(AppDbContext db, IMapper mapper)
        {
            _mapper = mapper;
            _db = db;
            _response = new ResponseDto();
        }

        [HttpGet]
        public object GetCoupons()
        {
            try
            {
                var list = _db.Coupons.ToList();
                _response.Result= _mapper.Map<IEnumerable<CouponDto>>(list);
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
                var coupon = _db.Coupons.FirstOrDefault(coupon => coupon.CouponId == id);
                _response.Result = _mapper.Map<CouponDto>(coupon);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet]
        [Route("GetByCode/code")]
        public object? GetCoupon(string code)
        {
            try
            {
                var coupon = _db.Coupons.FirstOrDefault(coupon => coupon.CouponCode.ToLower() == code.ToLower());
                _response.Result = _mapper.Map<CouponDto>(coupon);
                if (_response.Result == null) 
                    _response.IsSuccess= false;
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
                var coupon = _db.Coupons.FirstOrDefault(coupon => coupon.DiscountAmount == discMax);
                _response.Result = _mapper.Map<CouponDto>(coupon);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPost]
        public ResponseDto AddCoupon([FromBody] CouponDto couponDto)
        {
            try
            {
                var coupon = _mapper.Map<Coupon>(couponDto);
                _db.Coupons.Add(coupon);
                _db.SaveChanges();

                _response.Result = _mapper.Map<CouponDto>(coupon);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = (ex.InnerException ?? ex).Message;
            }
            return _response;
        }

        [HttpPut]
        public ResponseDto UpdateCoupon([FromBody] CouponDto couponDto)
        {
            try
            {
                var coupon = _mapper.Map<Coupon>(couponDto);
                _db.Coupons.Update(coupon);
                _db.SaveChanges();

                _response.Result = _mapper.Map<CouponDto>(coupon);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = (ex.InnerException ?? ex).Message;
            }
            return _response;
        }

        [HttpDelete]
        public ResponseDto DeleteCoupon(int id)
        {
            try
            {
                var coupon = _db.Coupons.First(c => c.CouponId == id);
                _db.Coupons.Remove(coupon);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = (ex.InnerException ?? ex).Message;
            }
            return _response;
        }
    }
}
