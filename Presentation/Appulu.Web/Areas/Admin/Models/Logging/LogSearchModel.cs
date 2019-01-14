using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Logging
{
    /// <summary>
    /// Represents a log search model
    /// </summary>
    public partial class LogSearchModel : BaseSearchModel
    {
        #region Ctor

        public LogSearchModel()
        {
            AvailableLogLevels = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [AppResourceDisplayName("Admin.System.Log.List.CreatedOnFrom")]
        [UIHint("DateNullable")]
        public DateTime? CreatedOnFrom { get; set; }

        [AppResourceDisplayName("Admin.System.Log.List.CreatedOnTo")]
        [UIHint("DateNullable")]
        public DateTime? CreatedOnTo { get; set; }

        [AppResourceDisplayName("Admin.System.Log.List.Message")]
        public string Message { get; set; }

        [AppResourceDisplayName("Admin.System.Log.List.LogLevel")]
        public int LogLevelId { get; set; }

        public IList<SelectListItem> AvailableLogLevels { get; set; }

        #endregion
    }
}