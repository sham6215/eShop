using eShop.Services.ProductAPI.Service.IService;
using eShop.Web.Models;
using eShop.Web.Models.Dto;
using eShop.Web.Services;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;

namespace eShop.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productSrvice;
        private readonly ICartService _cartService;

        public HomeController(IProductService productSrvice, ICartService cartService)
        {
            _productSrvice = productSrvice;
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            var products = new List<ProductDto>();
            try
            {
                var response = await _productSrvice.GetProductsAsync();
                if (response?.IsSuccess is true) {
                    products = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
                    return View(products);
                }
                throw new Exception(response?.Message);
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.InnerException?.Message ?? ex.Message;
                return View(products);
            }
        }

        public async Task<IActionResult> ProductDetails(int id)
        {
            var product = new ProductDto();
            try
            {
                var response = await _productSrvice.GetProductAsync(id);
                if (response?.IsSuccess is true)
                {
                    product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
                    return View(product);
                }
                throw new Exception(response?.Message);
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.InnerException?.Message ?? ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [Authorize]
        [HttpPost]
        [ActionName("ProductDetails")]
        public async Task<IActionResult> ProductDetails(ProductDto productDto)
        {
            try
            {
                var cart = new CartDto {
                    CartHeader = new CartHeaderDto
                    {
                        UserId = User.Claims.Where(u => u.Type == JwtClaimTypes.Subject)?.FirstOrDefault()?.Value
                    },
                    CartDetails = new List<CartDetailsDto>
                    {
                        new CartDetailsDto
                        {
                            ProductId = productDto.Id,
                            Count = productDto.Count
                        }
                    }
                };

                var response = await _cartService.UpsertCartDetailsAsync(cart);
                
                if (response?.IsSuccess is true)
                {
                    TempData["success"] = $"Product '{productDto.Name}' added to the Cart";
                    return RedirectToAction(nameof(Index));
                }

                throw new Exception(response?.Message);
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.InnerException?.Message ?? ex.Message;
                return RedirectToAction(nameof(ProductDetails));
            }
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}