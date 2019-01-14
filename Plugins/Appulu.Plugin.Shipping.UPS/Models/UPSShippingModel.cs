using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Plugin.Shipping.UPS.Models
{
    public class UPSShippingModel : BaseAppModel
    {
        public UPSShippingModel()
        {
            CarrierServicesOffered = new List<string>();
            AvailableCarrierServices = new List<string>();
            AvailableUserClassifications = new List<SelectListItem>();
            AvailablePickupTypes = new List<SelectListItem>();
            AvailablePackagingTypes = new List<SelectListItem>();
        }
        [AppResourceDisplayName("Plugins.Shipping.UPS.Fields.Url")]
        public string Url { get; set; }

        [AppResourceDisplayName("Plugins.Shipping.UPS.Fields.AccountNumber")]
        public string AccountNumber { get; set; }

        [AppResourceDisplayName("Plugins.Shipping.UPS.Fields.AccessKey")]
        public string AccessKey { get; set; }

        [AppResourceDisplayName("Plugins.Shipping.UPS.Fields.Username")]
        public string Username { get; set; }

        [AppResourceDisplayName("Plugins.Shipping.UPS.Fields.Password")]
        public string Password { get; set; }

        [AppResourceDisplayName("Plugins.Shipping.UPS.Fields.AdditionalHandlingCharge")]
        public decimal AdditionalHandlingCharge { get; set; }

        [AppResourceDisplayName("Plugins.Shipping.UPS.Fields.InsurePackage")]
        public bool InsurePackage { get; set; }

        [AppResourceDisplayName("Plugins.Shipping.UPS.Fields.UserClassification")]
        public string UserClassification { get; set; }
        public IList<SelectListItem> AvailableUserClassifications { get; set; }

        [AppResourceDisplayName("Plugins.Shipping.UPS.Fields.PickupType")]
        public string PickupType { get; set; }
        public IList<SelectListItem> AvailablePickupTypes { get; set; }

        [AppResourceDisplayName("Plugins.Shipping.UPS.Fields.PackagingType")]
        public string PackagingType { get; set; }
        public IList<SelectListItem> AvailablePackagingTypes { get; set; }
        
        public IList<string> CarrierServicesOffered { get; set; }
        [AppResourceDisplayName("Plugins.Shipping.UPS.Fields.AvailableCarrierServices")]
        public IList<string> AvailableCarrierServices { get; set; }
        public string[] CheckedCarrierServices { get; set; }

        [AppResourceDisplayName("Plugins.Shipping.UPS.Fields.PassDimensions")]
        public bool PassDimensions { get; set; }

        [AppResourceDisplayName("Plugins.Shipping.UPS.Fields.PackingPackageVolume")]
        public int PackingPackageVolume { get; set; }

        public int PackingType { get; set; }
        [AppResourceDisplayName("Plugins.Shipping.UPS.Fields.PackingType")]
        public SelectList PackingTypeValues { get; set; }

        [AppResourceDisplayName("Plugins.Shipping.UPS.Fields.Tracing")]
        public bool Tracing { get; set; }
    }
}