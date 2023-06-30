namespace eShop.Services.AuthAPI.Models
{
    public class JwtOptions
    {
        public string Secret { get; set; }
        public string Email { get; set; }
        public string Audience { get; set; }
    }
}
