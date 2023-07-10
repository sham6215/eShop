using eShop.Services.ProductAPI.Service.IService;
using eShop.Web.Models;
using eShop.Web.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace eShop.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productSrvice;

        public HomeController(IProductService productSrvice)
        {
            _productSrvice = productSrvice;
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