using System;
using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Discounts
{
    /// <summary>
    /// Represents a discount usage history model
    /// </summary>
    public partial class DiscountUsageHistoryModel : BaseAppEntityModel
    {
        #region Properties

        public int DiscountId { get; set; }

        public int OrderId { get; set; }

        [AppResourceDisplayName("Admin.Promotions.Discounts.History.CustomOrderNumber")]
        public string CustomOrderNumber { get; set; }

        [AppResourceDisplayName("Admin.Promotions.Discounts.History.OrderTotal")]
        public string OrderTotal { get; set; }

        [AppResourceDisplayName("Admin.Promotions.Discounts.History.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        #endregion
    }
}