using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Catalog
{
    /// <summary>
    /// Represents a model of products that use the product attribute
    /// </summary>
    public partial class ProductAttributeProductModel : BaseAppEntityModel
    {
        #region Properties

        [AppResourceDisplayName("Admin.Catalog.Attributes.ProductAttributes.UsedByProducts.Product")]
        public string ProductName { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Attributes.ProductAttributes.UsedByProducts.Published")]
        public bool Published { get; set; }

        #endregion
    }
}