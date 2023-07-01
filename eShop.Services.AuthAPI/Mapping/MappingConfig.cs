using AutoMapper;
using eShop.Services.Auth.Mapping;

namespace eShop.Services.AuthAPI.Mapping
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps() {
            return new MapperConfiguration(
                config => {
                    config.AddProfile(typeof(AuthMappingProfile));
                }
            );
        }
    }
}
