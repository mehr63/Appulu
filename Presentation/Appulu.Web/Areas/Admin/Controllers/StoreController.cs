using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Appulu.Core.Domain.Stores;
using Appulu.Services.Configuration;
using Appulu.Services.Localization;
using Appulu.Services.Logging;
using Appulu.Services.Security;
using Appulu.Services.Stores;
using Appulu.Web.Areas.Admin.Factories;
using Appulu.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Appulu.Web.Areas.Admin.Models.Stores;
using Appulu.Web.Framework.Controllers;
using Appulu.Web.Framework.Mvc.Filters;

namespace Appulu.Web.Areas.Admin.Controllers
{
    public partial class StoreController : BaseAdminController
    {
        #region Fields

        private readonly IUserActivityService _userActivityService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly IPermissionService _permissionService;
        private readonly ISettingService _settingService;
        private readonly IStoreModelFactory _storeModelFactory;
        private readonly IStoreService _storeService;

        #endregion

        #region Ctor

        public StoreController(IUserActivityService userActivityService,
            ILocalizationService localizationService,
            ILocalizedEntityService localizedEntityService,
            IPermissionService permissionService,
            ISettingService settingService,
            IStoreModelFactory storeModelFactory,
            IStoreService storeService)
        {
            this._userActivityService = userActivityService;
            this._localizationService = localizationService;
            this._localizedEntityService = localizedEntityService;
            this._permissionService = permissionService;
            this._settingService = settingService;
            this._storeModelFactory = storeModelFactory;
            this._storeService = storeService;
        }

        #endregion

        #region Utilities

        protected virtual void UpdateAttributeLocales(Store store, StoreModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(store,
                    x => x.Name,
                    localized.Name,
                    localized.LanguageId);
            }
        }

        #endregion

        #region Methods

        public virtual IActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageStores))
                return AccessDeniedView();

            //prepare model
            var model = _storeModelFactory.PrepareStoreSearchModel(new StoreSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult List(StoreSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageStores))
                return AccessDeniedKendoGridJson();

            //prepare model
            var model = _storeModelFactory.PrepareStoreListModel(searchModel);

            return Json(model);
        }

        public virtual IActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageStores))
                return AccessDeniedView();

            //prepare model
            var model = _storeModelFactory.PrepareStoreModel(new StoreModel(), null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Create(StoreModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageStores))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var store = model.ToEntity<Store>();

                //ensure we have "/" at the end
                if (!store.Url.EndsWith("/"))
                    store.Url += "/";

                _storeService.InsertStore(store);

                //activity log
                _userActivityService.InsertActivity("AddNewStore",
                    string.Format(_localizationService.GetResource("ActivityLog.AddNewStore"), store.Id), store);

                //locales
                UpdateAttributeLocales(store, model);

                SuccessNotification(_localizationService.GetResource("Admin.Configuration.Stores.Added"));

                return continueEditing ? RedirectToAction("Edit", new { id = store.Id }) : RedirectToAction("List");
            }

            //prepare model
            model = _storeModelFactory.PrepareStoreModel(model, null, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual IActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageStores))
                return AccessDeniedView();

            //try to get a store with the specified id
            var store = _storeService.GetStoreById(id, false);
            if (store == null)
                return RedirectToAction("List");

            //prepare model
            var model = _storeModelFactory.PrepareStoreModel(null, store);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public virtual IActionResult Edit(StoreModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageStores))
                return AccessDeniedView();

            //try to get a store with the specified id
            var store = _storeService.GetStoreById(model.Id, false);
            if (store == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                store = model.ToEntity(store);

                //ensure we have "/" at the end
                if (!store.Url.EndsWith("/"))
                    store.Url += "/";

                _storeService.UpdateStore(store);

                //activity log
                _userActivityService.InsertActivity("EditStore",
                    string.Format(_localizationService.GetResource("ActivityLog.EditStore"), store.Id), store);

                //locales
                UpdateAttributeLocales(store, model);

                SuccessNotification(_localizationService.GetResource("Admin.Configuration.Stores.Updated"));

                return continueEditing ? RedirectToAction("Edit", new { id = store.Id }) : RedirectToAction("List");
            }

            //prepare model
            model = _storeModelFactory.PrepareStoreModel(model, store, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageStores))
                return AccessDeniedView();

            //try to get a store with the specified id
            var store = _storeService.GetStoreById(id, false);
            if (store == null)
                return RedirectToAction("List");

            try
            {
                _storeService.DeleteStore(store);

                //activity log
                _userActivityService.InsertActivity("DeleteStore",
                    string.Format(_localizationService.GetResource("ActivityLog.DeleteStore"), store.Id), store);

                //when we delete a store we should also ensure that all "per store" settings will also be deleted
                var settingsToDelete = _settingService
                    .GetAllSettings()
                    .Where(s => s.StoreId == id)
                    .ToList();
                _settingService.DeleteSettings(settingsToDelete);

                //when we had two stores and now have only one store, we also should delete all "per store" settings
                var allStores = _storeService.GetAllStores(false);
                if (allStores.Count == 1)
                {
                    settingsToDelete = _settingService
                        .GetAllSettings()
                        .Where(s => s.StoreId == allStores[0].Id)
                        .ToList();
                    _settingService.DeleteSettings(settingsToDelete);
                }

                SuccessNotification(_localizationService.GetResource("Admin.Configuration.Stores.Deleted"));

                return RedirectToAction("List");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("Edit", new { id = store.Id });
            }
        }

        #endregion
    }
}