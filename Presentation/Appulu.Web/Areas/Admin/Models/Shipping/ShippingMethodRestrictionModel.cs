using System.Collections.Generic;
using Appulu.Web.Areas.Admin.Models.Directory;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Shipping
{
    /// <summary>
    /// Represents a shipping method restriction model
    /// </summary>
    public partial class ShippingMethodRestrictionModel : BaseAppModel
    {
        #region Ctor

        public ShippingMethodRestrictionModel()
        {
            AvailableShippingMethods = new List<ShippingMethodModel>();
            AvailableCountries = new List<CountryModel>();
            Restricted = new Dictionary<int, IDictionary<int, bool>>();
        }

        #endregion

        #region Properties

        public IList<ShippingMethodModel> AvailableShippingMethods { get; set; }

        public IList<CountryModel> AvailableCountries { get; set; }

        //[country id] / [shipping method id] / [restricted]
        public IDictionary<int, IDictionary<int, bool>> Restricted { get; set; }

        #endregion
    }
}