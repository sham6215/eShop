using eShop.Web.Models.Dto;
using eShop.Web.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace eShop.Web.Controllers
{
    public class CouponController : Controller
    {
        ICouponService _couponService;
        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDto> coupons = new();
            var response = await _couponService.GetCouponsAsync();

            if (response.IsSuccess)
            {
                coupons = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(response.Result));
            } 
            else
            {
                TempData["error"] = response.Message;
            }

            return View(coupons);
        }

		public async Task<IActionResult> CouponCreate()
		{
			return View();
		}

        [HttpPost]
        public async Task<IActionResult> CouponCreate(CouponDto model)
        {
            if (ModelState.IsValid)
            {
                var response = await _couponService.PostCouponAsync(model);
                if (response?.Result != null && response.IsSuccess)
                {
                    TempData["success"] = $"Coupon created successfully";
                    return RedirectToAction(nameof(CouponIndex));
                }
                else
                {
                    TempData["error"] = response.Message;
                }
            }

            return View(model);
        }

        public async Task<IActionResult> CouponDelete(int couponId)
        {
            var response = await _couponService.GetCouponAsync(couponId);
            if (response != null && response.IsSuccess)
            {
                var coupon = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(response.Result));
                return View(coupon);
            }
            else
            {
                TempData["error"] = response.Message;
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> CouponDelete(CouponDto couponDto)
        {
            var response = await _couponService.DeleteCouponAsync(couponDto.CouponId);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = $"Coupon deleted successfully";
                return RedirectToAction(nameof(CouponIndex));
            }
            else
            {
                TempData["error"] = response.Message;
            }

            return View(couponDto);
        }

    }
}
