using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.News
{
    /// <summary>
    /// Represents a news item search model
    /// </summary>
    public partial class NewsItemSearchModel : BaseSearchModel
    {
        #region Ctor

        public NewsItemSearchModel()
        {
            AvailableStores = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [AppResourceDisplayName("Admin.ContentManagement.News.NewsItems.List.SearchStore")]
        public int SearchStoreId { get; set; }

        public IList<SelectListItem> AvailableStores { get; set; }

        #endregion
    }
}