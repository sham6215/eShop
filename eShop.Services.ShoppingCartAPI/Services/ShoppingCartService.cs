using eShop.Services.ShoppingCartAPI.Data;
using eShop.Services.ShoppingCartAPI.Models;
using eShop.Services.ShoppingCartAPI.Service.IService;
using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;

namespace eShop.Services.ShoppingCartAPI.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly AppDbContext _db;

        public ShoppingCartService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<CartDetails?> GetCartProductDetailsAsync(int productId, int cartHeaderId)
        {
            return await _db.CartDetails.AsNoTracking()
                .FirstOrDefaultAsync(d => d.ProductId == productId && d.CartHeaderId == cartHeaderId);
        }

        public async Task<IEnumerable<CartDetails>?> GetCartDetailsAsync(int cartHeaderId)
        {
            return await _db.CartDetails.AsNoTracking()
                .Where(d => d.CartHeaderId == cartHeaderId).ToListAsync();
        }

        public async Task<CartHeader?> GetCartHeaderAsync(string? userId)
        {
            return await _db.CartHeaders.AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<CartHeader?> AddCartHeaderAsync(CartHeader cartHeader)
        {
            _db.CartHeaders.Add(cartHeader);
            await _db.SaveChangesAsync();
            _db.Entry(cartHeader).State = EntityState.Detached;
            return cartHeader;
        }

        public async Task<CartDetails?> AddCartDetailsAsync(CartDetails cartDetails)
        {
            _db.CartDetails.Add(cartDetails);
            var res = await _db.SaveChangesAsync();
            _db.Entry(cartDetails).State = EntityState.Detached;
            return cartDetails;
        }

        public async Task<CartDetails?> UpdateDetailsAsync(CartDetails details)
        {
            _db.CartDetails.Update(details);
            await _db.SaveChangesAsync();
            _db.Entry(details).State = EntityState.Detached;
            return details;
        }

        
        public async Task RemoveDetailsAsync(int detailsId, bool deleteHeaderIfEmpty = true)
        {
            CartDetails? details = _db.CartDetails.FirstOrDefault(d => d.Id == detailsId);
            CartHeader? header = null;
            int detailsCount = 0;

            if (details != null)
            {
                if (deleteHeaderIfEmpty)
                {
                    header = _db.CartHeaders.FirstOrDefault(h => h.Id == details.CartHeaderId);
                    detailsCount = _db.CartDetails.Count(d => d.CartHeaderId == header.Id);
                }
            }
            _db.CartDetails.Remove(details);
            if (detailsCount == 1)
            {
                _db.CartHeaders.Remove(header);
            }
            await _db.SaveChangesAsync();
        }

        public async Task ApplyCouponAsync(int headerId, string couponCode)
        {
            var headerDb = await _db.CartHeaders.FirstOrDefaultAsync(h => h.Id == headerId);
            if (headerDb != null) {
                headerDb.CouponCode = couponCode;
                await _db.SaveChangesAsync();
                _db.Entry(headerDb).State = EntityState.Detached;
            } else
            {
                throw new Exception($"There is no header with id {headerId}");
            }
        }

        public async Task RemoveCouponAsync(int headerId)
        {
            var headerDb = await _db.CartHeaders.FirstOrDefaultAsync(h => h.Id == headerId);
            if (headerDb != null)
            {
                headerDb.CouponCode = string.Empty;
                _db.SaveChanges();
                _db.Entry(headerDb).State = EntityState.Detached;
            }
            else
            {
                throw new Exception($"There is no header with id {headerId}");
            }
        }
    }
}
