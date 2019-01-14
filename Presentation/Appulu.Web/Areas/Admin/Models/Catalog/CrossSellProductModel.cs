using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Catalog
{
    /// <summary>
    /// Represents a cross-sell product model
    /// </summary>
    public partial class CrossSellProductModel : BaseAppEntityModel
    {
        #region Properties

        public int ProductId2 { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Products.CrossSells.Fields.Product")]
        public string Product2Name { get; set; }

        #endregion
    }
}