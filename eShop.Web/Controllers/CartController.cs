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

		

    }
}
