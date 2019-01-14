using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Settings
{
    /// <summary>
    /// Represents a security settings model
    /// </summary>
    public partial class SecuritySettingsModel : BaseAppModel, ISettingsModel
    {
        #region Properties

        public int ActiveStoreScopeConfiguration { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.EncryptionKey")]
        public string EncryptionKey { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.AdminAreaAllowedIpAddresses")]
        public string AdminAreaAllowedIpAddresses { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.ForceSslForAllPages")]
        public bool ForceSslForAllPages { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.EnableXSRFProtectionForAdminArea")]
        public bool EnableXsrfProtectionForAdminArea { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.EnableXSRFProtectionForPublicStore")]
        public bool EnableXsrfProtectionForPublicStore { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.HoneypotEnabled")]
        public bool HoneypotEnabled { get; set; }

        #endregion
    }
}