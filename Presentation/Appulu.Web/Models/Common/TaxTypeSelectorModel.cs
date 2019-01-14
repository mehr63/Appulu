using Appulu.Core.Domain.Tax;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Models.Common
{
    public partial class TaxTypeSelectorModel : BaseAppModel
    {
        public TaxDisplayType CurrentTaxType { get; set; }
    }
}