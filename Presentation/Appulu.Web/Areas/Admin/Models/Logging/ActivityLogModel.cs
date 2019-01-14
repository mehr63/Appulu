using System;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Logging
{
    /// <summary>
    /// Represents an activity log model
    /// </summary>
    public partial class ActivityLogModel : BaseAppEntityModel
    {
        #region Properties

        [AppResourceDisplayName("Admin.Configuration.ActivityLog.ActivityLog.Fields.ActivityLogType")]
        public string ActivityLogTypeName { get; set; }

        [AppResourceDisplayName("Admin.Configuration.ActivityLog.ActivityLog.Fields.User")]
        public int UserId { get; set; }

        [AppResourceDisplayName("Admin.Configuration.ActivityLog.ActivityLog.Fields.User")]
        public string UserEmail { get; set; }

        [AppResourceDisplayName("Admin.Configuration.ActivityLog.ActivityLog.Fields.Comment")]
        public string Comment { get; set; }

        [AppResourceDisplayName("Admin.Configuration.ActivityLog.ActivityLog.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.ActivityLog.IpAddress")]
        public string IpAddress { get; set; }

        #endregion
    }
}