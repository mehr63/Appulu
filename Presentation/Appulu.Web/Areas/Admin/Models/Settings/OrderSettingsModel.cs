﻿using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;
using Appulu.Web.Areas.Admin.Models.Orders;

namespace Appulu.Web.Areas.Admin.Models.Settings
{
    /// <summary>
    /// Represents an order settings model
    /// </summary>
    public partial class OrderSettingsModel : BaseAppModel, ISettingsModel
    {
        #region Ctor

        public OrderSettingsModel()
        {
            this.ReturnRequestReasonSearchModel = new ReturnRequestReasonSearchModel();
            this.ReturnRequestActionSearchModel = new ReturnRequestActionSearchModel();
        }

        #endregion

        #region Properties

        public int ActiveStoreScopeConfiguration { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.Order.IsReOrderAllowed")]
        public bool IsReOrderAllowed { get; set; }
        public bool IsReOrderAllowed_OverrideForStore { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.Order.MinOrderSubtotalAmount")]
        public decimal MinOrderSubtotalAmount { get; set; }
        public bool MinOrderSubtotalAmount_OverrideForStore { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.Order.MinOrderSubtotalAmountIncludingTax")]
        public bool MinOrderSubtotalAmountIncludingTax { get; set; }
        public bool MinOrderSubtotalAmountIncludingTax_OverrideForStore { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.Order.MinOrderTotalAmount")]
        public decimal MinOrderTotalAmount { get; set; }
        public bool MinOrderTotalAmount_OverrideForStore { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.Order.AutoUpdateOrderTotalsOnEditingOrder")]
        public bool AutoUpdateOrderTotalsOnEditingOrder { get; set; }
        public bool AutoUpdateOrderTotalsOnEditingOrder_OverrideForStore { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.Order.AnonymousCheckoutAllowed")]
        public bool AnonymousCheckoutAllowed { get; set; }
        public bool AnonymousCheckoutAllowed_OverrideForStore { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.Order.CheckoutDisabled")]
        public bool CheckoutDisabled { get; set; }
        public bool CheckoutDisabled_OverrideForStore { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.Order.TermsOfServiceOnShoppingCartPage")]
        public bool TermsOfServiceOnShoppingCartPage { get; set; }
        public bool TermsOfServiceOnShoppingCartPage_OverrideForStore { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.Order.TermsOfServiceOnOrderConfirmPage")]
        public bool TermsOfServiceOnOrderConfirmPage { get; set; }
        public bool TermsOfServiceOnOrderConfirmPage_OverrideForStore { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.Order.OnePageCheckoutEnabled")]
        public bool OnePageCheckoutEnabled { get; set; }
        public bool OnePageCheckoutEnabled_OverrideForStore { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.Order.OnePageCheckoutDisplayOrderTotalsOnPaymentInfoTab")]
        public bool OnePageCheckoutDisplayOrderTotalsOnPaymentInfoTab { get; set; }
        public bool OnePageCheckoutDisplayOrderTotalsOnPaymentInfoTab_OverrideForStore { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.Order.DisableBillingAddressCheckoutStep")]
        public bool DisableBillingAddressCheckoutStep { get; set; }
        public bool DisableBillingAddressCheckoutStep_OverrideForStore { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.Order.DisableOrderCompletedPage")]
        public bool DisableOrderCompletedPage { get; set; }
        public bool DisableOrderCompletedPage_OverrideForStore { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.Order.AttachPdfInvoiceToOrderPlacedEmail")]
        public bool AttachPdfInvoiceToOrderPlacedEmail { get; set; }
        public bool AttachPdfInvoiceToOrderPlacedEmail_OverrideForStore { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.Order.AttachPdfInvoiceToOrderPaidEmail")]
        public bool AttachPdfInvoiceToOrderPaidEmail { get; set; }
        public bool AttachPdfInvoiceToOrderPaidEmail_OverrideForStore { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.Order.AttachPdfInvoiceToOrderCompletedEmail")]
        public bool AttachPdfInvoiceToOrderCompletedEmail { get; set; }
        public bool AttachPdfInvoiceToOrderCompletedEmail_OverrideForStore { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.Order.ReturnRequestsEnabled")]
        public bool ReturnRequestsEnabled { get; set; }
        public bool ReturnRequestsEnabled_OverrideForStore { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.Order.ReturnRequestsAllowFiles")]
        public bool ReturnRequestsAllowFiles { get; set; }
        public bool ReturnRequestsAllowFiles_OverrideForStore { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.Order.ReturnRequestNumberMask")]
        public string ReturnRequestNumberMask { get; set; }
        public bool ReturnRequestNumberMask_OverrideForStore { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.Order.NumberOfDaysReturnRequestAvailable")]
        public int NumberOfDaysReturnRequestAvailable { get; set; }
        public bool NumberOfDaysReturnRequestAvailable_OverrideForStore { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.Order.ActivateGiftCardsAfterCompletingOrder")]
        public bool ActivateGiftCardsAfterCompletingOrder { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.Order.DeactivateGiftCardsAfterCancellingOrder")]
        public bool DeactivateGiftCardsAfterCancellingOrder { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.Order.DeactivateGiftCardsAfterDeletingOrder")]
        public bool DeactivateGiftCardsAfterDeletingOrder { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.Order.CompleteOrderWhenDelivered")]
        public bool CompleteOrderWhenDelivered { get; set; }

        public string PrimaryStoreCurrencyCode { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.Order.OrderIdent")]
        public int? OrderIdent { get; set; }
        
        [AppResourceDisplayName("Admin.Configuration.Settings.Order.CustomOrderNumberMask")]
        public string CustomOrderNumberMask { get; set; }
        public bool CustomOrderNumberMask_OverrideForStore { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.Order.ExportWithProducts")]
        public bool ExportWithProducts { get; set; }
        public bool ExportWithProducts_OverrideForStore { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.Order.AllowAdminsToBuyCallForPriceProducts")]
        public bool AllowAdminsToBuyCallForPriceProducts { get; set; }
        public bool AllowAdminsToBuyCallForPriceProducts_OverrideForStore { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.Order.DeleteGiftCardUsageHistory")]
        public bool DeleteGiftCardUsageHistory { get; set; }
        public bool DeleteGiftCardUsageHistory_OverrideForStore { get; set; }

        public ReturnRequestReasonSearchModel ReturnRequestReasonSearchModel { get; set; }

        public ReturnRequestActionSearchModel ReturnRequestActionSearchModel { get; set; }

        #endregion
    }
}