namespace Appulu.Services.Discounts
{
    /// <summary>
    /// Represents default values related to discounts services
    /// </summary>
    public static partial class AppDiscountDefaults
    {
        /// <summary>
        /// Gets the query parameter name to retrieve discount coupon code from URL
        /// </summary>
        public static string DiscountCouponQueryParameter => "discountcoupon";

        /// <summary>
        /// Key for discount requirement of a certain discount
        /// </summary>
        /// <remarks>
        /// {0} : discount id
        /// </remarks>
        public static string DiscountRequirementModelCacheKey => "App.discounts.requirements-{0}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string DiscountRequirementPatternCacheKey => "App.discounts.requirements";

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// {1} : coupon code
        /// {2} : discount name
        /// </remarks>
        public static string DiscountAllCacheKey => "App.discounts.all-{0}-{1}-{2}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string DiscountAllPatternCacheKey => "App.discounts.all";

        /// <summary>
        /// Key for category IDs of a discount
        /// </summary>
        /// <remarks>
        /// {0} : discount id
        /// {1} : roles of the current user
        /// {2} : current store ID
        /// </remarks>
        public static string DiscountCategoryIdsModelCacheKey => "App.discounts.categoryids-{0}-{1}-{2}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string DiscountCategoryIdsPatternCacheKey => "App.discounts.categoryids";

        /// <summary>
        /// Key for manufacturer IDs of a discount
        /// </summary>
        /// <remarks>
        /// {0} : discount id
        /// {1} : roles of the current user
        /// {2} : current store ID
        /// </remarks>
        public static string DiscountManufacturerIdsModelCacheKey => "App.discounts.manufacturerids-{0}-{1}-{2}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string DiscountManufacturerIdsPatternCacheKey => "App.discounts.manufacturerids";
    }
}