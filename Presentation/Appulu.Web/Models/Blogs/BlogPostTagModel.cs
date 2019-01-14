using Appulu.Web.Framework.Models;

namespace Appulu.Web.Models.Blogs
{
    public partial class BlogPostTagModel : BaseAppModel
    {
        public string Name { get; set; }

        public int BlogPostCount { get; set; }
    }
}