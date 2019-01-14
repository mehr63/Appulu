using Appulu.Web.Framework.Models;

namespace Appulu.Web.Models.Catalog
{
    public partial class BackInStockSubscribeModel : BaseAppModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSeName { get; set; }

        public bool IsCurrentUserRegistered { get; set; }
        public bool SubscriptionAllowed { get; set; }
        public bool AlreadySubscribed { get; set; }

        public int MaximumBackInStockSubscriptions { get; set; }
        public int CurrentNumberOfBackInStockSubscriptions { get; set; }
    }
}