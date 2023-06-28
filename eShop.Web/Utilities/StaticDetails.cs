namespace eShop.Web.Utilities
{
    public class StaticDetails
    {
        public static string CouponApiBase { get; set; }
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}
