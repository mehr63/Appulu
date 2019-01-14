using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Appulu.Core.Domain.Vendors;
using Appulu.Web.Factories;
using Appulu.Web.Framework.Components;

namespace Appulu.Web.Components
{
    public class VendorNavigationViewComponent : AppViewComponent
    {
        private readonly ICatalogModelFactory _catalogModelFactory;
        private readonly VendorSettings _vendorSettings;

        public VendorNavigationViewComponent(ICatalogModelFactory catalogModelFactory,
            VendorSettings vendorSettings)
        {
            this._catalogModelFactory = catalogModelFactory;
            this._vendorSettings = vendorSettings;
        }

        public IViewComponentResult Invoke()
        {
            if (_vendorSettings.VendorsBlockItemsToDisplay == 0)
                return Content("");

            var model = _catalogModelFactory.PrepareVendorNavigationModel();
            if (!model.Vendors.Any())
                return Content("");

            return View(model);
        }
    }
}
