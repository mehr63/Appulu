using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Payments
{
    /// <summary>
    /// Represents a payment method model
    /// </summary>
    public partial class PaymentMethodModel : BaseAppModel, IPluginModel
    {
        #region Properties

        [AppResourceDisplayName("Admin.Configuration.Payment.Methods.Fields.FriendlyName")]
        public string FriendlyName { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Payment.Methods.Fields.SystemName")]
        public string SystemName { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Payment.Methods.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Payment.Methods.Fields.IsActive")]
        public bool IsActive { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Payment.Methods.Configure")]
        public string ConfigurationUrl { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Payment.Methods.Fields.Logo")]
        public string LogoUrl { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Payment.Methods.Fields.SupportCapture")]
        public bool SupportCapture { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Payment.Methods.Fields.SupportPartiallyRefund")]
        public bool SupportPartiallyRefund { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Payment.Methods.Fields.SupportRefund")]
        public bool SupportRefund { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Payment.Methods.Fields.SupportVoid")]
        public bool SupportVoid { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Payment.Methods.Fields.RecurringPaymentType")]
        public string RecurringPaymentType { get; set; }

        #endregion
    }
}