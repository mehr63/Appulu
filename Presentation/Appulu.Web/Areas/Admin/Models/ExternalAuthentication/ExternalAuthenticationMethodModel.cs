using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.ExternalAuthentication
{
    /// <summary>
    /// Represents an external authentication method model
    /// </summary>
    public partial class ExternalAuthenticationMethodModel : BaseAppModel, IPluginModel
    {
        #region Properties

        [AppResourceDisplayName("Admin.Configuration.ExternalAuthenticationMethods.Fields.FriendlyName")]
        public string FriendlyName { get; set; }

        [AppResourceDisplayName("Admin.Configuration.ExternalAuthenticationMethods.Fields.SystemName")]
        public string SystemName { get; set; }

        [AppResourceDisplayName("Admin.Configuration.ExternalAuthenticationMethods.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [AppResourceDisplayName("Admin.Configuration.ExternalAuthenticationMethods.Fields.IsActive")]
        public bool IsActive { get; set; }

        [AppResourceDisplayName("Admin.Configuration.ExternalAuthenticationMethods.Configure")]
        public string ConfigurationUrl { get; set; }

        public string LogoUrl { get; set; }

        #endregion
    }
}