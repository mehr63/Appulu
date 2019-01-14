using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Catalog
{
    /// <summary>
    /// Represents a category product model
    /// </summary>
    public partial class CategoryProductModel : BaseAppEntityModel
    {
        #region Properties

        public int CategoryId { get; set; }

        public int ProductId { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Categories.Products.Fields.Product")]
        public string ProductName { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Categories.Products.Fields.IsFeaturedProduct")]
        public bool IsFeaturedProduct { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Categories.Products.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        #endregion
    }
}