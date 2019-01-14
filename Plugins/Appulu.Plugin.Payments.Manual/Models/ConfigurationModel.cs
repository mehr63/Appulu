using Microsoft.AspNetCore.Mvc.Rendering;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Plugin.Payments.Manual.Models
{
    public class ConfigurationModel : BaseAppModel
    {
        public int ActiveStoreScopeConfiguration { get; set; }

        [AppResourceDisplayName("Plugins.Payments.Manual.Fields.AdditionalFeePercentage")]
        public bool AdditionalFeePercentage { get; set; }
        public bool AdditionalFeePercentage_OverrideForStore { get; set; }

        [AppResourceDisplayName("Plugins.Payments.Manual.Fields.AdditionalFee")]
        public decimal AdditionalFee { get; set; }
        public bool AdditionalFee_OverrideForStore { get; set; }

        public int TransactModeId { get; set; }
        [AppResourceDisplayName("Plugins.Payments.Manual.Fields.TransactMode")]
        public SelectList TransactModeValues { get; set; }
        public bool TransactModeId_OverrideForStore { get; set; }
    }
}