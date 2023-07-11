using AutoMapper;
using Azure;
using eShop.Services.ProductAPI.Models;
using eShop.Services.ProductAPI.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;

namespace eShop.Services.ProductAPI.Mapping
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
        }
    }
}
