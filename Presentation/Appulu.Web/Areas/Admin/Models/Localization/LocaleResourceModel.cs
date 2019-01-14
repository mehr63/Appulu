using FluentValidation.Attributes;
using Appulu.Web.Areas.Admin.Validators.Localization;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Localization
{
    /// <summary>
    /// Represents a locale resource model
    /// </summary>
    [Validator(typeof(LanguageResourceValidator))]
    public partial class LocaleResourceModel : BaseAppEntityModel
    {
        #region Properties

        [AppResourceDisplayName("Admin.Configuration.Languages.Resources.Fields.Name")]
        public string Name { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Languages.Resources.Fields.Value")]
        public string Value { get; set; }

        public int LanguageId { get; set; }

        #endregion
    }
}