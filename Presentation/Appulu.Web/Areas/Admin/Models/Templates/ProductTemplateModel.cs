using FluentValidation.Attributes;
using Appulu.Web.Areas.Admin.Validators.Templates;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Templates
{
    /// <summary>
    /// Represents a product template model
    /// </summary>
    [Validator(typeof(ProductTemplateValidator))]
    public partial class ProductTemplateModel : BaseAppEntityModel
    {
        #region Properties

        [AppResourceDisplayName("Admin.System.Templates.Product.Name")]
        public string Name { get; set; }

        [AppResourceDisplayName("Admin.System.Templates.Product.ViewPath")]
        public string ViewPath { get; set; }

        [AppResourceDisplayName("Admin.System.Templates.Product.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [AppResourceDisplayName("Admin.System.Templates.Product.IgnoredProductTypes")]
        public string IgnoredProductTypes { get; set; }

        #endregion
    }
}