﻿using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Users
{
    /// <summary>
    /// Represents a user activity log search model
    /// </summary>
    public partial class UserActivityLogSearchModel : BaseSearchModel
    {
        #region Properties

        public int UserId { get; set; }

        #endregion
    }
}