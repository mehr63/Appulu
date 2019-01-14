using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Plugin.Tax.FixedOrByCountryStateZip.Models
{
    public class CountryStateZipModel : BaseAppEntityModel
    {
        [AppResourceDisplayName("Plugins.Tax.FixedOrByCountryStateZip.Fields.Store")]
        public int StoreId { get; set; }
        [AppResourceDisplayName("Plugins.Tax.FixedOrByCountryStateZip.Fields.Store")]
        public string StoreName { get; set; }

        [AppResourceDisplayName("Plugins.Tax.FixedOrByCountryStateZip.Fields.TaxCategory")]
        public int TaxCategoryId { get; set; }
        [AppResourceDisplayName("Plugins.Tax.FixedOrByCountryStateZip.Fields.TaxCategory")]
        public string TaxCategoryName { get; set; }

        [AppResourceDisplayName("Plugins.Tax.FixedOrByCountryStateZip.Fields.Country")]
        public int CountryId { get; set; }
        [AppResourceDisplayName("Plugins.Tax.FixedOrByCountryStateZip.Fields.Country")]
        public string CountryName { get; set; }

        [AppResourceDisplayName("Plugins.Tax.FixedOrByCountryStateZip.Fields.StateProvince")]
        public int StateProvinceId { get; set; }
        [AppResourceDisplayName("Plugins.Tax.FixedOrByCountryStateZip.Fields.StateProvince")]
        public string StateProvinceName { get; set; }

        [AppResourceDisplayName("Plugins.Tax.FixedOrByCountryStateZip.Fields.Zip")]
        public string Zip { get; set; }

        [AppResourceDisplayName("Plugins.Tax.FixedOrByCountryStateZip.Fields.Percentage")]
        public decimal Percentage { get; set; }
    }
}