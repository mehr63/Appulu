using Microsoft.AspNetCore.Mvc;
using Appulu.Core.Domain.Tax;
using Appulu.Web.Factories;
using Appulu.Web.Framework.Components;

namespace Appulu.Web.Components
{
    public class TaxTypeSelectorViewComponent : AppViewComponent
    {
        private readonly ICommonModelFactory _commonModelFactory;
        private readonly TaxSettings _taxSettings;

        public TaxTypeSelectorViewComponent(ICommonModelFactory commonModelFactory,
            TaxSettings taxSettings)
        {
            this._commonModelFactory = commonModelFactory;
            this._taxSettings = taxSettings;
        }

        public IViewComponentResult Invoke()
        {
            if (!_taxSettings.AllowUsersToSelectTaxDisplayType)
                return Content("");

            var model = _commonModelFactory.PrepareTaxTypeSelectorModel();
            return View(model);
        }
    }
}
