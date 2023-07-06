using eShop.Web.Models.Dto;

namespace eShop.Web.Services.IService
{
    public interface IBaseService
    {
        Task<ResponseDto?> SendAsync(RequestDto request, bool useToken = true);
    }
}
