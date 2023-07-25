using eShop.Web.Models.Dto;
using eShop.Web.Services;
using eShop.Web.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace eShop.Web.Controllers
{
    public class CartController : Controller
    {
        readonly private ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<IActionResult> CartIndex()
        {
            CartDto cart = new();

            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            var response = await _cartService.GetCartAsync(userId);

            if (response?.IsSuccess is true)
            {
                cart = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Result));
            }
            else
            {
                cart = new CartDto();
            }

            return View(cart);
        }

        public async Task<IActionResult> RemoveDetail(int detailsId)
        {
            var response = await _cartService.RemoveCartDetailsAsync(detailsId);

            if (response?.IsSuccess is true)
            {
                TempData["success"] = "Product removed from Cart";
            } else
            {
                TempData["error"] = $"Can't remove product: {response?.Message}";
            }

            return RedirectToAction("CartIndex");
        }

        public async Task<IActionResult> ApplyCoupon(CartDto cart)
        {
            var response = await _cartService.ApplyCouponAsync(cart.CartHeader.Id, cart.CartHeader.CouponCode);

            if (response?.IsSuccess is true)
            {
                TempData["success"] = "Coupon applied";
            }
            else
            {
                TempData["error"] = $"Can't apply coupon: {response?.Message}";
            }

            return RedirectToAction("CartIndex");
        }

        public async Task<IActionResult> RemoveCoupon(int headerId)
        {
            var response = await _cartService.RemoveCouponAsync(headerId);

            if (response?.IsSuccess is true)
            {
                TempData["success"] = "Coupon removed";
            }
            else
            {
                TempData["error"] = $"Can't remove coupon: {response?.Message}";
            }

            return RedirectToAction("CartIndex");
        }

    }
}
