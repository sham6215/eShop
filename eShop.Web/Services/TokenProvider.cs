using eShop.Web.Services.IService;
using eShop.Web.Utilities;
using Newtonsoft.Json.Linq;

namespace eShop.Web.Services
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public TokenProvider(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public void ClearToken()
        {
            _contextAccessor?.HttpContext?.Response.Cookies.Delete(StaticDetails.TokenCookie);
        }

        public string? GetToken()
        {
            string? token = null;
            var isTokenExists = _contextAccessor?.HttpContext?.Request.Cookies.TryGetValue(StaticDetails.TokenCookie, out token);
            return isTokenExists is true ? token : null;
        }

        public void SetToken(string token)
        {
            _contextAccessor?.HttpContext?.Response.Cookies.Append(StaticDetails.TokenCookie, token);
        }
    }
}
