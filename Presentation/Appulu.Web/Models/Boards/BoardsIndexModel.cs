using System.Collections.Generic;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Models.Boards
{
    public partial class BoardsIndexModel : BaseAppModel
    {
        public BoardsIndexModel()
        {
            this.ForumGroups = new List<ForumGroupModel>();
        }
        
        public IList<ForumGroupModel> ForumGroups { get; set; }
    }
}