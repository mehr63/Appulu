using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Reports
{
    /// <summary>
    /// Represents a best users report model
    /// </summary>
    public partial class BestUsersReportModel : BaseAppModel
    {
        #region Properties

        public int UserId { get; set; }

        [AppResourceDisplayName("Admin.Reports.Users.BestBy.Fields.User")]
        public string UserName { get; set; }

        [AppResourceDisplayName("Admin.Reports.Users.BestBy.Fields.OrderTotal")]
        public string OrderTotal { get; set; }

        [AppResourceDisplayName("Admin.Reports.Users.BestBy.Fields.OrderCount")]
        public decimal OrderCount { get; set; }
        
        #endregion
    }
}