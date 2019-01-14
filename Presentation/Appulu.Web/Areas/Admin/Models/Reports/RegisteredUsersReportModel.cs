using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Reports
{
    /// <summary>
    /// Represents a registered users report model
    /// </summary>
    public partial class RegisteredUsersReportModel : BaseAppModel
    {
        #region Properties

        [AppResourceDisplayName("Admin.Reports.Users.RegisteredUsers.Fields.Period")]
        public string Period { get; set; }

        [AppResourceDisplayName("Admin.Reports.Users.RegisteredUsers.Fields.Users")]
        public int Users { get; set; }

        #endregion
    }
}