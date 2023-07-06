using eShop.Models.Dto;
using eShop.Web.Models.Dto;
using eShop.Web.Services.IService;
using eShop.Web.Utilities;
using static eShop.Web.Utilities.StaticDetails;

namespace eShop.Web.Services
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;

        public AuthService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequest)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Url = AuthApiBase + "/api/auth/Login",
                Data = loginRequest
            }, false);
        }

        public async Task<ResponseDto?> RegisterAsync(RegisterRequestDto registerRequest)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Url = AuthApiBase + "/api/auth/Register",
                Data = registerRequest
            }, false);
        }

        public async Task<ResponseDto?> AssignRoleAsync(AssignRoleDto assignRoleRequest)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Url = AuthApiBase + "/api/auth/AssignRole",
                Data = assignRoleRequest
            });
        }
    }
}
