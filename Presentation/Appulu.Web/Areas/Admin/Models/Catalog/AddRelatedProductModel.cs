using System.Collections.Generic;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Catalog
{
    /// <summary>
    /// Represents a related product model to add to the product
    /// </summary>
    public partial class AddRelatedProductModel : BaseAppModel
    {
        #region Ctor

        public AddRelatedProductModel()
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