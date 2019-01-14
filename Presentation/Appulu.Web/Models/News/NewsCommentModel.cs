using System;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Models.News
{
    public partial class NewsCommentModel : BaseAppEntityModel
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string UserAvatarUrl { get; set; }

        public string CommentTitle { get; set; }

        public string CommentText { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool AllowViewingProfiles { get; set; }
    }
}