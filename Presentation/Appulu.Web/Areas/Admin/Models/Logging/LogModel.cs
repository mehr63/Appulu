using System;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Logging
{
    /// <summary>
    /// Represents a log model
    /// </summary>
    public partial class LogModel : BaseAppEntityModel
    {
        #region Properties

        [AppResourceDisplayName("Admin.System.Log.Fields.LogLevel")]
        public string LogLevel { get; set; }

        [AppResourceDisplayName("Admin.System.Log.Fields.ShortMessage")]
        public string ShortMessage { get; set; }

        [AppResourceDisplayName("Admin.System.Log.Fields.FullMessage")]
        public string FullMessage { get; set; }

        [AppResourceDisplayName("Admin.System.Log.Fields.IPAddress")]
        public string IpAddress { get; set; }

        [AppResourceDisplayName("Admin.System.Log.Fields.User")]
        public int? UserId { get; set; }

        [AppResourceDisplayName("Admin.System.Log.Fields.User")]
        public string UserEmail { get; set; }

        [AppResourceDisplayName("Admin.System.Log.Fields.PageURL")]
        public string PageUrl { get; set; }

        [AppResourceDisplayName("Admin.System.Log.Fields.ReferrerURL")]
        public string ReferrerUrl { get; set; }

        [AppResourceDisplayName("Admin.System.Log.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        #endregion
    }
}