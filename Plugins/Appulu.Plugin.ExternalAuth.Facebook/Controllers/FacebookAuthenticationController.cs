﻿using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Appulu.Core;
using Appulu.Plugin.ExternalAuth.Facebook.Models;
using Appulu.Services.Authentication.External;
using Appulu.Services.Configuration;
using Appulu.Services.Localization;
using Appulu.Services.Security;
using Appulu.Web.Framework;
using Appulu.Web.Framework.Controllers;
using Appulu.Web.Framework.Mvc.Filters;

namespace Appulu.Plugin.ExternalAuth.Facebook.Controllers
{
    public class FacebookAuthenticationController : BasePluginController
    {
        #region Fields

        private readonly FacebookExternalAuthSettings _facebookExternalAuthSettings;
        private readonly IExternalAuthenticationService _externalAuthenticationService;
        private readonly ILocalizationService _localizationService;
        private readonly IOptionsMonitorCache<FacebookOptions> _optionsCache;
        private readonly IPermissionService _permissionService;
        private readonly ISettingService _settingService;

        #endregion

        #region Ctor

        public FacebookAuthenticationController(FacebookExternalAuthSettings facebookExternalAuthSettings,
            IExternalAuthenticationService externalAuthenticationService,
            ILocalizationService localizationService,
            IOptionsMonitorCache<FacebookOptions> optionsCache,
            IPermissionService permissionService,
            ISettingService settingService)
        {
            this._facebookExternalAuthSettings = facebookExternalAuthSettings;
            this._externalAuthenticationService = externalAuthenticationService;
            this._localizationService = localizationService;
            this._optionsCache = optionsCache;
            this._permissionService = permissionService;
            this._settingService = settingService;
        }

        #endregion

        #region Methods

        [AuthorizeAdmin]
        [Area(AreaNames.Admin)]
        public IActionResult Configure()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageExternalAuthenticationMethods))
                return AccessDeniedView();

            var model = new ConfigurationModel
            {
                ClientId = _facebookExternalAuthSettings.ClientKeyIdentifier,
                ClientSecret = _facebookExternalAuthSettings.ClientSecret
            };

            return View("~/Plugins/ExternalAuth.Facebook/Views/Configure.cshtml", model);
        }

        [HttpPost]
        [AdminAntiForgery]
        [AuthorizeAdmin]
        [Area(AreaNames.Admin)]
        public IActionResult Configure(ConfigurationModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageExternalAuthenticationMethods))
                return AccessDeniedView();

            if (!ModelState.IsValid)
                return Configure();

            //save settings
            _facebookExternalAuthSettings.ClientKeyIdentifier = model.ClientId;
            _facebookExternalAuthSettings.ClientSecret = model.ClientSecret;
            _settingService.SaveSetting(_facebookExternalAuthSettings);

            //clear Facebook authentication options cache
            _optionsCache.TryRemove(FacebookDefaults.AuthenticationScheme);

            SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

            return Configure();
        }

        public IActionResult Login(string returnUrl)
        {
            if (!_externalAuthenticationService.ExternalAuthenticationMethodIsAvailable(FacebookAuthenticationDefaults.ProviderSystemName))
                throw new AppException("Facebook authentication module cannot be loaded");

            if (string.IsNullOrEmpty(_facebookExternalAuthSettings.ClientKeyIdentifier) || string.IsNullOrEmpty(_facebookExternalAuthSettings.ClientSecret))
                throw new AppException("Facebook authentication module not configured");

            //configure login callback action
            var authenticationProperties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("LoginCallback", "FacebookAuthentication", new { returnUrl = returnUrl })
            };

            return Challenge(authenticationProperties, FacebookDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> LoginCallback(string returnUrl)
        {
            //authenticate Facebook user
            var authenticateResult =  await this.HttpContext.AuthenticateAsync(FacebookDefaults.AuthenticationScheme);
            if (!authenticateResult.Succeeded || !authenticateResult.Principal.Claims.Any())
                return RedirectToRoute("Login");

            //create external authentication parameters
            var authenticationParameters = new ExternalAuthenticationParameters
            {
                ProviderSystemName = FacebookAuthenticationDefaults.ProviderSystemName,
                AccessToken = await this.HttpContext.GetTokenAsync(FacebookDefaults.AuthenticationScheme, "access_token"),
                Email = authenticateResult.Principal.FindFirst(claim => claim.Type == ClaimTypes.Email)?.Value,
                ExternalIdentifier = authenticateResult.Principal.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier)?.Value,
                ExternalDisplayIdentifier = authenticateResult.Principal.FindFirst(claim => claim.Type == ClaimTypes.Name)?.Value,
                Claims = authenticateResult.Principal.Claims.Select(claim => new ExternalAuthenticationClaim(claim.Type, claim.Value)).ToList()
            };

            //authenticate App user
            return _externalAuthenticationService.Authenticate(authenticationParameters, returnUrl);
        }

        #endregion
    }
}