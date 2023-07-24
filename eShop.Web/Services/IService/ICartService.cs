using eShop.Web.Models.Dto;

namespace eShop.Web.Services
{
    public interface ICartService
    {
        Task<ResponseDto> GetCartAsync(string userId);
        Task<ResponseDto> UpsertCartDetailsAsync(CartDto cartDto);
        Task<ResponseDto> RemoveCartDetailsAsync(int cartDetailsId);
        Task<ResponseDto> ApplyCouponAsync(int cartHeadId, string couponCode);
    }
}
