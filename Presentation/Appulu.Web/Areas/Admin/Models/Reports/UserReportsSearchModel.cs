using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Reports
{
    /// <summary>
    /// Represents a user reports search model
    /// </summary>
    public partial class UserReportsSearchModel : BaseSearchModel
    {
        #region Ctor

        public UserReportsSearchModel()
        {
            BestUsersByOrderTotal = new BestUsersReportSearchModel();
            BestUsersByNumberOfOrders = new BestUsersReportSearchModel();
            RegisteredUsers = new RegisteredUsersReportSearchModel();
        }

        #endregion

        #region Properties

        public BestUsersReportSearchModel BestUsersByOrderTotal { get; set; }

        public BestUsersReportSearchModel BestUsersByNumberOfOrders { get; set; }

        public RegisteredUsersReportSearchModel RegisteredUsers { get; set; }

        #endregion
    }
}