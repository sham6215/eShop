namespace eShop.Services.AuthAPI.Models.Dto
{
    public class RegisterRequestDto
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
    }
}
