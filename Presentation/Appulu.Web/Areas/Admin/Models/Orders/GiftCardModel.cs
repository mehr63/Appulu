using System;
using System.ComponentModel.DataAnnotations;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Orders
{
    /// <summary>
    /// Represents a gift card model
    /// </summary>
    public partial class GiftCardModel: BaseAppEntityModel
    {
        #region Ctor

        public GiftCardModel()
        {
            this.GiftCardUsageHistorySearchModel = new GiftCardUsageHistorySearchModel();
        }

        #endregion

        #region Properties

        [AppResourceDisplayName("Admin.GiftCards.Fields.GiftCardType")]
        public int GiftCardTypeId { get; set; }

        [AppResourceDisplayName("Admin.GiftCards.Fields.OrderId")]
        public int? PurchasedWithOrderId { get; set; }

        [AppResourceDisplayName("Admin.GiftCards.Fields.CustomOrderNumber")]
        public string PurchasedWithOrderNumber { get; set; }

        [AppResourceDisplayName("Admin.GiftCards.Fields.Amount")]
        public decimal Amount { get; set; }

        [AppResourceDisplayName("Admin.GiftCards.Fields.Amount")]
        public string AmountStr { get; set; }

        [AppResourceDisplayName("Admin.GiftCards.Fields.RemainingAmount")]
        public string RemainingAmountStr { get; set; }

        [AppResourceDisplayName("Admin.GiftCards.Fields.IsGiftCardActivated")]
        public bool IsGiftCardActivated { get; set; }

        [AppResourceDisplayName("Admin.GiftCards.Fields.GiftCardCouponCode")]
        public string GiftCardCouponCode { get; set; }

        [AppResourceDisplayName("Admin.GiftCards.Fields.RecipientName")]
        public string RecipientName { get; set; }

        [DataType(DataType.EmailAddress)]
        [AppResourceDisplayName("Admin.GiftCards.Fields.RecipientEmail")]
        public string RecipientEmail { get; set; }

        [AppResourceDisplayName("Admin.GiftCards.Fields.SenderName")]
        public string SenderName { get; set; }

        [DataType(DataType.EmailAddress)]
        [AppResourceDisplayName("Admin.GiftCards.Fields.SenderEmail")]
        public string SenderEmail { get; set; }

        [AppResourceDisplayName("Admin.GiftCards.Fields.Message")]
        public string Message { get; set; }

        [AppResourceDisplayName("Admin.GiftCards.Fields.IsRecipientNotified")]
        public bool IsRecipientNotified { get; set; }

        [AppResourceDisplayName("Admin.GiftCards.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        public string PrimaryStoreCurrencyCode { get; set; }

        public GiftCardUsageHistorySearchModel GiftCardUsageHistorySearchModel { get; set; }

        #endregion
    }
}