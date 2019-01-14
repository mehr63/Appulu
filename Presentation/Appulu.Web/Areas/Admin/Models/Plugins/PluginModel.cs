using System.Collections.Generic;
using FluentValidation.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;
using Appulu.Web.Areas.Admin.Validators.Plugins;
using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Plugins
{
    /// <summary>
    /// Represents a plugin model
    /// </summary>
    [Validator(typeof(PluginValidator))]
    public partial class PluginModel : BaseAppModel, IAclSupportedModel, ILocalizedModel<PluginLocalizedModel>, IPluginModel, IStoreMappingSupportedModel
    {
        #region Ctor

        public PluginModel()
        {
            Locales = new List<PluginLocalizedModel>();

            SelectedStoreIds = new List<int>();
            AvailableStores = new List<SelectListItem>();
            SelectedUserRoleIds = new List<int>();
            AvailableUserRoles = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [AppResourceDisplayName("Admin.Configuration.Plugins.Fields.Group")]
        public string Group { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Plugins.Fields.FriendlyName")]
        public string FriendlyName { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Plugins.Fields.SystemName")]
        public string SystemName { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Plugins.Fields.Version")]
        public string Version { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Plugins.Fields.Author")]
        public string Author { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Plugins.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Plugins.Fields.Configure")]
        public string ConfigurationUrl { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Plugins.Fields.Installed")]
        public bool Installed { get; set; }
        
        public string Description { get; set; }

        public bool CanChangeEnabled { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Plugins.Fields.IsEnabled")]
        public bool IsEnabled { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Plugins.Fields.Logo")]
        public string LogoUrl { get; set; }

        public IList<PluginLocalizedModel> Locales { get; set; }

        //ACL (user roles)
        [AppResourceDisplayName("Admin.Configuration.Plugins.Fields.AclUserRoles")]
        public IList<int> SelectedUserRoleIds { get; set; }

        public IList<SelectListItem> AvailableUserRoles { get; set; }

        //store mapping
        [AppResourceDisplayName("Admin.Configuration.Plugins.Fields.LimitedToStores")]
        public IList<int> SelectedStoreIds { get; set; }

        public IList<SelectListItem> AvailableStores { get; set; }

        public bool IsActive { get; set; }

        #endregion
    }

    public partial class PluginLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Plugins.Fields.FriendlyName")]
        public string FriendlyName { get; set; }
    }
}