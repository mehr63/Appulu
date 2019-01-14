using System;
using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Users
{
    /// <summary>
    /// Represents a user order model
    /// </summary>
    public partial class UserOrderModel : BaseAppEntityModel
    {
        #region Properties

        public override int Id { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.Orders.CustomOrderNumber")]
        public string CustomOrderNumber { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.Orders.OrderStatus")]
        public string OrderStatus { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.Orders.OrderStatus")]
        public int OrderStatusId { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.Orders.PaymentStatus")]
        public string PaymentStatus { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.Orders.ShippingStatus")]
        public string ShippingStatus { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.Orders.OrderTotal")]
        public string OrderTotal { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.Orders.Store")]
        public string StoreName { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.Orders.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        #endregion
    }
}