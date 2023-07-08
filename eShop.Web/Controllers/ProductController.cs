using eShop.Services.ProductAPI.Service.IService;
using eShop.Web.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace eShop.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _service;

        public ProductController(IProductService service)
        {
            _service = service;
        }

        public async Task<IActionResult> ProductIndex()
        {
            var products = new List<ProductDto>();
            var response = await _service.GetProductsAsync();

            if (response != null && response.IsSuccess)
            {
                products = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
            } else
            {
                TempData["error"] = $"Error: {response?.Message}";
            }
            return View(products);
        }

        public async Task<IActionResult> ProductCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductDto product)
        {
            if (ModelState.IsValid)
            {
                var response = await _service.CreateProductAsync(product);
                if (response?.Result != null && response.IsSuccess )
                {
                    var productResponse = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
                    TempData["success"] = $"Product created";
                    return RedirectToAction("ProductIndex");
                }
                TempData["error"] = response.Message;
            }

            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> ProductDelete(int id)
        {
            var productDto = new ProductDto();

            var responseProduct = await _service.GetProductAsync(id);
            if (responseProduct?.Result == null || !responseProduct.IsSuccess)
            {
                TempData["error"] = $"Error: {responseProduct.Message}";
                return View();
            }
            productDto = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(responseProduct.Result));

            return View(productDto);
        }

        [HttpPost]
        public async Task<IActionResult> ProductDelete(ProductDto productDto)
        {
            var response = await _service.DeleteProductAsync(productDto.Id);
            if (response?.IsSuccess is true)
            {
                TempData["success"] = $"Product deleted";
                return RedirectToAction("ProductIndex");
            }
            TempData["error"] = response.Message;

            return View(productDto);
        }

        [HttpGet]
        public async Task<IActionResult> ProductUpdate(int id)
        {
            var productDto = new ProductDto();

            var responseProduct = await _service.GetProductAsync(id);
            if (responseProduct?.Result == null || !responseProduct.IsSuccess)
            {
                TempData["error"] = $"Error: {responseProduct.Message}";
                return View();
            }
            productDto = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(responseProduct.Result));

            return View(productDto);
        }

        [HttpPost]
        public async Task<IActionResult> ProductUpdate(ProductDto productDto)
        {
            var response = await _service.UpdateProductAsync(productDto);
            if (response?.IsSuccess is true)
            {
                TempData["success"] = $"Product updated";
                return RedirectToAction("ProductIndex");
            }
            TempData["error"] = response.Message;

            return View(productDto);
        }
    }
}
