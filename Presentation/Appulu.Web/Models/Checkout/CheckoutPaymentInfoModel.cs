using Appulu.Web.Framework.Models;

namespace Appulu.Web.Models.Checkout
{
    public partial class CheckoutPaymentInfoModel : BaseAppModel
    {
        public string PaymentViewComponentName { get; set; }

        /// <summary>
        /// Used on one-page checkout page
        /// </summary>
        public bool DisplayOrderTotals { get; set; }
    }
}