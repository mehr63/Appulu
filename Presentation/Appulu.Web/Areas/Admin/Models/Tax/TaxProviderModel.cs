using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Tax
{
    /// <summary>
    /// Represents a tax provider model
    /// </summary>
    public partial class TaxProviderModel : BaseAppModel, IPluginModel
    {
        #region Properties

        [AppResourceDisplayName("Admin.Configuration.Tax.Providers.Fields.FriendlyName")]
        public string FriendlyName { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Tax.Providers.Fields.SystemName")]
        public string SystemName { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Tax.Providers.Fields.IsPrimaryTaxProvider")]
        public bool IsPrimaryTaxProvider { get; set; }
        
        [AppResourceDisplayName("Admin.Configuration.Tax.Providers.Configure")]
        public string ConfigurationUrl { get; set; }

        public string LogoUrl { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; }

        #endregion
    }
}