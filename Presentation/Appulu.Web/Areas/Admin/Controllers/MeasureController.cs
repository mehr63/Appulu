﻿using System;
using Microsoft.AspNetCore.Mvc;
using Appulu.Core.Domain.Directory;
using Appulu.Services.Configuration;
using Appulu.Services.Directory;
using Appulu.Services.Localization;
using Appulu.Services.Logging;
using Appulu.Services.Security;
using Appulu.Web.Areas.Admin.Factories;
using Appulu.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Appulu.Web.Areas.Admin.Models.Directory;
using Appulu.Web.Framework.Kendoui;
using Appulu.Web.Framework.Mvc;

namespace Appulu.Web.Areas.Admin.Controllers
{
    public partial class MeasureController : BaseAdminController
    {
        #region Fields

        private readonly IUserActivityService _userActivityService;
        private readonly ILocalizationService _localizationService;
        private readonly IMeasureModelFactory _measureModelFactory;
        private readonly IMeasureService _measureService;
        private readonly IPermissionService _permissionService;
        private readonly ISettingService _settingService;
        private readonly MeasureSettings _measureSettings;

        #endregion

        #region Ctor

        public MeasureController(IUserActivityService userActivityService,
            ILocalizationService localizationService,
            IMeasureModelFactory measureModelFactory,
            IMeasureService measureService,
            IPermissionService permissionService,
            ISettingService settingService,
            MeasureSettings measureSettings)
        {
            this._userActivityService = userActivityService;
            this._localizationService = localizationService;
            this._measureModelFactory = measureModelFactory;
            this._measureService = measureService;
            this._permissionService = permissionService;
            this._settingService = settingService;
            this._measureSettings = measureSettings;
        }

        #endregion

        #region Methods

