using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Catalog
{
    /// <summary>
    /// Represents a product review and review type mapping model
    /// </summary>
    public class ProductReviewReviewTypeMappingModel : BaseAppEntityModel
    {
        #region Properties

        public int ProductReviewId { get; set; }

        [AppResourceDisplayName("Admin.Catalog.ProductReviewsExt.Fields.Name")]
        public string Name { get; set; }

        [AppResourceDisplayName("Admin.Catalog.ProductReviewsExt.Fields.Description")]
        public string Description { get; set; }

        [AppResourceDisplayName("Admin.Catalog.ProductReviewsExt.Fields.VisibleToAllUsers")]
        public bool VisibleToAllUsers { get; set; }

        [AppResourceDisplayName("Admin.Catalog.ProductReviewsExt.Fields.Rating")]
        public int Rating { get; set; }

        #endregion
    }
}
