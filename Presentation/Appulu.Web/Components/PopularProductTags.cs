using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Appulu.Web.Factories;
using Appulu.Web.Framework.Components;

namespace Appulu.Web.Components
{
    public class PopularProductTagsViewComponent : AppViewComponent
    {
        private readonly ICatalogModelFactory _catalogModelFactory;

        public PopularProductTagsViewComponent(ICatalogModelFactory catalogModelFactory)
        {
            this._catalogModelFactory = catalogModelFactory;
        }

        public IViewComponentResult Invoke()
        {
            var model = _catalogModelFactory.PreparePopularProductTagsModel();

            if (!model.Tags.Any())
                return Content("");

            return View(model);
        }
    }
}
