using AutoMapper;
using eShop.Services.ShoppingCartAPI.Models.Dto;
using eShop.Services.ShoppingCartAPI.Services.IService;
using eShop.Services.ShoppingCartAPI.Utilities;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace eShop.Services.ShoppingCartAPI.Services
{
    public class ProductService : IProductService
    {
        IHttpClientFactory _httpClientFactory;
        IMapper _mapper;

        public ProductService(IHttpClientFactory httpClientFactory, IMapper mapper)
        {
            _httpClientFactory = httpClientFactory;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto> > GetProducts()
        {
            var client = _httpClientFactory.CreateClient(StaticData.ProductHttpClient);
            var message = await client.GetAsync("/api/Product");
            var content = await message.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<ResponseDto>(content);
            if (!response.IsSuccess)
            {
                throw new Exception(response.Message);
            }
            return JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(Convert.ToString(response.Result));
        }
    }
}
