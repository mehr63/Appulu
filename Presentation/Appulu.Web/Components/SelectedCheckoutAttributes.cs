using Microsoft.AspNetCore.Mvc;
using Appulu.Web.Factories;
using Appulu.Web.Framework.Components;

namespace Appulu.Web.Components
{
    public class SelectedCheckoutAttributesViewComponent : AppViewComponent
    {
        private readonly IShoppingCartModelFactory _shoppingCartModelFactory;

        public SelectedCheckoutAttributesViewComponent(IShoppingCartModelFactory shoppingCartModelFactory)
        {
            this._shoppingCartModelFactory = shoppingCartModelFactory;
        }

        public IViewComponentResult Invoke()
        {
            var attributes = _shoppingCartModelFactory.FormatSelectedCheckoutAttributes();
            return View(null, attributes);
        }
    }
}
