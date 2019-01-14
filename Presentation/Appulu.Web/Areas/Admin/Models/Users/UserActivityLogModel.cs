using System;
using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Users
{
    /// <summary>
    /// Represents a user activity log model
    /// </summary>
    public partial class UserActivityLogModel : BaseAppEntityModel
    {
        #region Properties

        [AppResourceDisplayName("Admin.Users.Users.ActivityLog.ActivityLogType")]
        public string ActivityLogTypeName { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.ActivityLog.Comment")]
        public string Comment { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.ActivityLog.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.ActivityLog.IpAddress")]
        public string IpAddress { get; set; }

        #endregion
    }
}