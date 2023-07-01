using AutoMapper;
using eShop.Services.AuthAPI.Data;
using eShop.Services.AuthAPI.Models;
using eShop.Services.AuthAPI.Models.Dto;
using eShop.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Services.AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly ResponseDto _response = new ResponseDto();
        private readonly IAuthService _authService;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IAuthService authService,
            IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterController([FromBody] RegisterRequestDto request)
        {
            try
            {
                var userDto = await _authService.Register(request);
                if (userDto == null) {
                    throw new Exception("Wrong request");
                }
                _response.IsSuccess = true;
                _response.Result = userDto;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Result = null;
                _response.Message = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(_response);
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginController([FromBody] LoginRequestDto request)
        {
            try
            {
                 var loginResponseDto = await _authService.Login(request);
                _response.IsSuccess = true;
                _response.Result = loginResponseDto;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Result = null;
                _response.Message = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(_response);
            }
        }

        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRoleController([FromBody] AssignRoleDto request)
        {
            try
            {
                await _authService.AssignRoleAsync(request.UserEmail, request.RoleName);
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Result = null;
                _response.Message = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(_response);
            }
        }
    }
}
