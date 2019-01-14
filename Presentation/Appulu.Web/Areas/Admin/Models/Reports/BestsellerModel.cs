using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Reports
{
    /// <summary>
    /// Represents a bestseller model
    /// </summary>
    public partial class BestsellerModel : BaseAppModel
    {
        #region Properties

        public int ProductId { get; set; }

        [AppResourceDisplayName("Admin.Reports.Sales.Bestsellers.Fields.Name")]
        public string ProductName { get; set; }

        [AppResourceDisplayName("Admin.Reports.Sales.Bestsellers.Fields.TotalAmount")]
        public string TotalAmount { get; set; }

        [AppResourceDisplayName("Admin.Reports.Sales.Bestsellers.Fields.TotalQuantity")]
        public decimal TotalQuantity { get; set; }

        #endregion
    }
}