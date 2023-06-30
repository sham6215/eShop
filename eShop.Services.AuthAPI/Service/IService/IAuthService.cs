using eShop.Services.AuthAPI.Models.Dto;

namespace eShop.Services.AuthAPI.Service.IService
{
    public interface IAuthService
    {
        Task<LoginResponseDto> Login(LoginRequestDto loginRequest);
        Task<UserDto> Register(RegisterRequestDto registerRequest);
    }
}
