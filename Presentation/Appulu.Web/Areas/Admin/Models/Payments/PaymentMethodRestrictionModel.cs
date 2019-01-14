using System.Collections.Generic;
using Appulu.Web.Areas.Admin.Models.Directory;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Payments
{
    /// <summary>
    /// Represents a payment method restriction model
    /// </summary>
    public partial class PaymentMethodRestrictionModel : BaseAppModel
    {
        #region Ctor

        public PaymentMethodRestrictionModel()
        {
            AvailablePaymentMethods = new List<PaymentMethodModel>();
            AvailableCountries = new List<CountryModel>();
            Resticted = new Dictionary<string, IDictionary<int, bool>>();
        }

        #endregion

        #region Properties

        public IList<PaymentMethodModel> AvailablePaymentMethods { get; set; }

        public IList<CountryModel> AvailableCountries { get; set; }

        //[payment method system name] / [user role id] / [restricted]
        public IDictionary<string, IDictionary<int, bool>> Resticted { get; set; }

        #endregion
    }
}