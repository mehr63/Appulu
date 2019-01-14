namespace Appulu.Services.Logging
{
    /// <summary>
    /// Represents default values related to logging services
    /// </summary>
    public static partial class AppLoggingDefaults
    {
        /// <summary>
        /// Gets a key for caching
        /// </summary>
        public static string ActivityTypeAllCacheKey => "App.activitytype.all";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string ActivityTypePatternCacheKey => "App.activitytype.";
    }
}