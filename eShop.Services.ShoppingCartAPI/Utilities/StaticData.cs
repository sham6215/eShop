namespace eShop.Services.ShoppingCartAPI.Utilities
{
    public static class StaticData
    {
        public static string ProductHttpClient { get; } = "Product";
        public static string ProductApiConfig { get; } = "ServiceUrls:ProductApiBase";
        public static string CouponHttpClient { get; } = "Coupon";
        public static string CouponApiConfig { get; } = "ServiceUrls:CouponApiBase";
    }
}
