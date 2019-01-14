using System;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.ShoppingCart
{
    /// <summary>
    /// Represents a shopping cart item model
    /// </summary>
    public partial class ShoppingCartItemModel : BaseAppEntityModel
    {
        #region Properties

        [AppResourceDisplayName("Admin.CurrentCarts.Store")]
        public string Store { get; set; }

        [AppResourceDisplayName("Admin.CurrentCarts.Product")]
        public int ProductId { get; set; }

        [AppResourceDisplayName("Admin.CurrentCarts.Product")]
        public string ProductName { get; set; }

        public string AttributeInfo { get; set; }

        [AppResourceDisplayName("Admin.CurrentCarts.UnitPrice")]
        public string UnitPrice { get; set; }

        [AppResourceDisplayName("Admin.CurrentCarts.Quantity")]
        public int Quantity { get; set; }

        [AppResourceDisplayName("Admin.CurrentCarts.Total")]
        public string Total { get; set; }

        [AppResourceDisplayName("Admin.CurrentCarts.UpdatedOn")]
        public DateTime UpdatedOn { get; set; }

        #endregion
    }
}