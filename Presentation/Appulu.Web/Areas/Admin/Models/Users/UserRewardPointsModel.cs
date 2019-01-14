using System;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Users
{
    /// <summary>
    /// Represents a reward point model
    /// </summary>
    public partial class UserRewardPointsModel : BaseAppEntityModel
    {
        #region Properties

        public string StoreName { get; set; }

        public int Points { get; set; }

        public string PointsBalance { get; set; }

        public string Message { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? EndDate { get; set; }

        #endregion
    }
}