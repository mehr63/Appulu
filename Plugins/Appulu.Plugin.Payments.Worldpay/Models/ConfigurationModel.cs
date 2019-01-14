using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Plugin.Payments.Worldpay.Models
{
    /// <summary>
    /// Represents the Worldpay configuration model
    /// </summary>
    public class ConfigurationModel : BaseAppModel
    {
        [AppResourceDisplayName("Plugins.Payments.Worldpay.Fields.SecureNetId")]
        public string SecureNetId { get; set; }

        [AppResourceDisplayName("Plugins.Payments.Worldpay.Fields.SecureKey")]
        [DataType(DataType.Password)]
        [NoTrim]
        public string SecureKey { get; set; }

        [AppResourceDisplayName("Plugins.Payments.Worldpay.Fields.PublicKey")]
        public string PublicKey { get; set; }

        [AppResourceDisplayName("Plugins.Payments.Worldpay.Fields.TransactionMode")]
        public int TransactionModeId { get; set; }
        public SelectList TransactionModes { get; set; }

        [AppResourceDisplayName("Plugins.Payments.Worldpay.Fields.UseSandbox")]
        public bool UseSandbox { get; set; }

        [AppResourceDisplayName("Plugins.Payments.Worldpay.Fields.ValidateAddress")]
        public bool ValidateAddress { get; set; }

        [AppResourceDisplayName("Plugins.Payments.Worldpay.Fields.AdditionalFee")]
        public decimal AdditionalFee { get; set; }

        [AppResourceDisplayName("Plugins.Payments.Worldpay.Fields.AdditionalFeePercentage")]
        public bool AdditionalFeePercentage { get; set; }
    }
}