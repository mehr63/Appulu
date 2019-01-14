using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Catalog
{
    /// <summary>
    /// Represents a model of products that use the specification attribute
    /// </summary>
    public partial class SpecificationAttributeProductModel : BaseAppEntityModel
    {
        #region Properties

        public int SpecificationAttributeId { get; set; }

        public int ProductId { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Attributes.SpecificationAttributes.UsedByProducts.Product")]
        public string ProductName { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Attributes.SpecificationAttributes.UsedByProducts.Published")]
        public bool Published { get; set; }

        #endregion
    }
}