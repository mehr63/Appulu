namespace Appulu.Services.Tax
{
    /// <summary>
    /// Represents default values related to tax services
    /// </summary>
    public static partial class AppTaxDefaults
    {
        /// <summary>
        /// Gets a key for caching
        /// </summary>
        public static string TaxCategoriesAllCacheKey => "App.taxcategory.all";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : tax category ID
        /// </remarks>
        public static string TaxCategoriesByIdCacheKey => "App.taxcategory.id-{0}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string TaxCategoriesPatternCacheKey => "App.taxcategory.";
    }
}