using Appulu.Web.Framework.Models;

namespace Appulu.Web.Models.Common
{
    public partial class CurrencyModel : BaseAppEntityModel
    {
        public string Name { get; set; }

        public string CurrencySymbol { get; set; }
    }
}