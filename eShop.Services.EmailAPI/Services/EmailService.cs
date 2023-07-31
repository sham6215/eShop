using eShop.Services.EmailAPI.Data;
using eShop.Services.EmailAPI.Models;
using eShop.Services.EmailAPI.Models.Dto;
using eShop.Services.EmailAPI.Services.IService;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace eShop.Services.EmailAPI.Services
{
    public class EmailService : IEmailService
    {
        private readonly DbContextOptions<AppDbContext> _dbOptions;
        private readonly IConfiguration _config;
        
        private string _adminEmail { get; set; }
        

        public EmailService(DbContextOptions<AppDbContext> options, IConfiguration config)
        {
            _dbOptions = options;
            _config = config;
            _adminEmail = _config.GetValue<string>("AdminEmail");
        }

        public async Task EmailCartAndLog(CartDto cartDto)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<br/>Cart Email requested ");
            sb.AppendLine("<br/>Total " + cartDto.CartHeader.CartTotal);
            sb.AppendLine("<br/>");
            sb.AppendLine("<ul>");

            foreach(var item in cartDto.CartDetails)
            {
                sb.AppendLine("<li>");
                sb.AppendLine(item.Product.Name + " x " + item.Count);
                sb.AppendLine("</li>");
            }
            sb.AppendLine("</ul>");

            await LogAndEmail(sb.ToString(), cartDto.CartHeader.Email);
        }

        public async Task EmailRegisterUserAndLog(UserDto userDto)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<br/>Regiser User Email requested ");
            sb.AppendLine("<br/>Email: " + userDto.Email);
            sb.AppendLine("<br/>Name: " + userDto.Name);
            sb.AppendLine("<br/>User Name: " + userDto.UserName);
            sb.AppendLine("<br/>Phone: " + userDto.PhoneNumber);
            sb.AppendLine("<br/>");
            
            await LogAndEmail(sb.ToString(), _adminEmail);
        }

        private async Task<bool> LogAndEmail(string message, string email)
        {
            await using var db = new AppDbContext(_dbOptions);
            var emailLogger = new EmailLogger
            {
                Email = email,
                MessageSent = DateTime.Now,
                Message = message
            };
            try
            {
                await db.EmailLoggers.AddAsync(emailLogger);
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
