using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Orders
{
    /// <summary>
    /// Represents a recurring payment model
    /// </summary>
    public partial class RecurringPaymentModel : BaseAppEntityModel
    {
        #region Ctor

        public RecurringPaymentModel()
        {
            this.RecurringPaymentHistorySearchModel = new RecurringPaymentHistorySearchModel();
        }

        #endregion

        #region Properties

        [AppResourceDisplayName("Admin.RecurringPayments.Fields.ID")]
        public override int Id { get; set; }

        [AppResourceDisplayName("Admin.RecurringPayments.Fields.CycleLength")]
        public int CycleLength { get; set; }

        [AppResourceDisplayName("Admin.RecurringPayments.Fields.CyclePeriod")]
        public int CyclePeriodId { get; set; }

        [AppResourceDisplayName("Admin.RecurringPayments.Fields.CyclePeriod")]
        public string CyclePeriodStr { get; set; }

        [AppResourceDisplayName("Admin.RecurringPayments.Fields.TotalCycles")]
        public int TotalCycles { get; set; }

        [AppResourceDisplayName("Admin.RecurringPayments.Fields.StartDate")]
        public string StartDate { get; set; }

        [AppResourceDisplayName("Admin.RecurringPayments.Fields.IsActive")]
        public bool IsActive { get; set; }

        [AppResourceDisplayName("Admin.RecurringPayments.Fields.NextPaymentDate")]
        public string NextPaymentDate { get; set; }

        [AppResourceDisplayName("Admin.RecurringPayments.Fields.CyclesRemaining")]
        public int CyclesRemaining { get; set; }

        [AppResourceDisplayName("Admin.RecurringPayments.Fields.InitialOrder")]
        public int InitialOrderId { get; set; }

        [AppResourceDisplayName("Admin.RecurringPayments.Fields.User")]
        public int UserId { get; set; }

        [AppResourceDisplayName("Admin.RecurringPayments.Fields.User")]
        public string UserEmail { get; set; }

        [AppResourceDisplayName("Admin.RecurringPayments.Fields.PaymentType")]
        public string PaymentType { get; set; }

        public bool CanCancelRecurringPayment { get; set; }

        public bool LastPaymentFailed { get; set; }

        public RecurringPaymentHistorySearchModel RecurringPaymentHistorySearchModel { get; set; }

        #endregion
    }
}