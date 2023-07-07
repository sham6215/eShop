using eShop.Services.ProductAPI.Data;
using eShop.Services.ProductAPI.Models;
using eShop.Services.ProductAPI.Models.Dto;
using eShop.Services.ProductAPI.Service.IService;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace eShop.Services.ProductAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _db;

        public ProductService(AppDbContext context)
        {
            _db = context;
        }

        public async Task<Product?> CreateProductAsync(Product product)
        {
            await _db.Products.AddAsync(product);
            await _db.SaveChangesAsync();
            _db.Entry(product).State = EntityState.Detached;
            
            return product;
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = new Product { Id = id };
            _db.Products.Attach(product);
            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _db.Products.AsNoTracking().ToListAsync();
        }

        public async Task<Product?> GetProductAsync(int productId)
        {
            return await _db.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == productId);
        }

        public async Task<Product?> UpdateProductAsync(Product product)
        {
            _db.Attach(product);
            _db.Entry(product).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            _db.Entry(product).State = EntityState.Detached;
            return product;
        }

        public async Task<Product?> PatchProductAsync(int id, JsonPatchDocument<Product> patchProduct)
        {
            var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return null;
            }
            patchProduct.ApplyTo(product);
            // Save the updated product in the database
            _db.Update(product);
            await _db.SaveChangesAsync();
            _db.Entry(product).State = EntityState.Detached;
            return product;
        }
    }
}
