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
            var response = await _couponService.GetCouponsAsync();

            List<CouponDto>? coupons = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(response.Result));

            return View(coupons);
        }
    }
}
