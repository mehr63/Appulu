using Microsoft.AspNetCore.Mvc;
using Appulu.Core.Domain.Blogs;
using Appulu.Web.Framework.Components;

namespace Appulu.Web.Components
{
    public class BlogRssHeaderLinkViewComponent : AppViewComponent
    {
        private readonly BlogSettings _blogSettings;

        public BlogRssHeaderLinkViewComponent(BlogSettings blogSettings)
        {
            this._blogSettings = blogSettings;
        }

        public IViewComponentResult Invoke(int currentCategoryId, int currentProductId)
        {
            if (!_blogSettings.Enabled || !_blogSettings.ShowHeaderRssUrl)
                return Content("");

            return View();
        }
    }
}
