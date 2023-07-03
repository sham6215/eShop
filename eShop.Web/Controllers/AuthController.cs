using eShop.Models.Dto;
using eShop.Web.Models.Dto;
using eShop.Web.Services.IService;
using eShop.Web.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace eShop.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequestDto registerRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var regResponse = await _authService.RegisterAsync(registerRequest);
                    if (regResponse == null || !regResponse.IsSuccess)
                    {
                        throw new Exception($"Can't register user: {regResponse.Message}");
                    }

                    var user = JsonConvert.DeserializeObject<UserDto>(Convert.ToString(regResponse.Result));

                    var roleRequest = new AssignRoleDto()
                    {

                        RoleName = StaticRoles.Customer,
                        UserEmail = user.Email
                    };
                    var roleResponse = await _authService.AssignRoleAsync(roleRequest);
                    if (roleResponse == null || !roleResponse.IsSuccess)
                    {
                        throw new Exception($"Can't register user: {roleResponse.Message}");
                    }
                    TempData["success"] = $"User '{user.UserName}' created";
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.InnerException?.Message ?? ex.Message;
            }
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto loginRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var loginResponse = await _authService.LoginAsync(loginRequest);
                    if (loginResponse == null || !loginResponse.IsSuccess)
                    {
                        throw new Exception(loginResponse.Message);
                    }

                    var loginDto = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(loginResponse.Result));

                    return RedirectToAction("Index", "Home");

                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("LoginError", ex.InnerException?.Message ?? ex.Message);
            }
            return View(loginRequest);
        }

        public IActionResult Logout()
        {
            return View();
        }

            public IActionResult AssignRole()
        {
            return View();
        }
    }
}
