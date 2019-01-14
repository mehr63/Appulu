using System;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Users
{
    /// <summary>
    /// Represents an online user model
    /// </summary>
    public partial class OnlineUserModel : BaseAppEntityModel
    {
        #region Properties

        [AppResourceDisplayName("Admin.Users.OnlineUsers.Fields.UserInfo")]
        public string UserInfo { get; set; }

        [AppResourceDisplayName("Admin.Users.OnlineUsers.Fields.IPAddress")]
        public string LastIpAddress { get; set; }

        [AppResourceDisplayName("Admin.Users.OnlineUsers.Fields.Location")]
        public string Location { get; set; }

        [AppResourceDisplayName("Admin.Users.OnlineUsers.Fields.LastActivityDate")]
        public DateTime LastActivityDate { get; set; }
        
        [AppResourceDisplayName("Admin.Users.OnlineUsers.Fields.LastVisitedPage")]
        public string LastVisitedPage { get; set; }

        #endregion
    }
}