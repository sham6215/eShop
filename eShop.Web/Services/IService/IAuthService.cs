using eShop.Models.Dto;
using eShop.Web.Models.Dto;

namespace eShop.Web.Services.IService
{
    public interface IAuthService
    {
        Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequest);
        Task<ResponseDto?> RegisterAsync(RegisterRequestDto registerRequest);
        Task<ResponseDto?> AssignRoleAsync(AssignRoleDto assignRoleRequest);
    }
}
