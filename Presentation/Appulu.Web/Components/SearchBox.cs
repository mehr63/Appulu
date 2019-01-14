using Microsoft.AspNetCore.Mvc;
using Appulu.Web.Factories;
using Appulu.Web.Framework.Components;

namespace Appulu.Web.Components
{
    public class SearchBoxViewComponent : AppViewComponent
    {
        private readonly ICatalogModelFactory _catalogModelFactory;

        public SearchBoxViewComponent(ICatalogModelFactory catalogModelFactory)
        {
            this._catalogModelFactory = catalogModelFactory;
        }

        public IViewComponentResult Invoke()
        {
            var model = _catalogModelFactory.PrepareSearchBoxModel();
            return View(model);
        }
    }
}
