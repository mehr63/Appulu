using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Settings
{
    /// <summary>
    /// Represents an admin area settings model
    /// </summary>
    public partial class AdminAreaSettingsModel : BaseAppModel, ISettingsModel
    {
        #region Properties

        public int ActiveStoreScopeConfiguration { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.GeneralCommon.AdminArea.UseRichEditorInMessageTemplates")]
        public bool UseRichEditorInMessageTemplates { get; set; }
        public bool UseRichEditorInMessageTemplates_OverrideForStore { get; set; }

        #endregion
    }
}