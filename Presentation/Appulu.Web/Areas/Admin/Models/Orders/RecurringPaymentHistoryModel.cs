using System;
using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Orders
{
    /// <summary>
    /// Represents a recurring payment history model
    /// </summary>
    public partial class RecurringPaymentHistoryModel : BaseAppEntityModel
    {
        #region Properties

        public int OrderId { get; set; }

        [AppResourceDisplayName("Admin.RecurringPayments.History.CustomOrderNumber")]
        public string CustomOrderNumber { get; set; }

        public int RecurringPaymentId { get; set; }

        [AppResourceDisplayName("Admin.RecurringPayments.History.OrderStatus")]
        public string OrderStatus { get; set; }

        [AppResourceDisplayName("Admin.RecurringPayments.History.PaymentStatus")]
        public string PaymentStatus { get; set; }

        [AppResourceDisplayName("Admin.RecurringPayments.History.ShippingStatus")]
        public string ShippingStatus { get; set; }

        [AppResourceDisplayName("Admin.RecurringPayments.History.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        #endregion
    }
}