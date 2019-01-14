using System;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.News
{
    /// <summary>
    /// Represents a news comment model
    /// </summary>
    public partial class NewsCommentModel : BaseAppEntityModel
    {
        #region Properties

        [AppResourceDisplayName("Admin.ContentManagement.News.Comments.Fields.NewsItem")]
        public int NewsItemId { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.News.Comments.Fields.NewsItem")]
        public string NewsItemTitle { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.News.Comments.Fields.User")]
        public int UserId { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.News.Comments.Fields.User")]
        public string UserInfo { get; set; }
        
        [AppResourceDisplayName("Admin.ContentManagement.News.Comments.Fields.CommentTitle")]
        public string CommentTitle { get; set; }
        
        [AppResourceDisplayName("Admin.ContentManagement.News.Comments.Fields.CommentText")]
        public string CommentText { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.News.Comments.Fields.IsApproved")]
        public bool IsApproved { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.News.Comments.Fields.StoreName")]
        public int StoreId { get; set; }

        public string StoreName { get; set; }

        [AppResourceDisplayName("Admin.ContentManagement.News.Comments.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        #endregion
    }
}