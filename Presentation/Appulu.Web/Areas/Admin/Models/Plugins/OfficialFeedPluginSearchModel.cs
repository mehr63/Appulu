using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Plugins
{
    /// <summary>
    /// Represents a search model of plugins of the official feed
    /// </summary>
    public partial class OfficialFeedPluginSearchModel : BaseSearchModel
    {
        #region Ctor

        public OfficialFeedPluginSearchModel()
        {
            AvailableVersions = new List<SelectListItem>();
            AvailableCategories = new List<SelectListItem>();
            AvailablePrices = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [AppResourceDisplayName("Admin.Configuration.Plugins.OfficialFeed.Name")]
        public string SearchName { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Plugins.OfficialFeed.Version")]
        public int SearchVersionId { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Plugins.OfficialFeed.Category")]
        public int SearchCategoryId { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Plugins.OfficialFeed.Price")]
        public int SearchPriceId { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Plugins.OfficialFeed.Version")]
        public IList<SelectListItem> AvailableVersions { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Plugins.OfficialFeed.Category")]
        public IList<SelectListItem> AvailableCategories { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Plugins.OfficialFeed.Price")]
        public IList<SelectListItem> AvailablePrices { get; set; }

        #endregion
    }
}