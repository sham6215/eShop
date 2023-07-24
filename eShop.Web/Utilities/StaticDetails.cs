namespace eShop.Web.Utilities
{
    public class StaticDetails
    {
        public static string CouponApiBase { get; set; }
        public static string AuthApiBase { get; set; }
        public static string ProductApiBase { get; set; }
        public static string CartApiBase { get; set; }
        public static string TokenCookie { get; } = "JWToken";

        public static string HeaderAuthorization { get; } = "Authorization";

        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }


    }
}
