using FluentValidation.Attributes;
using Appulu.Web.Areas.Admin.Validators.Templates;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Templates
{
    /// <summary>
    /// Represents a category template model
    /// </summary>
    [Validator(typeof(CategoryTemplateValidator))]
    public partial class CategoryTemplateModel : BaseAppEntityModel
    {
        #region Properties

        [AppResourceDisplayName("Admin.System.Templates.Category.Name")]
        public string Name { get; set; }

        [AppResourceDisplayName("Admin.System.Templates.Category.ViewPath")]
        public string ViewPath { get; set; }

        [AppResourceDisplayName("Admin.System.Templates.Category.DisplayOrder")]
        public int DisplayOrder { get; set; }

        #endregion
    }
}