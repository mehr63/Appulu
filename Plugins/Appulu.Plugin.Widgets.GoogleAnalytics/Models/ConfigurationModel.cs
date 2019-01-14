using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Plugin.Widgets.GoogleAnalytics.Models
{
    public class ConfigurationModel : BaseAppModel
    {
        public int ActiveStoreScopeConfiguration { get; set; }
        
        [AppResourceDisplayName("Plugins.Widgets.GoogleAnalytics.GoogleId")]
        public string GoogleId { get; set; }
        public bool GoogleId_OverrideForStore { get; set; }

        [AppResourceDisplayName("Plugins.Widgets.GoogleAnalytics.EnableEcommerce")]
        public bool EnableEcommerce { get; set; }
        public bool EnableEcommerce_OverrideForStore { get; set; }

        [AppResourceDisplayName("Plugins.Widgets.GoogleAnalytics.UseJsToSendEcommerceInfo")]
        public bool UseJsToSendEcommerceInfo { get; set; }
        public bool UseJsToSendEcommerceInfo_OverrideForStore { get; set; }

        [AppResourceDisplayName("Plugins.Widgets.GoogleAnalytics.TrackingScript")]
        public string TrackingScript { get; set; }
        public bool TrackingScript_OverrideForStore { get; set; }
        
        [AppResourceDisplayName("Plugins.Widgets.GoogleAnalytics.IncludingTax")]
        public bool IncludingTax { get; set; }
        public bool IncludingTax_OverrideForStore { get; set; }

        [AppResourceDisplayName("Plugins.Widgets.GoogleAnalytics.IncludeUserId")]
        public bool IncludeUserId { get; set; }
        public bool IncludeUserId_OverrideForStore { get; set; }
    }
}