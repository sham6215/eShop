﻿namespace eShop.Services.ShoppingCartAPI.Models.Dto
{
    public class CartDetailsDto
    {
        public int Id { get; set; }
        public int CartHeaderId { get; set; }
        public CartHeader? CartHeader { get; set; }
        public int ProductId { get; set; }
        public ProductDto? Product { get; set; }
        public int Count { get; set; }
    }
}
