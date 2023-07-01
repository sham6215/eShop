using AutoMapper;
using eShop.Services.AuthAPI.Models;
using eShop.Services.AuthAPI.Models.Dto;

namespace eShop.Services.Auth.Mapping
{
    public class AuthMappingProfile : Profile
    {
        public AuthMappingProfile()
        {
            CreateMap<ApplicationUser, UserDto>();
            CreateMap<UserDto, ApplicationUser>();
        }
    }
}
