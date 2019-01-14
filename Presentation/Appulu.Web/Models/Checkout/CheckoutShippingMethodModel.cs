using System.Collections.Generic;
using Appulu.Core.Domain.Shipping;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Models.Checkout
{
    public partial class CheckoutShippingMethodModel : BaseAppModel
    {
        public CheckoutShippingMethodModel()
        {
            ShippingMethods = new List<ShippingMethodModel>();
            Warnings = new List<string>();
        }

        public IList<ShippingMethodModel> ShippingMethods { get; set; }

        public bool NotifyUserAboutShippingFromMultipleLocations { get; set; }

        public IList<string> Warnings { get; set; }

        #region Nested classes

        public partial class ShippingMethodModel : BaseAppModel
        {
            public string ShippingRateComputationMethodSystemName { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Fee { get; set; }
            public bool Selected { get; set; }

            public ShippingOption ShippingOption { get; set; } 
        }

        #endregion
    }
}