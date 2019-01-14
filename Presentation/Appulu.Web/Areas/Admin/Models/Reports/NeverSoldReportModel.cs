using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Reports
{
    /// <summary>
    /// Represents a never sold products report model
    /// </summary>
    public partial class NeverSoldReportModel : BaseAppModel
    {
        #region Properties

        public int ProductId { get; set; }

        [AppResourceDisplayName("Admin.Reports.Sales.NeverSold.Fields.Name")]
        public string ProductName { get; set; }

        #endregion
    }
}