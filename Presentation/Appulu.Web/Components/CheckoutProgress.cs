using Microsoft.AspNetCore.Mvc;
using Appulu.Web.Factories;
using Appulu.Web.Framework.Components;
using Appulu.Web.Models.Checkout;

namespace Appulu.Web.Components
{
    public class CheckoutProgressViewComponent : AppViewComponent
    {
        private readonly ICheckoutModelFactory _checkoutModelFactory;

        public CheckoutProgressViewComponent(ICheckoutModelFactory checkoutModelFactory)
        {
            this._checkoutModelFactory = checkoutModelFactory;
        }

        public IViewComponentResult Invoke(CheckoutProgressStep step)
        {
            var model = _checkoutModelFactory.PrepareCheckoutProgressModel(step);
            return View(model);
        }
    }
}
