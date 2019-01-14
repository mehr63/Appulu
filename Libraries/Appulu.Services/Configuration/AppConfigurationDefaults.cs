namespace Appulu.Services.Configuration
{
    /// <summary>
    /// Represents default values related to configuration services
    /// </summary>
    public static partial class AppConfigurationDefaults
    {
        /// <summary>
        /// Gets a key for caching
        /// </summary>
        public static string SettingsAllCacheKey => "App.setting.all";

        /// <summary>
        /// Gets a key pattern to clear cache
        /// </summary>
        public static string SettingsPatternCacheKey => "App.setting.";
    }
}