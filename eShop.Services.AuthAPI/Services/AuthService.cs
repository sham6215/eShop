using AutoMapper;
using Azure.Core;
using eShop.Services.AuthAPI.Data;
using eShop.Services.AuthAPI.Models;
using eShop.Services.AuthAPI.Models.Dto;
using eShop.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;
using System;

namespace eShop.Services.AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IMapper _mapper;

        public AuthService(AppDbContext db,
            UserManager<ApplicationUser> userMngr,
            RoleManager<IdentityRole> roleMngr,
            IJwtTokenGenerator jwtTokenGenerator,
            IMapper mapper)
        {
            _db = db;
            _userManager = userMngr;
            _roleManager = roleMngr;
            _jwtTokenGenerator = jwtTokenGenerator;
            _mapper = mapper;
        }

        public async Task AssignRoleAsync(string email, string role)
        {
            try
            {
                var user = _db.Users.FirstOrDefault(u => u.Email == email);
                if (user == null) {
                    throw new Exception("User with such email doesn't exist");
                }
                var roleExists = await _roleManager.RoleExistsAsync(role);
                if (!roleExists)
                {
                    throw new Exception("Such role doen't exist");
                }
                var result = await _userManager.AddToRoleAsync(user, role);
                if (!result.Succeeded)
                {
                    var errorMessage = string.Join(
                    Environment.NewLine,
                    result.Errors.Select(e => $"Error | Code: {e.Code} | Description: {e.Description}"));
                    throw new Exception(errorMessage);
                }
            }
            catch (Exception) {
                throw;
            }
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequest)
        {
            try
            {
                ApplicationUser user = _db.Users.FirstOrDefault(u => u.UserName == loginRequest.UserName);
                if (user != null)
                {
                    var isValid = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
                    if (isValid)
                    {
                        var roles = await _userManager.GetRolesAsync(user);

                        return new LoginResponseDto { 
                            User = _mapper.Map<ApplicationUser, UserDto>(user), 
                            Token = _jwtTokenGenerator.GenerateToken(user, roles)
                        };
                    }
                }
                throw new Exception("Wrong user name or password");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserDto> Register(RegisterRequestDto request)
        {
            ApplicationUser user = new ApplicationUser()
            {
                Email = request.Email,
                Name = request.Name,
                UserName = request.UserName,
                PhoneNumber = request.PhoneNumber
            };

            try
            {
                IdentityResult result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    var newUser = _db.Users.First(u => u.UserName == user.UserName);
                    var userDto = _mapper.Map<ApplicationUser, UserDto>(newUser);
                    return userDto;
                }
                var errorMessage = string.Join(
                    Environment.NewLine,
                    result.Errors.Select(e => $"Error | Code: {e.Code} | Description: {e.Description}"));

                throw new Exception(errorMessage);
                
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
