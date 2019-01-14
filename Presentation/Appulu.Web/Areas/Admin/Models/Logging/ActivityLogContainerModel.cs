using Appulu.Web.Framework.Models;
using System.Collections.Generic;

namespace Appulu.Web.Areas.Admin.Models.Logging
{
    /// <summary>
    /// Represents an activity log container model
    /// </summary>
    public partial class ActivityLogContainerModel : BaseAppModel
    {
        #region Ctor

        public ActivityLogContainerModel()
        {
            ListLogs = new ActivityLogSearchModel();
            ListTypes = new List<ActivityLogTypeModel>();
        }

        #endregion

        #region Properties

        public ActivityLogSearchModel ListLogs { get; set; }

        public IList<ActivityLogTypeModel> ListTypes { get; set; }

        #endregion
    }
}
