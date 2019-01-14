using Microsoft.AspNetCore.Routing;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Shipping
{
    /// <summary>
    /// Represents a pickup point provider model
    /// </summary>
    public partial class PickupPointProviderModel : BaseAppModel, IPluginModel
    {
        #region Properties

        [AppResourceDisplayName("Admin.Configuration.Shipping.PickupPointProviders.Fields.FriendlyName")]
        public string FriendlyName { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Shipping.PickupPointProviders.Fields.SystemName")]
        public string SystemName { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Shipping.PickupPointProviders.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Shipping.PickupPointProviders.Fields.IsActive")]
        public bool IsActive { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Shipping.PickupPointProviders.Fields.Logo")]
        public string LogoUrl { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Shipping.PickupPointProviders.Configure")]
        public string ConfigurationUrl { get; set; }

        #endregion
    }
}