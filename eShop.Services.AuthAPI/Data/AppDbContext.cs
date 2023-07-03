using eShop.Services.AuthAPI.Extensions;
using eShop.Services.AuthAPI.Models;
using eShop.Services.AuthAPI.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace eShop.Services.AuthAPI.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base (options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            SeedRoles(modelBuilder);
        }

        private void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole() { 
                    Name = StaticRoles.Admin,
                    ConcurrencyStamp = "1", 
                    NormalizedName = StaticRoles.Admin.ToUpper() },
                new IdentityRole() { 
                    Name = StaticRoles.Customer, 
                    ConcurrencyStamp = "2",
                    NormalizedName = StaticRoles.Customer.ToUpper() });
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}
