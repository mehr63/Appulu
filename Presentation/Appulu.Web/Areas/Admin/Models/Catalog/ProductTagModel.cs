using System.Collections.Generic;
using FluentValidation.Attributes;
using Appulu.Web.Areas.Admin.Validators.Catalog;
using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Catalog
{
    /// <summary>
    /// Represents a product tag model
    /// </summary>
    [Validator(typeof(ProductTagValidator))]
    public partial class ProductTagModel : BaseAppEntityModel, ILocalizedModel<ProductTagLocalizedModel>
    {
        #region Ctor

        public ProductTagModel()
        {
            Locales = new List<ProductTagLocalizedModel>();
        }
        
        #endregion

        #region Properties

        [AppResourceDisplayName("Admin.Catalog.ProductTags.Fields.Name")]
        public string Name { get; set; }

        [AppResourceDisplayName("Admin.Catalog.ProductTags.Fields.ProductCount")]
        public int ProductCount { get; set; }

        public IList<ProductTagLocalizedModel> Locales { get; set; }

        #endregion
    }

    public partial class ProductTagLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [AppResourceDisplayName("Admin.Catalog.ProductTags.Fields.Name")]
        public string Name { get; set; }
    }
}