using System;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Blogs
{
    /// <summary>
    /// Represents a blog comment model
    /// </summary>
    public partial class BlogCommentModel : BaseAppEntityModel
    {
        #region Properties

        [AppResourceDisplayName("Admin.ContentManagement.Blog.Comments.Fields.BlogPost")]
        public int BlogPostId { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.Blog.Comments.Fields.BlogPost")]
        public string BlogPostTitle { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.Blog.Comments.Fields.User")]
        public int UserId { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.Blog.Comments.Fields.User")]
        public string UserInfo { get; set; }
        
        [AppResourceDisplayName("Admin.ContentManagement.Blog.Comments.Fields.Comment")]
        public string Comment { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.Blog.Comments.Fields.IsApproved")]
        public bool IsApproved { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.Blog.Comments.Fields.StoreName")]
        public int StoreId { get; set; }
        public string StoreName { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.Blog.Comments.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        #endregion
    }
}