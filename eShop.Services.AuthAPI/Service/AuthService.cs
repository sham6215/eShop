using eShop.Services.AuthAPI.Data;
using eShop.Services.AuthAPI.Models;
using eShop.Services.AuthAPI.Models.Dto;
using eShop.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;

namespace eShop.Services.AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthService(AppDbContext db,
            UserManager<ApplicationUser> userMngr,
            RoleManager<IdentityRole> roleMngr)
        {
            _db = db;
            _userManager = userMngr;
            _roleManager = roleMngr;
        }

        public Task<LoginResponseDto> Login(LoginRequestDto loginRequest)
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> Register(RegisterRequestDto registerRequest)
        {
            throw new NotImplementedException();
        }
    }
}
