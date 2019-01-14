using Microsoft.AspNetCore.Mvc;
using Appulu.Core;
using Appulu.Plugin.Widgets.GoogleAnalytics.Models;
using Appulu.Services.Configuration;
using Appulu.Services.Localization;
using Appulu.Services.Security;
using Appulu.Web.Framework;
using Appulu.Web.Framework.Controllers;
using Appulu.Web.Framework.Mvc.Filters;

namespace Appulu.Plugin.Widgets.GoogleAnalytics.Controllers
{
    [Area(AreaNames.Admin)]
    [AuthorizeAdmin]
    [AdminAntiForgery]
    public class WidgetsGoogleAnalyticsController : BasePluginController
    {
        #region Fields
        
        private readonly IStoreContext _storeContext;
        private readonly ISettingService _settingService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;

        #endregion

        #region Ctor

        public WidgetsGoogleAnalyticsController(IStoreContext storeContext, 
            ISettingService settingService, 
            ILocalizationService localizationService,
            IPermissionService permissionService)
        {
            this._storeContext = storeContext;
            this._settingService = settingService;
            this._localizationService = localizationService;
            this._permissionService = permissionService;
        }

        #endregion

        #region Methods
        
        public IActionResult Configure()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = _storeContext.ActiveStoreScopeConfiguration;
            var googleAnalyticsSettings = _settingService.LoadSetting<GoogleAnalyticsSettings>(storeScope);

            var model = new ConfigurationModel
            {
                GoogleId = googleAnalyticsSettings.GoogleId,
                TrackingScript = googleAnalyticsSettings.TrackingScript,
                EnableEcommerce = googleAnalyticsSettings.EnableEcommerce,
                UseJsToSendEcommerceInfo = googleAnalyticsSettings.UseJsToSendEcommerceInfo,
                IncludingTax = googleAnalyticsSettings.IncludingTax,
                IncludeUserId = googleAnalyticsSettings.IncludeUserId,
                ActiveStoreScopeConfiguration = storeScope
            };

            if (storeScope > 0)
            {
                model.GoogleId_OverrideForStore = _settingService.SettingExists(googleAnalyticsSettings, x => x.GoogleId, storeScope);
                model.TrackingScript_OverrideForStore = _settingService.SettingExists(googleAnalyticsSettings, x => x.TrackingScript, storeScope);
                model.EnableEcommerce_OverrideForStore = _settingService.SettingExists(googleAnalyticsSettings, x => x.EnableEcommerce, storeScope);
                model.UseJsToSendEcommerceInfo_OverrideForStore = _settingService.SettingExists(googleAnalyticsSettings, x => x.UseJsToSendEcommerceInfo, storeScope);
                model.IncludingTax_OverrideForStore = _settingService.SettingExists(googleAnalyticsSettings, x => x.IncludingTax, storeScope);
                model.IncludeUserId_OverrideForStore = _settingService.SettingExists(googleAnalyticsSettings, x => x.IncludeUserId, storeScope);
            }

            return View("~/Plugins/Widgets.GoogleAnalytics/Views/Configure.cshtml", model);
        }

        [HttpPost]
        public IActionResult Configure(ConfigurationModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = _storeContext.ActiveStoreScopeConfiguration;
            var googleAnalyticsSettings = _settingService.LoadSetting<GoogleAnalyticsSettings>(storeScope);

            googleAnalyticsSettings.GoogleId = model.GoogleId;
            googleAnalyticsSettings.TrackingScript = model.TrackingScript;
            googleAnalyticsSettings.EnableEcommerce = model.EnableEcommerce;
            googleAnalyticsSettings.UseJsToSendEcommerceInfo = model.UseJsToSendEcommerceInfo;
            googleAnalyticsSettings.IncludingTax = model.IncludingTax;
            googleAnalyticsSettings.IncludeUserId = model.IncludeUserId;

            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            _settingService.SaveSettingOverridablePerStore(googleAnalyticsSettings, x => x.GoogleId, model.GoogleId_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(googleAnalyticsSettings, x => x.TrackingScript, model.TrackingScript_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(googleAnalyticsSettings, x => x.EnableEcommerce, model.EnableEcommerce_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(googleAnalyticsSettings, x => x.UseJsToSendEcommerceInfo, model.UseJsToSendEcommerceInfo_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(googleAnalyticsSettings, x => x.IncludingTax, model.IncludingTax_OverrideForStore, storeScope, false);
            _settingService.SaveSettingOverridablePerStore(googleAnalyticsSettings, x => x.IncludeUserId, model.IncludeUserId_OverrideForStore, storeScope, false);

            //now clear settings cache
            _settingService.ClearCache();

            SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

            return Configure();
        }

        #endregion
    }
}