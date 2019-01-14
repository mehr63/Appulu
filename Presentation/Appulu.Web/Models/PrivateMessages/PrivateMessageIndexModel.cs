using Appulu.Web.Framework.Models;

namespace Appulu.Web.Models.PrivateMessages
{
    public partial class PrivateMessageIndexModel : BaseAppModel
    {
        public int InboxPage { get; set; }
        public int SentItemsPage { get; set; }
        public bool SentItemsTabSelected { get; set; }
    }
}