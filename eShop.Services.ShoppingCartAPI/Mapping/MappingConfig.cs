using AutoMapper;

namespace eShop.Services.ShoppingCartAPI.Mapping
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps() {
            return new MapperConfiguration(
                config => {
                    config.AddProfile(typeof(ShoppingCartMappingProfile));
                }
            );
        }
    }
}
