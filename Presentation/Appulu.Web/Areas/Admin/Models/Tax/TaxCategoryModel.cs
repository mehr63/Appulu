using FluentValidation.Attributes;
using Appulu.Web.Areas.Admin.Validators.Tax;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Tax
{
    /// <summary>
    /// Represents a tax category model
    /// </summary>
    [Validator(typeof(TaxCategoryValidator))]
    public partial class TaxCategoryModel : BaseAppEntityModel
    {
        #region Properties

        [AppResourceDisplayName("Admin.Configuration.Tax.Categories.Fields.Name")]
        public string Name { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Tax.Categories.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        #endregion
    }
}