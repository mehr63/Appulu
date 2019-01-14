using FluentValidation.Attributes;
using Appulu.Core.Domain.Forums;
using Appulu.Web.Framework.Models;
using Appulu.Web.Validators.Boards;

namespace Appulu.Web.Models.Boards
{
    [Validator(typeof(EditForumPostValidator))]
    public partial class EditForumPostModel : BaseAppModel
    {
        public int Id { get; set; }
        public int ForumTopicId { get; set; }

        public bool IsEdit { get; set; }

        public string Text { get; set; }
        public EditorType ForumEditor { get; set; }

        public string ForumName { get; set; }
        public string ForumTopicSubject { get; set; }
        public string ForumTopicSeName { get; set; }

        public bool IsUserAllowedToSubscribe { get; set; }
        public bool Subscribed { get; set; }
    }
}