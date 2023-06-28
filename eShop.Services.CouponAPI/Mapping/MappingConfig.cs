using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata;

namespace eShop.Services.CouponAPI.Mapping
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps() {
            return new MapperConfiguration(
                config => {
                    config.AddProfile(typeof(CouponMappingProfile));
                }
            );
        }
    }
}
