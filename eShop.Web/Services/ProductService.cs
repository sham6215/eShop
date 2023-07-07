using eShop.Services.ProductAPI.Service.IService;
using eShop.Web.Models.Dto;
using eShop.Web.Services.IService;
using eShop.Web.Utilities;
using System;
using static eShop.Web.Utilities.StaticDetails;

namespace eShop.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly IBaseService _service;

        public ProductService(IBaseService service)
        {
            _service = service;
        }

        public async Task<ResponseDto?> CreateProductAsync(ProductDto product)
        {
            var request = new RequestDto
            {
                Data = product,
                ApiType = ApiType.POST,
                Url = ProductApiBase + $"/api/Product"
            };
            return await _service.SendAsync(request);
        }

        public async Task<ResponseDto?> DeleteProductAsync(int id)
        {
            var request = new RequestDto
            {
                ApiType = ApiType.DELETE,
                Url = ProductApiBase + $"/api/Product/{id}"
            };
            return await _service.SendAsync(request);
        }

        public async Task<ResponseDto?> GetProductAsync(int id)
        {
            var request = new RequestDto
            {
                ApiType = ApiType.GET,
                Url = ProductApiBase + $"/api/Product/{id}"
            };
            return await _service.SendAsync(request);
        }

        public async Task<ResponseDto?> GetProductsAsync()
        {
            var request = new RequestDto
            {
                ApiType = ApiType.GET,
                Url = ProductApiBase + $"/api/Product"
            };
            return await _service.SendAsync(request);
        }

        public async Task<ResponseDto?> UpdateProductAsync(ResponseDto product)
        {
            var request = new RequestDto
            {
                Data = product,
                ApiType = ApiType.PUT,
                Url = ProductApiBase + $"/api/Product"
            };
            return await _service.SendAsync(request);
        }
    }
}
