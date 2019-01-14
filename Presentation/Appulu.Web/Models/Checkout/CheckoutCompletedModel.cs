using Appulu.Web.Framework.Models;

namespace Appulu.Web.Models.Checkout
{
    public partial class CheckoutCompletedModel : BaseAppModel
    {
        public int OrderId { get; set; }
        public string CustomOrderNumber { get; set; }
        public bool OnePageCheckoutEnabled { get; set; }
    }
}