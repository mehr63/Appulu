using Microsoft.AspNetCore.Mvc;
using Appulu.Web.Framework.Components;

namespace Appulu.Plugin.Payments.PayPalStandard.Components
{
    [ViewComponent(Name = "PaymentPayPalStandard")]
    public class PaymentPayPalStandardViewComponent : AppViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("~/Plugins/Payments.PayPalStandard/Views/PaymentInfo.cshtml");
        }
    }
}
