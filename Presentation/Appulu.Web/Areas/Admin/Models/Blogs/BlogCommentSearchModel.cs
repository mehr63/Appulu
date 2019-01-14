using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Blogs
{
    /// <summary>
    /// Represents a blog comment search model
    /// </summary>
    public partial class BlogCommentSearchModel : BaseSearchModel
    {
        #region Ctor

        public BlogCommentSearchModel()
        {
            AvailableApprovedOptions = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [AppResourceDisplayName("Admin.ContentManagement.Blog.Comments.List.BlogPostId")]
        [UIHint("Int32Nullable")]
        public int? BlogPostId { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.Blog.Comments.List.CreatedOnFrom")]
        [UIHint("DateNullable")]
        public DateTime? CreatedOnFrom { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.Blog.Comments.List.CreatedOnTo")]
        [UIHint("DateNullable")]
        public DateTime? CreatedOnTo { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.Blog.Comments.List.SearchText")]
        public string SearchText { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.Blog.Comments.List.SearchApproved")]
        public int SearchApprovedId { get; set; }

        public IList<SelectListItem> AvailableApprovedOptions { get; set; }        

        #endregion
    }
}