        public virtual IActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageShippingSettings))
                return AccessDeniedView();

            //prepare model
            var model = _measureModelFactory.PrepareMeasureSearchModel(new MeasureSearchModel());

            return View(model);
        }

        #region Weights

        [HttpPost]
        public virtual IActionResult Weights(MeasureWeightSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageShippingSettings))
                return AccessDeniedKendoGridJson();

            //prepare model
            var model = _measureModelFactory.PrepareMeasureWeightListModel(searchModel);

            return Json(model);
        }

        [HttpPost]
        public virtual IActionResult WeightUpdate(MeasureWeightModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageShippingSettings))
                return AccessDeniedView();

            if (!ModelState.IsValid)
                return Json(new DataSourceResult { Errors = ModelState.SerializeErrors() });

            var weight = _measureService.GetMeasureWeightById(model.Id);
            weight = model.ToEntity(weight);
            _measureService.UpdateMeasureWeight(weight);

            //activity log
            _userActivityService.InsertActivity("EditMeasureWeight",
                string.Format(_localizationService.GetResource("ActivityLog.EditMeasureWeight"), weight.Id), weight);

            return new NullJsonResult();
        }

        [HttpPost]
        public virtual IActionResult WeightAdd(MeasureWeightModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageShippingSettings))
                return AccessDeniedView();

            if (!ModelState.IsValid)
                return Json(new DataSourceResult { Errors = ModelState.SerializeErrors() });

            var weight = new MeasureWeight();
            weight = model.ToEntity(weight);
            _measureService.InsertMeasureWeight(weight);

            //activity log
            _userActivityService.InsertActivity("AddNewMeasureWeight",
                string.Format(_localizationService.GetResource("ActivityLog.AddNewMeasureWeight"), weight.Id), weight);

            return new NullJsonResult();
        }

        [HttpPost]
        public virtual IActionResult WeightDelete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageShippingSettings))
                return AccessDeniedView();

            //try to get a weight with the specified id
            var weight = _measureService.GetMeasureWeightById(id)
                ?? throw new ArgumentException("No weight found with the specified id", nameof(id));

            if (weight.Id == _measureSettings.BaseWeightId)
            {
                return Json(new DataSourceResult
                {
                    Errors = _localizationService.GetResource("Admin.Configuration.Shipping.Measures.Weights.CantDeletePrimary")
                });
            }

            _measureService.DeleteMeasureWeight(weight);

            //activity log
            _userActivityService.InsertActivity("DeleteMeasureWeight",
                string.Format(_localizationService.GetResource("ActivityLog.DeleteMeasureWeight"), weight.Id), weight);

            return new NullJsonResult();
        }

        [HttpPost]
        public virtual IActionResult MarkAsPrimaryWeight(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageShippingSettings))
                return AccessDeniedView();

            //try to get a weight with the specified id
            var weight = _measureService.GetMeasureWeightById(id)
                ?? throw new ArgumentException("No weight found with the specified id", nameof(id));

            _measureSettings.BaseWeightId = weight.Id;
            _settingService.SaveSetting(_measureSettings);

            return Json(new { result = true });
        }

        #endregion

        #region Dimensions

        [HttpPost]
        public virtual IActionResult Dimensions(MeasureDimensionSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageShippingSettings))
                return AccessDeniedKendoGridJson();

            //prepare model
            var model = _measureModelFactory.PrepareMeasureDimensionListModel(searchModel);

            return Json(model);
        }

        [HttpPost]
        public virtual IActionResult DimensionUpdate(MeasureDimensionModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageShippingSettings))
                return AccessDeniedView();

            if (!ModelState.IsValid)
                return Json(new DataSourceResult { Errors = ModelState.SerializeErrors() });

            var dimension = _measureService.GetMeasureDimensionById(model.Id);
            dimension = model.ToEntity(dimension);
            _measureService.UpdateMeasureDimension(dimension);

            //activity log
            _userActivityService.InsertActivity("EditMeasureDimension",
                string.Format(_localizationService.GetResource("ActivityLog.EditMeasureDimension"), dimension.Id), dimension);

            return new NullJsonResult();
        }

        [HttpPost]
        public virtual IActionResult DimensionAdd(MeasureDimensionModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageShippingSettings))
                return AccessDeniedView();

            if (!ModelState.IsValid)
                return Json(new DataSourceResult { Errors = ModelState.SerializeErrors() });

            var dimension = new MeasureDimension();
            dimension = model.ToEntity(dimension);
            _measureService.InsertMeasureDimension(dimension);

            //activity log
            _userActivityService.InsertActivity("AddNewMeasureDimension",
                string.Format(_localizationService.GetResource("ActivityLog.AddNewMeasureDimension"), dimension.Id), dimension);

            return new NullJsonResult();
        }

        [HttpPost]
        public virtual IActionResult DimensionDelete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageShippingSettings))
                return AccessDeniedView();

            //try to get a dimension with the specified id
            var dimension = _measureService.GetMeasureDimensionById(id)
                ?? throw new ArgumentException("No dimension found with the specified id", nameof(id));

            if (dimension.Id == _measureSettings.BaseDimensionId)
            {
                return Json(new DataSourceResult
                {
                    Errors = _localizationService.GetResource("Admin.Configuration.Shipping.Measures.Dimensions.CantDeletePrimary")
                });
            }

            _measureService.DeleteMeasureDimension(dimension);

            //activity log
            _userActivityService.InsertActivity("DeleteMeasureDimension",
                string.Format(_localizationService.GetResource("ActivityLog.DeleteMeasureDimension"), dimension.Id), dimension);

            return new NullJsonResult();
        }

        [HttpPost]
        public virtual IActionResult MarkAsPrimaryDimension(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageShippingSettings))
                return AccessDeniedView();

            //try to get a dimension with the specified id
            var dimension = _measureService.GetMeasureDimensionById(id)
                ?? throw new ArgumentException("No dimension found with the specified id", nameof(id));

            _measureSettings.BaseDimensionId = dimension.Id;
            _settingService.SaveSetting(_measureSettings);

            return Json(new { result = true });
        }

        #endregion

        #endregion
    }
}