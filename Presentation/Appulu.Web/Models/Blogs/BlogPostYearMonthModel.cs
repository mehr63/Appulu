using System.Collections.Generic;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Models.Blogs
{
    public partial class BlogPostYearModel : BaseAppModel
    {
        public BlogPostYearModel()
        {
            Months = new List<BlogPostMonthModel>();
        }
        public int Year { get; set; }
        public IList<BlogPostMonthModel> Months { get; set; }
    }

    public partial class BlogPostMonthModel : BaseAppModel
    {
        public int Month { get; set; }

        public int BlogPostCount { get; set; }
    }
}