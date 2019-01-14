using Appulu.Core.Domain.Orders;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.ShoppingCart
{
    /// <summary>
    /// Represents a shopping cart item search model
    /// </summary>
    public partial class ShoppingCartItemSearchModel : BaseSearchModel
    {
        #region Properties

        public int UserId { get; set; }

        public ShoppingCartType ShoppingCartType { get; set; }

        #endregion
    }
}