using System;
using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Orders
{
    /// <summary>
    /// Represents a gift card usage history model
    /// </summary>
    public partial class GiftCardUsageHistoryModel : BaseAppEntityModel
    {
        #region Properties

        [AppResourceDisplayName("Admin.GiftCards.History.UsedValue")]
        public string UsedValue { get; set; }

        public int OrderId { get; set; }

        [AppResourceDisplayName("Admin.GiftCards.History.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        [AppResourceDisplayName("Admin.GiftCards.History.CustomOrderNumber")]
        public string CustomOrderNumber { get; set; }

        #endregion
    }
}