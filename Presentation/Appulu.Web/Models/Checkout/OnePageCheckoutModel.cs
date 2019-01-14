using Appulu.Web.Framework.Models;

namespace Appulu.Web.Models.Checkout
{
    public partial class OnePageCheckoutModel : BaseAppModel
    {
        public bool ShippingRequired { get; set; }
        public bool DisableBillingAddressCheckoutStep { get; set; }

        public CheckoutBillingAddressModel BillingAddress { get; set; }
    }
}