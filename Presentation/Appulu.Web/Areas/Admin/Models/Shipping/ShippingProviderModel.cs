using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Shipping
{
    /// <summary>
    /// Represents a shipping provider model
    /// </summary>
    public partial class ShippingProviderModel : BaseAppModel, IPluginModel
    {
        #region Properties

        [AppResourceDisplayName("Admin.Configuration.Shipping.Providers.Fields.FriendlyName")]
        public string FriendlyName { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Shipping.Providers.Fields.SystemName")]
        public string SystemName { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Shipping.Providers.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Shipping.Providers.Fields.IsActive")]
        public bool IsActive { get; set; }
        
        [AppResourceDisplayName("Admin.Configuration.Shipping.Providers.Fields.Logo")]
        public string LogoUrl { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Shipping.Providers.Configure")]
        public string ConfigurationUrl { get; set; }

        #endregion
    }
}