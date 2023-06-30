using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Services.AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IdentityDbContext _db;

        public AuthController(IdentityDbContext db)
        {
            _db = db;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterController()
        {
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginController()
        {
            return Ok();
        }
    }
}
