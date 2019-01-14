using FluentValidation.Attributes;
using Appulu.Web.Areas.Admin.Validators.Templates;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Templates
{
    /// <summary>
    /// Represents a manufacturer template model
    /// </summary>
    [Validator(typeof(ManufacturerTemplateValidator))]
    public partial class ManufacturerTemplateModel : BaseAppEntityModel
    {
        #region Properties

        [AppResourceDisplayName("Admin.System.Templates.Manufacturer.Name")]
        public string Name { get; set; }

        [AppResourceDisplayName("Admin.System.Templates.Manufacturer.ViewPath")]
        public string ViewPath { get; set; }

        [AppResourceDisplayName("Admin.System.Templates.Manufacturer.DisplayOrder")]
        public int DisplayOrder { get; set; }

        #endregion
    }
}