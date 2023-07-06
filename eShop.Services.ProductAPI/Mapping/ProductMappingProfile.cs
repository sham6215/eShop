using AutoMapper;
using eShop.Services.ProductAPI.Models;
using eShop.Services.ProductAPI.Models.Dto;

namespace eShop.Services.ProductAPI.Mapping
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();
        }
    }
}
