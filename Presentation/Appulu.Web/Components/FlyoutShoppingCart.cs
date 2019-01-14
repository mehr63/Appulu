using Microsoft.AspNetCore.Mvc;
using Appulu.Core.Domain.Orders;
using Appulu.Services.Security;
using Appulu.Web.Factories;
using Appulu.Web.Framework.Components;

namespace Appulu.Web.Components
{
    public class FlyoutShoppingCartViewComponent : AppViewComponent
    {
        private readonly IPermissionService _permissionService;
        private readonly IShoppingCartModelFactory _shoppingCartModelFactory;
        private readonly ShoppingCartSettings _shoppingCartSettings;

        public FlyoutShoppingCartViewComponent(IPermissionService permissionService,
            IShoppingCartModelFactory shoppingCartModelFactory,
            ShoppingCartSettings shoppingCartSettings)
        {
            this._permissionService = permissionService;
            this._shoppingCartModelFactory = shoppingCartModelFactory;
            this._shoppingCartSettings = shoppingCartSettings;
        }

        public IViewComponentResult Invoke()
        {
            if (!_shoppingCartSettings.MiniShoppingCartEnabled)
                return Content("");

            if (!_permissionService.Authorize(StandardPermissionProvider.EnableShoppingCart))
                return Content("");

            var model = _shoppingCartModelFactory.PrepareMiniShoppingCartModel();
            return View(model);
        }
    }
}
