using AutoMapper;
using eShop.Services.ProductAPI.Data;
using eShop.Services.ProductAPI.Models.Dto;
using eShop.Services.ProductAPI.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Services.ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private ResponseDto _response;

        public ProductController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;

            _response = new ResponseDto {
                IsSuccess = false,
                Message = string.Empty,
                Result = null
            };
        }

        [HttpGet]
        public async Task<ActionResult> GetProducts()
        {
            try
            {
                var products = await _productService.GetProductsAsync();
                _response.Result = _mapper.Map<List<ProductDto>>(products.ToList());
                _response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _response.Message = ex.InnerException?.Message ?? ex.Message;
                throw;
            }
            return Ok(_response);
        }
    }
}
