using AutoMapper;
using eShop.Services.ShoppingCartAPI.Models;
using eShop.Services.ShoppingCartAPI.Models.Dto;
using eShop.Services.ShoppingCartAPI.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Services.ShoppingCartAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartAPIController : ControllerBase
    {
        private readonly IShoppingCartService _cartService;
        private readonly IMapper _mapper;
        private ResponseDto _response;

        public CartAPIController(IShoppingCartService cartService, IMapper mapper)
        {
            _cartService = cartService;
            _mapper = mapper;
            _response = new ResponseDto { IsSuccess = false };
        }

        [HttpPost("CartUpsert")]
        public async Task<ResponseDto> UpsertCart(CartDto cartDto)
        {
            try
            {

                var cartHeader = await _cartService.GetCartHeaderAsync(cartDto.CartHeader.UserId);
                if (cartHeader == null)
                {
                    // Cart doesn't exist. Create header and details.
                    var header = _mapper.Map<CartHeader>(cartDto.CartHeader);
                    await _cartService.AddCartHeaderAsync(header);
                    cartDto.CartHeader = _mapper.Map<CartHeaderDto>(header);

                    cartDto.CartDetails.First().CartHeaderId = header.Id;
                    var details = _mapper.Map<CartDetails>(cartDto.CartDetails.First());
                    await _cartService.AddCartDetailsAsync(details);
                    cartDto.CartDetails.First().Id = details.Id;
                }
                else
                {
                    var detailsDto = cartDto.CartDetails.First();
                    var details = await _cartService.GetCartDetailsAsync(
                        detailsDto.ProductId, cartHeader.Id);
                    // Header exists but details don't. Create details.
                    if (details == null)
                    {
                        detailsDto.CartHeaderId = cartHeader.Id;
                        details = _mapper.Map<CartDetails>(detailsDto);
                        await _cartService.AddCartDetailsAsync(details);
                    }
                    else
                    {
                        // Update details. Only count neet to be changed.
                        details.Count = detailsDto.Count += details.Count;
                        detailsDto.Id = details.Id;
                        await _cartService.UpdateDetailsAsync(details);
                    }
                }

                _response.Result = cartDto;
                _response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _response.Message = ex.InnerException?.Message ?? ex.Message;
                return _response;
            }
            return _response;
        }

        [HttpDelete("DetailsRemove")]
        public async Task<ResponseDto> RemoveDetails(int detailsId)
        {
            try
            {
                await _cartService.RemoveDetailsAsync(detailsId);
                _response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _response.Message = ex.InnerException?.Message ?? ex.Message;
                return _response;
            }
            return _response;
        }
    }
}
