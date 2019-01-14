using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Users
{
    /// <summary>
    /// Represents a user shopping cart search model
    /// </summary>
    public partial class UserShoppingCartSearchModel : BaseSearchModel
    {
        #region Ctor

        public UserShoppingCartSearchModel()
        {
            AvailableShoppingCartTypes = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        public int UserId { get; set; }

        [AppResourceDisplayName("Admin.ShoppingCartType.ShoppingCartType")]
        public int ShoppingCartTypeId { get; set; }

        public IList<SelectListItem> AvailableShoppingCartTypes { get; set; }

        #endregion
    }
}