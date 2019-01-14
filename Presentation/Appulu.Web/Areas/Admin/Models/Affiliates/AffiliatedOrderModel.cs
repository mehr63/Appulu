using System;
using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Affiliates
{
    /// <summary>
    /// Represents an affiliated order model
    /// </summary>
    public partial class AffiliatedOrderModel : BaseAppEntityModel
    {
        #region Properties

        public override int Id { get; set; }

        [AppResourceDisplayName("Admin.Affiliates.Orders.CustomOrderNumber")]
        public string CustomOrderNumber { get; set; }

        [AppResourceDisplayName("Admin.Affiliates.Orders.OrderStatus")]
        public string OrderStatus { get; set; }
        [AppResourceDisplayName("Admin.Affiliates.Orders.OrderStatus")]
        public int OrderStatusId { get; set; }

        [AppResourceDisplayName("Admin.Affiliates.Orders.PaymentStatus")]
        public string PaymentStatus { get; set; }

        [AppResourceDisplayName("Admin.Affiliates.Orders.ShippingStatus")]
        public string ShippingStatus { get; set; }

        [AppResourceDisplayName("Admin.Affiliates.Orders.OrderTotal")]
        public string OrderTotal { get; set; }

        [AppResourceDisplayName("Admin.Affiliates.Orders.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        #endregion
    }
}