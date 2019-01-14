using System.Collections.Generic;
using FluentValidation.Attributes;
using Appulu.Web.Areas.Admin.Validators.Catalog;
using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Catalog
{
    /// <summary>
    /// Represents a specification attribute model
    /// </summary>
    [Validator(typeof(SpecificationAttributeValidator))]
    public partial class SpecificationAttributeModel : BaseAppEntityModel, ILocalizedModel<SpecificationAttributeLocalizedModel>
    {
        #region Ctor

        public SpecificationAttributeModel()
        {
            Locales = new List<SpecificationAttributeLocalizedModel>();
            SpecificationAttributeOptionSearchModel = new SpecificationAttributeOptionSearchModel();
            SpecificationAttributeProductSearchModel = new SpecificationAttributeProductSearchModel();
        }

        #endregion

        #region Properties

        [AppResourceDisplayName("Admin.Catalog.Attributes.SpecificationAttributes.Fields.Name")]
        public string Name { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Attributes.SpecificationAttributes.Fields.DisplayOrder")]
        public int DisplayOrder {get;set;}

        public IList<SpecificationAttributeLocalizedModel> Locales { get; set; }

        public SpecificationAttributeOptionSearchModel SpecificationAttributeOptionSearchModel { get; set; }

        public SpecificationAttributeProductSearchModel SpecificationAttributeProductSearchModel { get; set; }

        #endregion
    }

    public partial class SpecificationAttributeLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Attributes.SpecificationAttributes.Fields.Name")]
        public string Name { get; set; }
    }
}