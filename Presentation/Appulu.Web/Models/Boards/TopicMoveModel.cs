using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Models.Boards
{
    public partial class TopicMoveModel : BaseAppEntityModel
    {
        public TopicMoveModel()
        {
            ForumList = new List<SelectListItem>();
        }

        public int ForumSelected { get; set; }
        public string TopicSeName { get; set; }
        
        public IEnumerable<SelectListItem> ForumList { get; set; }
    }
}