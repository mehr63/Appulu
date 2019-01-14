using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Appulu.Web.Factories;
using Appulu.Web.Framework.Components;

namespace Appulu.Web.Components
{
    public class HomepageCategoriesViewComponent : AppViewComponent
    {
        private readonly ICatalogModelFactory _catalogModelFactory;

        public HomepageCategoriesViewComponent(ICatalogModelFactory catalogModelFactory)
        {
            this._catalogModelFactory = catalogModelFactory;
        }

        public IViewComponentResult Invoke()
        {
            var model = _catalogModelFactory.PrepareHomepageCategoryModels();
            if (!model.Any())
                return Content("");

            return View(model);
        }
    }
}
