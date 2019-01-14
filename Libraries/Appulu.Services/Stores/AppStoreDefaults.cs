namespace Appulu.Services.Stores
{
    /// <summary>
    /// Represents default values related to stores services
    /// </summary>
    public static partial class AppStoreDefaults
    {
        #region Store mappings

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : entity ID
        /// {1} : entity name
        /// </remarks>
        public static string StoreMappingByEntityIdNameCacheKey => "App.storemapping.entityid-name-{0}-{1}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string StoreMappingPatternCacheKey => "App.storemapping.";

        #endregion

        #region Stores

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        public static string StoresAllCacheKey => "App.stores.all";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : store ID
        /// </remarks>
        public static string StoresByIdCacheKey => "App.stores.id-{0}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string StoresPatternCacheKey => "App.stores.";

        #endregion
    }
}