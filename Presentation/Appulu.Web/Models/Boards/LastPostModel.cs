using Appulu.Web.Framework.Models;

namespace Appulu.Web.Models.Boards
{
    public partial class LastPostModel : BaseAppModel
    {
        public int Id { get; set; }
        public int ForumTopicId { get; set; }
        public string ForumTopicSeName { get; set; }
        public string ForumTopicSubject { get; set; }
        
        public int UserId { get; set; }
        public bool AllowViewingProfiles { get; set; }
        public string UserName { get; set; }

        public string PostCreatedOnStr { get; set; }
        
        public bool ShowTopic { get; set; }
    }
}