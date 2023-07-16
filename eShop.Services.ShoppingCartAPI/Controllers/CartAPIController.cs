using AutoMapper;
using eShop.Services.ShoppingCartAPI.Models;
using eShop.Services.ShoppingCartAPI.Models.Dto;
using eShop.Services.ShoppingCartAPI.Service.IService;
using eShop.Services.ShoppingCartAPI.Services.IService;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Services.ShoppingCartAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartAPIController : ControllerBase
    {
        private readonly IShoppingCartService _cartService;
        private readonly IProductService _productService;
        private readonly ICouponService _couponService;
        private readonly IMapper _mapper;
        private ResponseDto _response;

        public CartAPIController(IShoppingCartService cartService, IMapper mapper, IProductService productService, ICouponService couponService)
        {
            _cartService = cartService;
            _mapper = mapper;
            _response = new ResponseDto { IsSuccess = false };
            _productService = productService;
            _couponService = couponService;
        }

        [HttpGet("GetCart/{userId}")]
        public async Task<ResponseDto> GetCart(string userId)
        {
            try
            {
                var header = await _cartService.GetCartHeaderAsync(userId);
                var cart = new CartDto
                {
                    CartHeader = _mapper.Map<CartHeaderDto>(header)
                };
                cart.CartHeader.CartTotal = 0;

                var items = await _cartService.GetCartDetailsAsync(header.Id);
                cart.CartDetails = _mapper.Map<IEnumerable<CartDetailsDto>>(items);
                var products = await _productService.GetProducts();
                foreach (var details in cart.CartDetails)
                {
                    details.Product = products.FirstOrDefault(x => x.Id == details.ProductId);
                    cart.CartHeader.CartTotal += details.Product.Price * details.Count;
                }

                // Apply coupon
                var coupon = await _couponService.GetCouponAsync(cart.CartHeader.CouponCode);
                if (coupon != null)
                {
                    if (cart.CartHeader.CartTotal >= coupon.MinAmount)
                    {
                        cart.CartHeader.CartTotal -= coupon.DiscountAmount;
                        cart.CartHeader.Discount = coupon.DiscountAmount;
                    }
                }

                _response.Result = cart;
                _response.IsSuccess = true;
                return _response;
            }
            catch (Exception ex)
            {
                _response.Message = ex.InnerException?.Message ?? ex.Message;
                return _response;
            }
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
                    var details = await _cartService.GetCartProductDetailsAsync(
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

        [HttpPost("ApplyCoupon")]
        public async Task<ResponseDto> ApplyCoupon([FromBody] CartDto cart)
        {
            try
            {
                var cartDetails = await _cartService.GetCartDetailsAsync(cart.CartHeader.Id);
                if (cartDetails == null)
                {
                    throw new Exception("Cart doesn't exist");
                }
                await _cartService.ApplyCouponAsync(cart.CartHeader.Id, cart.CartHeader.CouponCode);
                _response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _response.Message = ex.InnerException?.Message ?? ex.Message;
                return _response;
            }
            return _response;
        }

        [HttpDelete("RemoveCoupon")]
        public async Task<ResponseDto> RemoveCoupon([FromBody] CartDto cart)
        {
            try
            {
                var cartDetails = await _cartService.GetCartDetailsAsync(cart.CartHeader.Id);
                if (cartDetails == null)
                {
                    throw new Exception("Cart doesn't exist");
                }
                await _cartService.RemoveCouponAsync(cart.CartHeader.Id);
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
