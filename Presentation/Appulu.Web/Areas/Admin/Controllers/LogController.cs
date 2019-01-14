﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Appulu.Services.Localization;
using Appulu.Services.Logging;
using Appulu.Services.Security;
using Appulu.Web.Areas.Admin.Factories;
using Appulu.Web.Areas.Admin.Models.Logging;
using Appulu.Web.Framework.Controllers;

namespace Appulu.Web.Areas.Admin.Controllers
{
    public partial class LogController : BaseAdminController
    {
        #region Fields

        private readonly IUserActivityService _userActivityService;
        private readonly ILocalizationService _localizationService;
        private readonly ILogger _logger;
        private readonly ILogModelFactory _logModelFactory;
        private readonly IPermissionService _permissionService;

        #endregion

        #region Ctor

        public LogController(IUserActivityService userActivityService,
            ILocalizationService localizationService,
            ILogger logger,
            ILogModelFactory logModelFactory,
            IPermissionService permissionService)
        {
            this._userActivityService = userActivityService;
            this._localizationService = localizationService;
            this._logger = logger;
            this._logModelFactory = logModelFactory;
            this._permissionService = permissionService;
        }

        #endregion

        #region Methods

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSystemLog))
                return AccessDeniedView();

            //prepare model
            var model = _logModelFactory.PrepareLogSearchModel(new LogSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult LogList(LogSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSystemLog))
                return AccessDeniedKendoGridJson();

            //prepare model
            var model = _logModelFactory.PrepareLogListModel(searchModel);

            return Json(model);
        }

        [HttpPost, ActionName("List")]
        [FormValueRequired("clearall")]
        public virtual IActionResult ClearAll()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSystemLog))
                return AccessDeniedView();

            _logger.ClearLog();

            //activity log
            _userActivityService.InsertActivity("DeleteSystemLog", _localizationService.GetResource("ActivityLog.DeleteSystemLog"));

            SuccessNotification(_localizationService.GetResource("Admin.System.Log.Cleared"));

            return RedirectToAction("List");
        }

        public virtual IActionResult View(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSystemLog))
                return AccessDeniedView();

            //try to get a log with the specified id
            var log = _logger.GetLogById(id);
            if (log == null)
                return RedirectToAction("List");

            //prepare model
            var model = _logModelFactory.PrepareLogModel(null, log);

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSystemLog))
                return AccessDeniedView();

            //try to get a log with the specified id
            var log = _logger.GetLogById(id);
            if (log == null)
                return RedirectToAction("List");

            _logger.DeleteLog(log);

            //activity log
            _userActivityService.InsertActivity("DeleteSystemLog", _localizationService.GetResource("ActivityLog.DeleteSystemLog"), log);

            SuccessNotification(_localizationService.GetResource("Admin.System.Log.Deleted"));

            return RedirectToAction("List");
        }

        [HttpPost]
        public virtual IActionResult DeleteSelected(ICollection<int> selectedIds)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSystemLog))
                return AccessDeniedView();

            if (selectedIds != null)
                _logger.DeleteLogs(_logger.GetLogByIds(selectedIds.ToArray()).ToList());

            //activity log
            _userActivityService.InsertActivity("DeleteSystemLog", _localizationService.GetResource("ActivityLog.DeleteSystemLog"));

            return Json(new { Result = true });
        }

        #endregion
    }
}