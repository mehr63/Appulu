using System.Collections.Generic;
using Appulu.Web.Framework.Models;
using Appulu.Web.Models.Common;

namespace Appulu.Web.Models.PrivateMessages
{
    public partial class PrivateMessageListModel : BaseAppModel
    {
        public IList<PrivateMessageModel> Messages { get; set; }
        public PagerModel PagerModel { get; set; }
    }
}