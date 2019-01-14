using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Users
{
    /// <summary>
    /// Represents a GDPR log search model
    /// </summary>
    public partial class GdprLogSearchModel : BaseSearchModel
    {
        #region Ctor

        public GdprLogSearchModel()
        {
            AvailableRequestTypes = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [AppResourceDisplayName("Admin.Users.GdprLog.List.SearchEmail")]
        public string SearchEmail { get; set; }

        [AppResourceDisplayName("Admin.Users.GdprLog.List.SearchRequestType")]
        public int SearchRequestTypeId { get; set; }

        public IList<SelectListItem> AvailableRequestTypes { get; set; }

        #endregion
    }
}