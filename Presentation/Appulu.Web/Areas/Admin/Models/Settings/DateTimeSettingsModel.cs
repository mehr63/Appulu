using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Settings
{
    /// <summary>
    /// Represents a date time settings model
    /// </summary>
    public partial class DateTimeSettingsModel : BaseAppModel, ISettingsModel
    {
        #region Ctor

        public DateTimeSettingsModel()
        {
            AvailableTimeZones = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        public int ActiveStoreScopeConfiguration { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.AllowUsersToSetTimeZone")]
        public bool AllowUsersToSetTimeZone { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.DefaultStoreTimeZone")]
        public string DefaultStoreTimeZoneId { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.DefaultStoreTimeZone")]
        public IList<SelectListItem> AvailableTimeZones { get; set; }

        #endregion
    }
}