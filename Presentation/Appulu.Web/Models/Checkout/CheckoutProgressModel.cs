using Appulu.Web.Framework.Models;

namespace Appulu.Web.Models.Checkout
{
    public partial class CheckoutProgressModel : BaseAppModel
    {
        public CheckoutProgressStep CheckoutProgressStep { get; set; }
    }

    public enum CheckoutProgressStep
    {
        Cart,
        Address,
        Shipping,
        Payment,
        Confirm,
        Complete
    }
}