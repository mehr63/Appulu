using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Settings
{
    /// <summary>
    /// Represents a localization settings model
    /// </summary>
    public partial class LocalizationSettingsModel : BaseAppModel, ISettingsModel
    {
        #region Properties

        public int ActiveStoreScopeConfiguration { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.UseImagesForLanguageSelection")]
        public bool UseImagesForLanguageSelection { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.SeoFriendlyUrlsForLanguagesEnabled")]
        public bool SeoFriendlyUrlsForLanguagesEnabled { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.AutomaticallyDetectLanguage")]
        public bool AutomaticallyDetectLanguage { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.LoadAllLocaleRecordsOnStartup")]
        public bool LoadAllLocaleRecordsOnStartup { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.LoadAllLocalizedPropertiesOnStartup")]
        public bool LoadAllLocalizedPropertiesOnStartup { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.LoadAllUrlRecordsOnStartup")]
        public bool LoadAllUrlRecordsOnStartup { get; set; }

        #endregion
    }
}