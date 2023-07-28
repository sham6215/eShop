using eShop.Services.EmailAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace eShop.Services.EmailAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base (options)
        {
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //}

        public DbSet<EmailLogger> EmailLoggers { get; set; }
    }
}
