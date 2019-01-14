using Appulu.Web.Areas.Admin.Models.ShoppingCart;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Users
{
    /// <summary>
    /// Represents a user shopping cart list model
    /// </summary>
    public partial class UserShoppingCartListModel : BasePagedListModel<ShoppingCartItemModel>
    {
    }
}