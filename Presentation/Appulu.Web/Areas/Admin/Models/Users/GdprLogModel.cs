using System;
using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Users
{
    /// <summary>
    /// Represents a GDPR log (request) model
    /// </summary>
    public partial class GdprLogModel : BaseAppEntityModel
    {
        #region Properties

        [AppResourceDisplayName("Admin.Users.GdprLog.Fields.UserInfo")]
        public string UserInfo { get; set; }

        [AppResourceDisplayName("Admin.Users.GdprLog.Fields.RequestType")]
        public string RequestType { get; set; }

        [AppResourceDisplayName("Admin.Users.GdprLog.Fields.RequestDetails")]
        public string RequestDetails { get; set; }

        [AppResourceDisplayName("Admin.Users.GdprLog.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        #endregion
    }
}