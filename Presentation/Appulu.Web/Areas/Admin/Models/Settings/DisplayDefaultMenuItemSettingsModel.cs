using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Settings
{
    /// <summary>
    /// Represents a display default menu item settings model
    /// </summary>
    public partial class DisplayDefaultMenuItemSettingsModel : BaseAppModel, ISettingsModel
    {
        #region Properties

        public int ActiveStoreScopeConfiguration { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.DisplayDefaultMenuItemSettings.DisplayHomePageMenuItem")]
        public bool DisplayHomePageMenuItem { get; set; }
        public bool DisplayHomePageMenuItem_OverrideForStore { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.DisplayDefaultMenuItemSettings.DisplayNewProductsMenuItem")]
        public bool DisplayNewProductsMenuItem { get; set; }
        public bool DisplayNewProductsMenuItem_OverrideForStore { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.DisplayDefaultMenuItemSettings.DisplayProductSearchMenuItem")]
        public bool DisplayProductSearchMenuItem { get; set; }
        public bool DisplayProductSearchMenuItem_OverrideForStore { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.DisplayDefaultMenuItemSettings.DisplayUserInfoMenuItem")]
        public bool DisplayUserInfoMenuItem { get; set; }
        public bool DisplayUserInfoMenuItem_OverrideForStore { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.DisplayDefaultMenuItemSettings.DisplayBlogMenuItem")]
        public bool DisplayBlogMenuItem { get; set; }
        public bool DisplayBlogMenuItem_OverrideForStore { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.DisplayDefaultMenuItemSettings.DisplayForumsMenuItem")]
        public bool DisplayForumsMenuItem { get; set; }
        public bool DisplayForumsMenuItem_OverrideForStore { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.DisplayDefaultMenuItemSettings.DisplayContactUsMenuItem")]
        public bool DisplayContactUsMenuItem { get; set; }
        public bool DisplayContactUsMenuItem_OverrideForStore { get; set; }

        #endregion
    }
}