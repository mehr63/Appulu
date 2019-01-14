using System.Collections.Generic;
using Appulu.Web.Framework.Models;
using Appulu.Web.Models.Common;

namespace Appulu.Web.Models.Profile
{
    public partial class ProfilePostsModel : BaseAppModel
    {
        public IList<PostsModel> Posts { get; set; }
        public PagerModel PagerModel { get; set; }
    }
}