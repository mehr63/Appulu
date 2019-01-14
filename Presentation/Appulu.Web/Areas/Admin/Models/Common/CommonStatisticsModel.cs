using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Common
{
    public partial class CommonStatisticsModel : BaseAppModel
    {
        public int NumberOfOrders { get; set; }

        public int NumberOfUsers { get; set; }

        public int NumberOfPendingReturnRequests { get; set; }

        public int NumberOfLowStockProducts { get; set; }
    }
}