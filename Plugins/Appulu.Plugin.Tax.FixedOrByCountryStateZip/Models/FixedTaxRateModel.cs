using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Plugin.Tax.FixedOrByCountryStateZip.Models
{
    public class FixedTaxRateModel
    {
        public int TaxCategoryId { get; set; }

        [AppResourceDisplayName("Plugins.Tax.FixedOrByCountryStateZip.Fields.TaxCategoryName")]
        public string TaxCategoryName { get; set; }

        [AppResourceDisplayName("Plugins.Tax.FixedOrByCountryStateZip.Fields.Rate")]
        public decimal Rate { get; set; }
    }
}