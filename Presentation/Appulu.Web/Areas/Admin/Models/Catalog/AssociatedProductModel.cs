using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Catalog
{
    /// <summary>
    /// Represents an associated product model
    /// </summary>
    public partial class AssociatedProductModel : BaseAppEntityModel
    {
        #region Properties

        [AppResourceDisplayName("Admin.Catalog.Products.AssociatedProducts.Fields.Product")]
        public string ProductName { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Products.AssociatedProducts.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        #endregion
    }
}