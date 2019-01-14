using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Settings
{
    /// <summary>
    /// Represents an external authentication settings model
    /// </summary>
    public partial class ExternalAuthenticationSettingsModel : BaseAppModel, ISettingsModel
    {
        #region Properties

        public int ActiveStoreScopeConfiguration { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.AllowUsersToRemoveAssociations")]
        public bool AllowUsersToRemoveAssociations { get; set; }

        #endregion
    }
}