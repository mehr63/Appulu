using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Directory
{
    /// <summary>
    /// Represents a currency exchange rate provider model
    /// </summary>
    public partial class CurrencyExchangeRateProviderModel : BaseAppModel
    {
        #region Ctor

        public CurrencyExchangeRateProviderModel()
        {
            ExchangeRates = new List<CurrencyExchangeRateModel>();
            ExchangeRateProviders = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [AppResourceDisplayName("Admin.Configuration.Currencies.Fields.CurrencyRateAutoUpdateEnabled")]
        public bool AutoUpdateEnabled { get; set; }

        public IList<CurrencyExchangeRateModel> ExchangeRates { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Currencies.Fields.ExchangeRateProvider")]
        public string ExchangeRateProvider { get; set; }
        public IList<SelectListItem> ExchangeRateProviders { get; set; }

        #endregion
    }
}