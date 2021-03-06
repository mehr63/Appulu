﻿using Microsoft.AspNetCore.Mvc;
using Appulu.Core.Domain.Users;
using Appulu.Core.Plugins;
using Appulu.Services.Authentication.External;
using Appulu.Services.Configuration;
using Appulu.Services.Plugins;
using Appulu.Services.Security;
using Appulu.Web.Areas.Admin.Factories;
using Appulu.Web.Areas.Admin.Models.ExternalAuthentication;
using Appulu.Web.Framework.Mvc;

namespace Appulu.Web.Areas.Admin.Controllers
{
    public partial class ExternalAuthenticationController : BaseAdminController
    {
        #region Fields

        private readonly ExternalAuthenticationSettings _externalAuthenticationSettings;
        private readonly IExternalAuthenticationMethodModelFactory _externalAuthenticationMethodModelFactory;
        private readonly IExternalAuthenticationService _externalAuthenticationService;
        private readonly IPermissionService _permissionService;
        private readonly IPluginFinder _pluginFinder;
        private readonly ISettingService _settingService;

        #endregion

        #region Ctor

        public ExternalAuthenticationController(ExternalAuthenticationSettings externalAuthenticationSettings,
            IExternalAuthenticationMethodModelFactory externalAuthenticationMethodModelFactory,
            IExternalAuthenticationService externalAuthenticationService,
            IPermissionService permissionService,
            IPluginFinder pluginFinder,
            ISettingService settingService)
        {
            this._externalAuthenticationSettings = externalAuthenticationSettings;
            this._externalAuthenticationMethodModelFactory = externalAuthenticationMethodModelFactory;
            this._externalAuthenticationService = externalAuthenticationService;
            this._permissionService = permissionService;
            this._pluginFinder = pluginFinder;
            this._settingService = settingService;
        }

        #endregion

        #region Methods

        public virtual IActionResult Methods()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageExternalAuthenticationMethods))
                return AccessDeniedView();

            //prepare model
            var model = _externalAuthenticationMethodModelFactory
                .PrepareExternalAuthenticationMethodSearchModel(new ExternalAuthenticationMethodSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult Methods(ExternalAuthenticationMethodSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageExternalAuthenticationMethods))
                return AccessDeniedKendoGridJson();

            //prepare model
            var model = _externalAuthenticationMethodModelFactory.PrepareExternalAuthenticationMethodListModel(searchModel);

            return Json(model);
        }

        [HttpPost]
        public virtual IActionResult MethodUpdate(ExternalAuthenticationMethodModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageExternalAuthenticationMethods))
                return AccessDeniedView();

            var eam = _externalAuthenticationService.LoadExternalAuthenticationMethodBySystemName(model.SystemName);
            if (_externalAuthenticationService.IsExternalAuthenticationMethodActive(eam))
            {
                if (!model.IsActive)
                {
                    //mark as disabled
                    _externalAuthenticationSettings.ActiveAuthenticationMethodSystemNames.Remove(eam.PluginDescriptor.SystemName);
                    _settingService.SaveSetting(_externalAuthenticationSettings);
                }
            }
            else
            {
                if (model.IsActive)
                {
                    //mark as active
                    _externalAuthenticationSettings.ActiveAuthenticationMethodSystemNames.Add(eam.PluginDescriptor.SystemName);
                    _settingService.SaveSetting(_externalAuthenticationSettings);
                }
            }

            var pluginDescriptor = eam.PluginDescriptor;
            pluginDescriptor.DisplayOrder = model.DisplayOrder;

            //update the description file
            PluginManager.SavePluginDescriptor(pluginDescriptor);

            //reset plugin cache
            _pluginFinder.ReloadPlugins(pluginDescriptor);

            return new NullJsonResult();
        }

        #endregion
    }
}