using System;
using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Users
{
    /// <summary>
    /// Represents a user back in stock subscription model
    /// </summary>
    public partial class UserBackInStockSubscriptionModel : BaseAppEntityModel
    {
        #region Properties

        [AppResourceDisplayName("Admin.Users.Users.BackInStockSubscriptions.Store")]
        public string StoreName { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.BackInStockSubscriptions.Product")]
        public int ProductId { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.BackInStockSubscriptions.Product")]
        public string ProductName { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.BackInStockSubscriptions.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        #endregion
    }
}