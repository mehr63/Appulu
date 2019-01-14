using System.Collections.Generic;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Catalog
{
    /// <summary>
    /// Represents a cross-sell product model to add to the product
    /// </summary>
    public partial class AddCrossSellProductModel : BaseAppModel
    {
        #region Ctor

        public AddCrossSellProductModel()
        {
            SelectedProductIds = new List<int>();
        }
        #endregion

        #region Properties

        public int ProductId { get; set; }

        public IList<int> SelectedProductIds { get; set; }

        #endregion
    }
}