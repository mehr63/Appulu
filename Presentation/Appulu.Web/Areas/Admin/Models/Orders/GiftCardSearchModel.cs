using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Orders
{
    /// <summary>
    /// Represents a gift card search model
    /// </summary>
    public partial class GiftCardSearchModel : BaseSearchModel
    {
        #region Ctor

        public GiftCardSearchModel()
        {
            ActivatedList = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [AppResourceDisplayName("Admin.GiftCards.List.CouponCode")]
        public string CouponCode { get; set; }

        [AppResourceDisplayName("Admin.GiftCards.List.RecipientName")]
        public string RecipientName { get; set; }

        [AppResourceDisplayName("Admin.GiftCards.List.Activated")]
        public int ActivatedId { get; set; }

        [AppResourceDisplayName("Admin.GiftCards.List.Activated")]
        public IList<SelectListItem> ActivatedList { get; set; }

        #endregion
    }
}