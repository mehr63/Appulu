using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Models.News
{
    public partial class AddNewsCommentModel : BaseAppModel
    {
        [AppResourceDisplayName("News.Comments.CommentTitle")]
        public string CommentTitle { get; set; }

        [AppResourceDisplayName("News.Comments.CommentText")]
        public string CommentText { get; set; }

        public bool DisplayCaptcha { get; set; }
    }
}