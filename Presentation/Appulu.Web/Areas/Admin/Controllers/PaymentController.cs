using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Appulu.Core.Domain.Payments;
using Appulu.Core.Plugins;
using Appulu.Services.Configuration;
using Appulu.Services.Directory;
using Appulu.Services.Localization;
using Appulu.Services.Payments;
using Appulu.Services.Plugins;
using Appulu.Services.Security;
using Appulu.Web.Areas.Admin.Factories;
using Appulu.Web.Areas.Admin.Models.Payments;
using Appulu.Web.Framework.Mvc;
using Appulu.Web.Framework.Mvc.Filters;

namespace Appulu.Web.Areas.Admin.Controllers
{
    public partial class PaymentController : BaseAdminController
    {
        #region Fields

        private readonly ICountryService _countryService;
        private readonly ILocalizationService _localizationService;
        private readonly IPaymentModelFactory _paymentModelFactory;
        private readonly IPaymentService _paymentService;
        private readonly IPermissionService _permissionService;
        private readonly IPluginFinder _pluginFinder;
        private readonly ISettingService _settingService;
        private readonly PaymentSettings _paymentSettings;

        #endregion

        #region Ctor

        public PaymentController(ICountryService countryService,
            ILocalizationService localizationService,
            IPaymentModelFactory paymentModelFactory,
            IPaymentService paymentService,
            IPermissionService permissionService,
            IPluginFinder pluginFinder,
            ISettingService settingService,
            PaymentSettings paymentSettings)
        {
            this._countryService = countryService;
            this._localizationService = localizationService;
            this._paymentModelFactory = paymentModelFactory;
            this._paymentService = paymentService;
            this._permissionService = permissionService;
            this._pluginFinder = pluginFinder;
            this._settingService = settingService;
            this._paymentSettings = paymentSettings;
        }

        #endregion

        #region Methods        

        public virtual IActionResult PaymentMethods()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManagePaymentMethods))
                return AccessDeniedView();

            //prepare model
            var model = _paymentModelFactory.PreparePaymentMethodsModel(new PaymentMethodsModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult Methods(PaymentMethodSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManagePaymentMethods))
                return AccessDeniedKendoGridJson();

            //prepare model
            var model = _paymentModelFactory.PreparePaymentMethodListModel(searchModel);

            return Json(model);
        }

        [HttpPost]
        public virtual IActionResult MethodUpdate(PaymentMethodModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManagePaymentMethods))
                return AccessDeniedView();

            var pm = _paymentService.LoadPaymentMethodBySystemName(model.SystemName);
            if (_paymentService.IsPaymentMethodActive(pm))
            {
                if (!model.IsActive)
                {
                    //mark as disabled
                    _paymentSettings.ActivePaymentMethodSystemNames.Remove(pm.PluginDescriptor.SystemName);
                    _settingService.SaveSetting(_paymentSettings);
                }
            }
            else
            {
                if (model.IsActive)
                {
                    //mark as active
                    _paymentSettings.ActivePaymentMethodSystemNames.Add(pm.PluginDescriptor.SystemName);
                    _settingService.SaveSetting(_paymentSettings);
                }
            }

            var pluginDescriptor = pm.PluginDescriptor;
            pluginDescriptor.FriendlyName = model.FriendlyName;
            pluginDescriptor.DisplayOrder = model.DisplayOrder;

            //update the description file
            PluginManager.SavePluginDescriptor(pluginDescriptor);

            //reset plugin cache
            _pluginFinder.ReloadPlugins(pluginDescriptor);

            return new NullJsonResult();
        }

        public virtual IActionResult MethodRestrictions()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManagePaymentMethods))
                return AccessDeniedView();

            //prepare model
            var model = _paymentModelFactory.PreparePaymentMethodRestrictionModel(new PaymentMethodRestrictionModel());

            return View(model);
        }

        //we ignore this filter for increase RequestFormLimits
        [AdminAntiForgery(true)]
        //we use 2048 value because in some cases default value (1024) is too small for this action
        [RequestFormLimits(ValueCountLimit = 2048)]
        [HttpPost, ActionName("MethodRestrictions")]
        public virtual IActionResult MethodRestrictionsSave(PaymentMethodsModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManagePaymentMethods))
                return AccessDeniedView();

            var paymentMethods = _paymentService.LoadAllPaymentMethods();
            var countries = _countryService.GetAllCountries(showHidden: true);

            foreach (var pm in paymentMethods)
            {
                var formKey = "restrict_" + pm.PluginDescriptor.SystemName;
                var countryIdsToRestrict = (!StringValues.IsNullOrEmpty(model.Form[formKey])
                        ? model.Form[formKey].ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList()
                        : new List<string>())
                    .Select(x => Convert.ToInt32(x)).ToList();

                var newCountryIds = new List<int>();
                foreach (var c in countries)
                {
                    if (countryIdsToRestrict.Contains(c.Id))
                    {
                        newCountryIds.Add(c.Id);
                    }
                }

                _paymentService.SaveRestictedCountryIds(pm, newCountryIds);
            }

            SuccessNotification(_localizationService.GetResource("Admin.Configuration.Payment.MethodRestrictions.Updated"));

            //selected tab
            SaveSelectedTabName();

            return RedirectToAction("PaymentMethods");
        }

        #endregion
    }
}