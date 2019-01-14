using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Settings
{
    /// <summary>
    /// Represents a setting search model
    /// </summary>
    public partial class SettingSearchModel : BaseSearchModel
    {
        #region Properties

        [AppResourceDisplayName("Admin.Configuration.Settings.AllSettings.SearchSettingName")]
        public string SearchSettingName { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.AllSettings.SearchSettingValue")]
        public string SearchSettingValue { get; set; }

        #endregion
    }
}