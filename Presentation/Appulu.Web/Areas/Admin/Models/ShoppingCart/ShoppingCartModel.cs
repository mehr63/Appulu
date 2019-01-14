using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.ShoppingCart
{
    /// <summary>
    /// Represents a shopping cart model
    /// </summary>
    public partial class ShoppingCartModel : BaseAppModel
    {
        #region Properties

        [AppResourceDisplayName("Admin.CurrentCarts.User")]
        public int UserId { get; set; }

        [AppResourceDisplayName("Admin.CurrentCarts.User")]
        public string UserEmail { get; set; }

        [AppResourceDisplayName("Admin.CurrentCarts.TotalItems")]
        public int TotalItems { get; set; }

        #endregion
    }
}