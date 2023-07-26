using eShop.Web.Models.Dto;
using eShop.Web.Services.IService;
using static eShop.Web.Utilities.StaticDetails;

namespace eShop.Web.Services
{
    public class CartService : ICartService
    {
        private readonly IBaseService _baseService;

        public CartService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto> ApplyCouponAsync(int cartHeadId, string couponCode)
        {
            var cart = new CartDto {
                CartHeader = new CartHeaderDto
                {
                    Id = cartHeadId,
                    CouponCode = couponCode
                }
            };

            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Url = CartApiBase + $"/api/CartAPI/ApplyCoupon",
                Data = cart
            });
        }

        public async Task<ResponseDto> RemoveCouponAsync(int cartHeadId)
        {
            var cart = new CartDto
            {
                CartHeader = new CartHeaderDto
                {
                    Id = cartHeadId
                }
            };

            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.DELETE,
                Url = CartApiBase + $"/api/CartAPI/RemoveCoupon",
                Data = cart
            });
        }

        public async Task<ResponseDto> GetCartAsync(string userId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.GET,
                Url = CartApiBase + $"/api/CartAPI/GetCart/{userId}"
            });
        }

        public async Task<ResponseDto> RemoveCartDetailsAsync(int cartDetailsId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.DELETE,
                Url = CartApiBase + $"/api/CartAPI/DetailsRemove/{cartDetailsId}"
            });
        }

        public async Task<ResponseDto> UpsertCartDetailsAsync(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Url = CartApiBase + $"/api/CartAPI/CartUpsert",
                Data = cartDto
            });
        }

        public async Task<ResponseDto> SendCartEmailAsync(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Url = CartApiBase + $"/api/CartAPI/EmailCartRequest",
                Data = cartDto
            });
        }
    }
}
