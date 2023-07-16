using AutoMapper;
using eShop.Services.ShoppingCartAPI.Models.Dto;
using eShop.Services.ShoppingCartAPI.Services.IService;
using eShop.Services.ShoppingCartAPI.Utilities;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace eShop.Services.ShoppingCartAPI.Services
{
    public class CouponService : ICouponService
    {
        IHttpClientFactory _httpClientFactory;

        public CouponService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<CouponDto> GetCouponAsync(string code)
        {
            var client = _httpClientFactory.CreateClient(StaticData.CouponHttpClient);
            var message = await client.GetAsync($"/api/Coupon/GetByCode/{code}");
            var content = await message.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<ResponseDto>(content);
            if (response?.IsSuccess is true)
            {
                return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(response.Result));
            }
            return null;
        }
    }
}
