using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Catalog
{
    /// <summary>
    /// Represents a related product model
    /// </summary>
    public partial class RelatedProductModel : BaseAppEntityModel
    {
        #region Properties

        public int ProductId2 { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Products.RelatedProducts.Fields.Product")]
        public string Product2Name { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Products.RelatedProducts.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        #endregion
    }
}