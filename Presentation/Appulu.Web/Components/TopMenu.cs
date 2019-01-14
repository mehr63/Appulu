using Microsoft.AspNetCore.Mvc;
using Appulu.Web.Factories;
using Appulu.Web.Framework.Components;

namespace Appulu.Web.Components
{
    public class TopMenuViewComponent : AppViewComponent
    {
        private readonly ICatalogModelFactory _catalogModelFactory;

        public TopMenuViewComponent(ICatalogModelFactory catalogModelFactory)
        {
            this._catalogModelFactory = catalogModelFactory;
        }

        public IViewComponentResult Invoke(int? productThumbPictureSize)
        {
            var model = _catalogModelFactory.PrepareTopMenuModel();
            return View(model);
        }
    }
}
