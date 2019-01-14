namespace Appulu.Services.Orders
{
    /// <summary>
    /// Represents default values related to orders services
    /// </summary>
    public static partial class AppOrderDefaults
    {
        #region Checkout attributes

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : store ID
        /// {1} : >A value indicating whether we should exclude shippable attributes
        /// </remarks>
        public static string CheckoutAttributesAllCacheKey => "App.checkoutattribute.all-{0}-{1}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : checkout attribute ID
        /// </remarks>
        public static string CheckoutAttributesByIdCacheKey => "App.checkoutattribute.id-{0}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : checkout attribute ID
        /// </remarks>
        public static string CheckoutAttributeValuesAllCacheKey => "App.checkoutattributevalue.all-{0}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : checkout attribute value ID
        /// </remarks>
        public static string CheckoutAttributeValuesByIdCacheKey => "App.checkoutattributevalue.id-{0}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string CheckoutAttributesPatternCacheKey => "App.checkoutattribute.";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string CheckoutAttributeValuesPatternCacheKey => "App.checkoutattributevalue.";

        #endregion
    }
}