using eShop.Models.Dto;
using eShop.Web.Models.Dto;
using eShop.Web.Services.IService;
using eShop.Web.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;

namespace eShop.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;

        public AuthController(IAuthService authService, ITokenProvider tokenProvider)
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
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

                    if (loginDto?.Token != null)
                    _tokenProvider.SetToken(loginDto.Token);

                    await SignInAsync(loginDto);

                    return await RedirectHome();

                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("LoginError", ex.InnerException?.Message ?? ex.Message);
            }
            return View(loginRequest);
        }

        private async Task SignInAsync(LoginResponseDto loginDto)
        {
                var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(loginDto.Token);
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
                jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
                jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
                jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub).Value));

            identity.AddClaim(new Claim(ClaimTypes.Name,
                jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email).Value));

            var roles = jwt.Claims
                .Where(c => c.Type == "role")
                .Select(c => new Claim(ClaimTypes.Role, c.Value));
            
            identity.AddClaims(roles);


            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        private async Task<IActionResult> RedirectHome()
        {
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
            return await RedirectHome();
        }

        public IActionResult AssignRole()
        {
            return View();
        }
    }
}
