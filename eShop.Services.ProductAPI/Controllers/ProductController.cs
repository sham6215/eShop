using AutoMapper;
using eShop.Services.ProductAPI.Models;
using eShop.Services.ProductAPI.Models.Dto;
using eShop.Services.ProductAPI.Service.IService;
using eShop.Services.ProductAPI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace eShop.Services.ProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
                return BadRequest(_response);
            }
            return Ok(_response);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult> GetProduct(int id)
        {
            try
            {
                var product = await _productService.GetProductAsync(id);
                _response.Result = _mapper.Map<ProductDto>(product);
                _response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _response.Message = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(_response);
            }
            return Ok(_response);
        }

        [HttpPost]
        [Authorize(Roles = StaticRoles.Admin)]
        public async Task<ActionResult> CreateProduct([FromBody] ProductDto productDto)
        {
            try
            {
                var product = _mapper.Map<Product>(productDto);
                await _productService.CreateProductAsync(product);
                _response.Result = _mapper.Map<ProductDto>(product);
                _response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _response.Message = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(_response);
            }
            return Ok(_response);
        }

        [HttpPut]
        [Authorize(Roles = StaticRoles.Admin)]
        public async Task<ActionResult> UpdateProduct([FromBody] ProductDto productDto)
        {
            try
            {
                var product = _mapper.Map<Product>(productDto);
                await _productService.UpdateProductAsync(product);
                _response.Result = _mapper.Map<ProductDto>(product);
                _response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _response.Message = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(_response);
            }
            return Ok(_response);
        }

        [HttpPatch]
        [Route("{id:int}")]
        [Authorize(Roles = StaticRoles.Admin)]
        public async Task<ActionResult> PatchProduct([FromRoute, Required]int id, [FromBody] JsonPatchDocument<ProductDto> productPatchDto)
        {
            /*try
            {
                var patchProduct = _mapper.Map<JsonPatchDocument<Product> >(productPatchDto);
                var product = await _productService.PatchProductAsync(id, patchProduct);
                _response.Result = _mapper.Map<ProductDto>(product);
                _response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _response.Message = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(_response);
            }
            return Ok(_response);*/
            throw new NotImplementedException();
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = StaticRoles.Admin)]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            try
            {
                await _productService.DeleteProductAsync(id);
                _response.Result = null;
                _response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _response.Message = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(_response);
            }
            return Ok(_response);
        }
    }
}
