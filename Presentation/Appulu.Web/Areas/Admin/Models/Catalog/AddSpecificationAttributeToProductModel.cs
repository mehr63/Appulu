using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Catalog
{
    /// <summary>
    /// Represents a specification attribute model to add to the product
    /// </summary>
    public partial class AddSpecificationAttributeToProductModel : BaseAppModel
    {
        #region Ctor

        public AddSpecificationAttributeToProductModel()
        {
            AvailableAttributes = new List<SelectListItem>();
            AvailableOptions = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [AppResourceDisplayName("Admin.Catalog.Products.SpecificationAttributes.Fields.SpecificationAttribute")]
        public int SpecificationAttributeId { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Products.SpecificationAttributes.Fields.AttributeType")]
        public int AttributeTypeId { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Products.SpecificationAttributes.Fields.SpecificationAttributeOption")]
        public int SpecificationAttributeOptionId { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Products.SpecificationAttributes.Fields.CustomValue")]
        public string CustomValue { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Products.SpecificationAttributes.Fields.AllowFiltering")]
        public bool AllowFiltering { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Products.SpecificationAttributes.Fields.ShowOnProductPage")]
        public bool ShowOnProductPage { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Products.SpecificationAttributes.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        public IList<SelectListItem> AvailableAttributes { get; set; }

        public IList<SelectListItem> AvailableOptions { get; set; }

        #endregion
    }
}