using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Users
{
    /// <summary>
    /// Represents a user orders search model
    /// </summary>
    public partial class UserOrderSearchModel : BaseSearchModel
    {
        #region Properties

        public int UserId { get; set; }

        #endregion
    }
}