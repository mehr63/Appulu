using System;
using Microsoft.AspNetCore.Mvc;
using Appulu.Core.Domain.Catalog;
using Appulu.Services.Catalog;
using Appulu.Services.Localization;
using Appulu.Services.Logging;
using Appulu.Services.Security;
using Appulu.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Appulu.Web.Areas.Admin.Factories;
using Appulu.Web.Areas.Admin.Models.Catalog;
using Appulu.Web.Framework.Mvc.Filters;

namespace Appulu.Web.Areas.Admin.Controllers
{
    /// <summary>
    /// Represent a review type controller
    /// </summary>
    public partial class ReviewTypeController : BaseAdminController
    {
        #region Fields

        private readonly IUserActivityService _userActivityService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly IPermissionService _permissionService;
        private readonly IReviewTypeModelFactory _reviewTypeModelFactory;
        private readonly IReviewTypeService _reviewTypeService;

        #endregion

        #region Ctor

        public ReviewTypeController(IUserActivityService userActivityService,
            ILocalizationService localizationService,
            ILocalizedEntityService localizedEntityService,
            IPermissionService permissionService,
            IReviewTypeModelFactory reviewTypeModelFactory,
            IReviewTypeService reviewTypeService)
        {
            this._reviewTypeModelFactory = reviewTypeModelFactory;
            this._reviewTypeService = reviewTypeService;
            this._userActivityService = userActivityService;
            this._localizedEntityService = localizedEntityService;
            this._localizationService = localizationService;
            this._permissionService = permissionService;
        }

        #endregion

        #region Utilities

        protected virtual void UpdateReviewTypeLocales(ReviewType reviewType, ReviewTypeModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(reviewType,
                    x => x.Name,                    
                    localized.Name,
                    localized.LanguageId);

                _localizedEntityService.SaveLocalizedValue(reviewType,
                   x => x.Description,
                   localized.Description,
                   localized.LanguageId);
            }
        }

        #endregion

        #region Review type

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //we just redirect a user to the catalog settings page
            return RedirectToAction("Catalog", "Setting");
        }

        [HttpPost]
        public virtual IActionResult List(ReviewTypeSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedKendoGridJson();

            //prepare model
            var model = _reviewTypeModelFactory.PrepareReviewTypeListModel(searchModel);

            return Json(model);
        }

        public virtual IActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //prepare model
            var model = _reviewTypeModelFactory.PrepareReviewTypeModel(new ReviewTypeModel(), null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Create(ReviewTypeModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var reviewType = model.ToEntity<ReviewType>();
                _reviewTypeService.InsertReviewType(reviewType);                

                //activity log
                _userActivityService.InsertActivity("AddNewReviewType",
                    string.Format(_localizationService.GetResource("ActivityLog.AddNewReviewType"), reviewType.Id), reviewType);

                //locales                
                UpdateReviewTypeLocales(reviewType, model);

                SuccessNotification(_localizationService.GetResource("Admin.Settings.ReviewType.Added"));

                return continueEditing ? RedirectToAction("Edit", new { id = reviewType.Id }) : RedirectToAction("List");                
            }

            //prepare model
            model = _reviewTypeModelFactory.PrepareReviewTypeModel(model, null, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual IActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //try to get an product review type with the specified id
            var reviewType = _reviewTypeService.GetReviewTypeById(id);
            if (reviewType == null)
                return RedirectToAction("List");

            //prepare model
            var model = _reviewTypeModelFactory.PrepareReviewTypeModel(null, reviewType);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Edit(ReviewTypeModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //try to get an review type with the specified id
            var reviewType = _reviewTypeService.GetReviewTypeById(model.Id);
            if (reviewType == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                reviewType = model.ToEntity(reviewType);
                _reviewTypeService.UpdateReviewType(reviewType);

                //activity log
                _userActivityService.InsertActivity("EditReviewType",
                    string.Format(_localizationService.GetResource("ActivityLog.EditReviewType"), reviewType.Id),
                    reviewType);

                //locales
                UpdateReviewTypeLocales(reviewType, model);

                SuccessNotification(_localizationService.GetResource("Admin.Settings.ReviewType.Updated"));

                return continueEditing ? RedirectToAction("Edit", new { id = reviewType.Id }) : RedirectToAction("List");                
            }

            //prepare model
            model = _reviewTypeModelFactory.PrepareReviewTypeModel(model, reviewType, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //try to get an review type with the specified id
            var reviewType = _reviewTypeService.GetReviewTypeById(id);
            if (reviewType == null)
                return RedirectToAction("List");

            try
            {
                _reviewTypeService.DeleteReiewType(reviewType);

                //activity log
                _userActivityService.InsertActivity("DeleteReviewType",
                    string.Format(_localizationService.GetResource("ActivityLog.DeleteReviewType"), reviewType),
                    reviewType);

                SuccessNotification(_localizationService.GetResource("Admin.Settings.ReviewType.Deleted"));

                return RedirectToAction("List");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("Edit", new { id = reviewType.Id });
            }            
        }

        #endregion
    }
}
