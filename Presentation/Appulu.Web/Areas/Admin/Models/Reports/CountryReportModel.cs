using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Reports
{
    /// <summary>
    /// Represents a country report model
    /// </summary>
    public partial class CountryReportModel : BaseAppModel
    {
        #region Properties

        [AppResourceDisplayName("Admin.Reports.Sales.Country.Fields.CountryName")]
        public string CountryName { get; set; }

        [AppResourceDisplayName("Admin.Reports.Sales.Country.Fields.TotalOrders")]
        public int TotalOrders { get; set; }

        [AppResourceDisplayName("Admin.Reports.Sales.Country.Fields.SumOrders")]
        public string SumOrders { get; set; }

        #endregion
    }
}