using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.News
{
    /// <summary>
    /// Represents a news comment search model
    /// </summary>
    public partial class NewsCommentSearchModel : BaseSearchModel
    {
        #region Ctor

        public NewsCommentSearchModel()
        {
            AvailableApprovedOptions = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [AppResourceDisplayName("Admin.ContentManagement.News.Comments.List.NewsItemId")]
        [UIHint("Int32Nullable")]
        public int? NewsItemId { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.News.Comments.List.CreatedOnFrom")]
        [UIHint("DateNullable")]
        public DateTime? CreatedOnFrom { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.News.Comments.List.CreatedOnTo")]
        [UIHint("DateNullable")]
        public DateTime? CreatedOnTo { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.News.Comments.List.SearchText")]
        public string SearchText { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.News.Comments.List.SearchApproved")]
        public int SearchApprovedId { get; set; }

        public IList<SelectListItem> AvailableApprovedOptions { get; set; }

        #endregion
    }
}