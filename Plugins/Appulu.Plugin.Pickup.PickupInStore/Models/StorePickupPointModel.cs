using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Plugin.Pickup.PickupInStore.Models
{
    public class StorePickupPointModel : BaseAppEntityModel
    {
        public StorePickupPointModel()
        {
            this.Address = new AddressModel();
            AvailableStores = new List<SelectListItem>();
        }

        public AddressModel Address { get; set; }

        [AppResourceDisplayName("Plugins.Pickup.PickupInStore.Fields.Name")]
        public string Name { get; set; }

        [AppResourceDisplayName("Plugins.Pickup.PickupInStore.Fields.Description")]
        public string Description { get; set; }

        [AppResourceDisplayName("Plugins.Pickup.PickupInStore.Fields.PickupFee")]
        public decimal PickupFee { get; set; }

        [AppResourceDisplayName("Plugins.Pickup.PickupInStore.Fields.OpeningHours")]
        public string OpeningHours { get; set; }

        [AppResourceDisplayName("Plugins.Pickup.PickupInStore.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        public List<SelectListItem> AvailableStores { get; set; }
        [AppResourceDisplayName("Plugins.Pickup.PickupInStore.Fields.Store")]
        public int StoreId { get; set; }
        public string StoreName { get; set; }
    }

    public class AddressModel
    {
        public AddressModel()
        {
            AvailableCountries = new List<SelectListItem>();
            AvailableStates = new List<SelectListItem>();
        }

        [AppResourceDisplayName("Admin.Address.Fields.Country")]
        public int? CountryId { get; set; }
        public IList<SelectListItem> AvailableCountries { get; set; }

        [AppResourceDisplayName("Admin.Address.Fields.StateProvince")]
        public int? StateProvinceId { get; set; }
        public IList<SelectListItem> AvailableStates { get; set; }

        [AppResourceDisplayName("Admin.Address.Fields.County")]
        public string County { get; set; }

        [AppResourceDisplayName("Admin.Address.Fields.City")]
        public string City { get; set; }

        [AppResourceDisplayName("Admin.Address.Fields.Address1")]
        public string Address1 { get; set; }

        [AppResourceDisplayName("Admin.Address.Fields.ZipPostalCode")]
        public string ZipPostalCode { get; set; }
    }
}