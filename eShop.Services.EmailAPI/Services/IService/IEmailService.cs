using eShop.Services.EmailAPI.Models.Dto;

namespace eShop.Services.EmailAPI.Services.IService
{
    public interface IEmailService
    {
        Task EmailCartAndLog(CartDto cartDto);
    }
}
