using Microsoft.AspNetCore.Mvc.Rendering;
using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Settings
{
    /// <summary>
    /// Represents a full-text settings model
    /// </summary>
    public partial class FullTextSettingsModel : BaseAppModel, ISettingsModel
    {
        #region Properties

        public int ActiveStoreScopeConfiguration { get; set; }

        public bool Supported { get; set; }

        public bool Enabled { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.FullTextSettings.SearchMode")]
        public int SearchMode { get; set; }
        public SelectList SearchModeValues { get; set; }

        #endregion
    }
}