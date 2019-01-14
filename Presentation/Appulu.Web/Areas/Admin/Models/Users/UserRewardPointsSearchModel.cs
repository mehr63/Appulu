using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Users
{
    /// <summary>
    /// Represents a reward points search model
    /// </summary>
    public partial class UserRewardPointsSearchModel : BaseSearchModel
    {
        #region Properties

        public int UserId { get; set; }
        
        #endregion
    }
}