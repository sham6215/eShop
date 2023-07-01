using eShop.Services.AuthAPI.Extensions;
using eShop.Services.AuthAPI.Models;
using eShop.Services.AuthAPI.Models.Enums;
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
                    Name = IdentityRoles.Admin.GetStringValue(), 
                    ConcurrencyStamp = "1", 
                    NormalizedName = IdentityRoles.Admin.GetStringValue().ToUpper() },
                new IdentityRole() { 
                    Name = IdentityRoles.Customer.GetStringValue(), 
                    ConcurrencyStamp = "2",
                    NormalizedName = IdentityRoles.Customer.GetStringValue().ToUpper() });
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}
