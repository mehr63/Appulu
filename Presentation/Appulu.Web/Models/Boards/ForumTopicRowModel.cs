using Appulu.Core.Domain.Forums;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Models.Boards
{
    public partial class ForumTopicRowModel : BaseAppModel
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string SeName { get; set; }
        public int LastPostId { get; set; }

        public int NumPosts { get; set; }
        public int Views { get; set; }
        public int Votes { get; set; }
        public int NumReplies { get; set; }
        public ForumTopicType ForumTopicType { get; set; }

        public int UserId { get; set; }
        public bool AllowViewingProfiles { get; set; }
        public string UserName { get; set; }

        //posts
        public int TotalPostPages { get; set; }
    }
}