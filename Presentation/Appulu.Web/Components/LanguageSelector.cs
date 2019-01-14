using Microsoft.AspNetCore.Mvc;
using Appulu.Web.Factories;
using Appulu.Web.Framework.Components;

namespace Appulu.Web.Components
{
    public class LanguageSelectorViewComponent : AppViewComponent
    {
        private readonly ICommonModelFactory _commonModelFactory;

        public LanguageSelectorViewComponent(ICommonModelFactory commonModelFactory)
        {
            this._commonModelFactory = commonModelFactory;
        }

        public IViewComponentResult Invoke()
        {
            var model = _commonModelFactory.PrepareLanguageSelectorModel();

            if (model.AvailableLanguages.Count == 1)
                return Content("");

            return View(model);
        }
    }
}
