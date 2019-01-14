using Microsoft.AspNetCore.Mvc;
using Appulu.Web.Factories;
using Appulu.Web.Framework.Components;

namespace Appulu.Web.Components
{
    public class CurrencySelectorViewComponent : AppViewComponent
    {
        private readonly ICommonModelFactory _commonModelFactory;

        public CurrencySelectorViewComponent(ICommonModelFactory commonModelFactory)
        {
            this._commonModelFactory = commonModelFactory;
        }

        public IViewComponentResult Invoke()
        {
            var model = _commonModelFactory.PrepareCurrencySelectorModel();
            if (model.AvailableCurrencies.Count == 1)
                return Content("");

            return View(model);
        }
    }
}
