using FluentValidation.Attributes;
using Appulu.Web.Areas.Admin.Validators.Settings;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Settings
{
    /// <summary>
    /// Represents a setting model
    /// </summary>
    [Validator(typeof(SettingValidator))]
    public partial class SettingModel : BaseAppEntityModel
    {
        #region Properties

        [AppResourceDisplayName("Admin.Configuration.Settings.AllSettings.Fields.Name")]
        public string Name { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.AllSettings.Fields.Value")]
        public string Value { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.AllSettings.Fields.StoreName")]
        public string Store { get; set; }

        public int StoreId { get; set; }

        #endregion
    }
}