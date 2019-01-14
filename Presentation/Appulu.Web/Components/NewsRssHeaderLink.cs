using Microsoft.AspNetCore.Mvc;
using Appulu.Core.Domain.News;
using Appulu.Web.Framework.Components;

namespace Appulu.Web.Components
{
    public class NewsRssHeaderLinkViewComponent : AppViewComponent
    {
        private readonly NewsSettings _newsSettings;

        public NewsRssHeaderLinkViewComponent(NewsSettings newsSettings)
        {
            this._newsSettings = newsSettings;
        }

        public IViewComponentResult Invoke(int currentCategoryId, int currentProductId)
        {
            if (!_newsSettings.Enabled || !_newsSettings.ShowHeaderRssUrl)
                return Content("");

            return View();
        }
    }
}
