using System.Collections.Generic;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Models.Common
{
    public partial class CurrencySelectorModel : BaseAppModel
    {
        public CurrencySelectorModel()
        {
            AvailableCurrencies = new List<CurrencyModel>();
        }

        public IList<CurrencyModel> AvailableCurrencies { get; set; }

        public int CurrentCurrencyId { get; set; }
    }
}