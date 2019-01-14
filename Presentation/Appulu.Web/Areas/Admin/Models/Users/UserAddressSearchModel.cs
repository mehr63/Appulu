using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Users
{
    /// <summary>
    /// Represents a user address search model
    /// </summary>
    public partial class UserAddressSearchModel : BaseSearchModel
    {
        #region Properties

        public int UserId { get; set; }

        #endregion
    }
}