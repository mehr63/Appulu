namespace Appulu.Services.Shipping
{
    /// <summary>
    /// Represents default values related to shipping services
    /// </summary>
    public static partial class AppShippingDefaults
    {
        #region Shipping methods

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : country identifier
        /// </remarks>
        public static string ShippingMethodsAllCacheKey => "App.shippingmethod.all-{0}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string ShippingMethodsPatternCacheKey => "App.shippingmethod.";

        #endregion

        #region Warehouses

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : warehouse ID
        /// </remarks>
        public static string WarehousesByIdCacheKey => "App.warehouse.id-{0}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string WarehousesPatternCacheKey => "App.warehouse.";

        #endregion
    }
}