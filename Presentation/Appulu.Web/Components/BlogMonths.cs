using Microsoft.AspNetCore.Mvc;
using Appulu.Core.Domain.Blogs;
using Appulu.Web.Factories;
using Appulu.Web.Framework.Components;

namespace Appulu.Web.Components
{
    public class BlogMonthsViewComponent : AppViewComponent
    {
        private readonly BlogSettings _blogSettings;
        private readonly IBlogModelFactory _blogModelFactory;

        public BlogMonthsViewComponent(BlogSettings blogSettings, IBlogModelFactory blogModelFactory)
        {
            this._blogSettings = blogSettings;
            this._blogModelFactory = blogModelFactory;
        }

        public IViewComponentResult Invoke(int currentCategoryId, int currentProductId)
        {
            if (!_blogSettings.Enabled)
                return Content("");

            var model = _blogModelFactory.PrepareBlogPostYearModel();
            return View(model);
        }
    }
}
