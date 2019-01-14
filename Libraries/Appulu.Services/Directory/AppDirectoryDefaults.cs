namespace Appulu.Services.Directory
{
    /// <summary>
    /// Represents default values related to directory services
    /// </summary>
    public static partial class AppDirectoryDefaults
    {
        #region Countries

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : language ID
        /// {1} : show hidden records?
        /// </remarks>
        public static string CountriesAllCacheKey => "App.country.all-{0}-{1}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string CountriesPatternCacheKey => "App.country.";

        #endregion

        #region Currencies

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : currency ID
        /// </remarks>
        public static string CurrenciesByIdCacheKey => "App.currency.id-{0}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// </remarks>
        public static string CurrenciesAllCacheKey => "App.currency.all-{0}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string CurrenciesPatternCacheKey => "App.currency.";

        #endregion

        #region Measures

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        public static string MeasureDimensionsAllCacheKey => "App.measuredimension.all";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : dimension ID
        /// </remarks>
        public static string MeasureDimensionsByIdCacheKey => "App.measuredimension.id-{0}";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        public static string MeasureWeightsAllCacheKey => "App.measureweight.all";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : weight ID
        /// </remarks>
        public static string MeasureWeightsByIdCacheKey => "App.measureweight.id-{0}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string MeasureDimensionsPatternCacheKey => "App.measuredimension.";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string MeasureWeightsPatternCacheKey => "App.measureweight.";

        #endregion

        #region States and provinces

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// {0} : country ID
        /// {1} : language ID
        /// {2} : show hidden records?
        /// </remarks>
        public static string StateProvincesAllCacheKey => "App.stateprovince.all-{0}-{1}-{2}";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string StateProvincesPatternCacheKey => "App.stateprovince.";

        #endregion
    }
}