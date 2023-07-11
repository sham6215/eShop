using AutoMapper;
using eShop.Services.CouponAPI.Models;
using eShop.Services.CouponAPI.Models.Dto;

namespace eShop.Services.CouponAPI.Mapping
{
    public class CouponMappingProfile : Profile
    {
        public CouponMappingProfile()
        {
            CreateMap<Coupon, CouponDto>().ReverseMap();
        }
    }
}
