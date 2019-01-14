using System.Collections.Generic;
using Appulu.Web.Framework.Models;
using Appulu.Web.Models.Common;

namespace Appulu.Web.Models.Catalog
{
    public partial class UserBackInStockSubscriptionsModel : BaseAppModel
    {
        public UserBackInStockSubscriptionsModel()
        {
            this.Subscriptions = new List<BackInStockSubscriptionModel>();
        }

        public IList<BackInStockSubscriptionModel> Subscriptions { get; set; }
        public PagerModel PagerModel { get; set; }

        #region Nested classes

        public partial class BackInStockSubscriptionModel : BaseAppEntityModel
        {
            public int ProductId { get; set; }
            public string ProductName { get; set; }
            public string SeName { get; set; }
        }

        #endregion
    }
}