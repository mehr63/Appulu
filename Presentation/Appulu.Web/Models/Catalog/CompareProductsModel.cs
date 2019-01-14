using System.Collections.Generic;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Models.Catalog
{
    public partial class CompareProductsModel : BaseAppEntityModel
    {
        public CompareProductsModel()
        {
            Products = new List<ProductOverviewModel>();
        }
        public IList<ProductOverviewModel> Products { get; set; }

        public bool IncludeShortDescriptionInCompareProducts { get; set; }
        public bool IncludeFullDescriptionInCompareProducts { get; set; }
    }
}