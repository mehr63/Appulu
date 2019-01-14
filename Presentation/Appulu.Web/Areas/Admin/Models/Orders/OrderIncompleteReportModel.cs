using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Orders
{
    /// <summary>
    /// Represents an incomplete order report model
    /// </summary>
    public partial class OrderIncompleteReportModel : BaseAppModel
    {
        #region Properties

        [AppResourceDisplayName("Admin.SalesReport.Incomplete.Item")]
        public string Item { get; set; }

        [AppResourceDisplayName("Admin.SalesReport.Incomplete.Total")]
        public string Total { get; set; }

        [AppResourceDisplayName("Admin.SalesReport.Incomplete.Count")]
        public int Count { get; set; }

        [AppResourceDisplayName("Admin.SalesReport.Incomplete.View")]
        public string ViewLink { get; set; }

        #endregion
    }
}