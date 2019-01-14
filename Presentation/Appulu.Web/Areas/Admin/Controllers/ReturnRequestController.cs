﻿using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Appulu.Core.Domain.Orders;
using Appulu.Services.Users;
using Appulu.Services.Localization;
using Appulu.Services.Logging;
using Appulu.Services.Messages;
using Appulu.Services.Orders;
using Appulu.Services.Security;
using Appulu.Web.Areas.Admin.Factories;
using Appulu.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Appulu.Web.Areas.Admin.Models.Orders;
using Appulu.Web.Framework.Controllers;
using Appulu.Web.Framework.Mvc.Filters;

namespace Appulu.Web.Areas.Admin.Controllers
{
    public partial class ReturnRequestController : BaseAdminController
    {
        #region Fields

        private readonly IUserActivityService _userActivityService;
        private readonly IUserService _userService;
        private readonly ILocalizationService _localizationService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly IOrderService _orderService;
        private readonly IPermissionService _permissionService;
        private readonly IReturnRequestModelFactory _returnRequestModelFactory;
        private readonly IReturnRequestService _returnRequestService;
        private readonly IWorkflowMessageService _workflowMessageService;

        #endregion Fields

        #region Ctor

        public ReturnRequestController(IUserActivityService userActivityService,
            IUserService userService,
            ILocalizationService localizationService,
            ILocalizedEntityService localizedEntityService,
            IOrderService orderService,
            IPermissionService permissionService,
            IReturnRequestModelFactory returnRequestModelFactory,
            IReturnRequestService returnRequestService,
            IWorkflowMessageService workflowMessageService)
        {
            this._userActivityService = userActivityService;
            this._userService = userService;
            this._localizationService = localizationService;
            this._localizedEntityService = localizedEntityService;
            this._orderService = orderService;
            this._permissionService = permissionService;
            this._returnRequestModelFactory = returnRequestModelFactory;
            this._returnRequestService = returnRequestService;
            this._workflowMessageService = workflowMessageService;
        }

        #endregion

        #region Utilities

        protected virtual void UpdateLocales(ReturnRequestReason rrr, ReturnRequestReasonModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(rrr,
                    x => x.Name,
                    localized.Name,
                    localized.LanguageId);
            }
        }

        protected virtual void UpdateLocales(ReturnRequestAction rra, ReturnRequestActionModel model)
        {
            foreach (var localized in model.Locales)
            {
                _localizedEntityService.SaveLocalizedValue(rra,
                    x => x.Name,
                    localized.Name,
                    localized.LanguageId);
            }
        }

        #endregion

        #region Methods

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageReturnRequests))
                return AccessDeniedView();

            //prepare model
            var model = _returnRequestModelFactory.PrepareReturnRequestSearchModel(new ReturnRequestSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult List(ReturnRequestSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageReturnRequests))
                return AccessDeniedKendoGridJson();

            //prepare model
            var model = _returnRequestModelFactory.PrepareReturnRequestListModel(searchModel);

            return Json(model);
        }

        public virtual IActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageReturnRequests))
                return AccessDeniedView();

            //try to get a return request with the specified id
            var returnRequest = _returnRequestService.GetReturnRequestById(id);
            if (returnRequest == null)
                return RedirectToAction("List");

            //prepare model
            var model = _returnRequestModelFactory.PrepareReturnRequestModel(null, returnRequest);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public virtual IActionResult Edit(ReturnRequestModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageReturnRequests))
                return AccessDeniedView();

            //try to get a return request with the specified id
            var returnRequest = _returnRequestService.GetReturnRequestById(model.Id);
            if (returnRequest == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                returnRequest.Quantity = model.Quantity;
                returnRequest.ReasonForReturn = model.ReasonForReturn;
                returnRequest.RequestedAction = model.RequestedAction;
                returnRequest.UserComments = model.UserComments;
                returnRequest.StaffNotes = model.StaffNotes;
                returnRequest.ReturnRequestStatusId = model.ReturnRequestStatusId;
                returnRequest.UpdatedOnUtc = DateTime.UtcNow;
                _userService.UpdateUser(returnRequest.User);

                //activity log
                _userActivityService.InsertActivity("EditReturnRequest",
                    string.Format(_localizationService.GetResource("ActivityLog.EditReturnRequest"), returnRequest.Id), returnRequest);

                SuccessNotification(_localizationService.GetResource("Admin.ReturnRequests.Updated"));

                return continueEditing ? RedirectToAction("Edit", new { id = returnRequest.Id }) : RedirectToAction("List");
            }

            //prepare model
            model = _returnRequestModelFactory.PrepareReturnRequestModel(model, returnRequest, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        [FormValueRequired("notify-user")]
        public virtual IActionResult NotifyUser(ReturnRequestModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageReturnRequests))
                return AccessDeniedView();

            //try to get a return request with the specified id
            var returnRequest = _returnRequestService.GetReturnRequestById(model.Id);
            if (returnRequest == null)
                return RedirectToAction("List");

            var orderItem = _orderService.GetOrderItemById(returnRequest.OrderItemId);
            if (orderItem == null)
            {
                ErrorNotification(_localizationService.GetResource("Admin.ReturnRequests.OrderItemDeleted"));
                return RedirectToAction("Edit", new { id = returnRequest.Id });
            }

            var queuedEmailIds = _workflowMessageService.SendReturnRequestStatusChangedUserNotification(returnRequest, orderItem, orderItem.Order.UserLanguageId);
            if (queuedEmailIds.Any())
                SuccessNotification(_localizationService.GetResource("Admin.ReturnRequests.Notified"));

            return RedirectToAction("Edit", new { id = returnRequest.Id });
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageReturnRequests))
                return AccessDeniedView();

            //try to get a return request with the specified id
            var returnRequest = _returnRequestService.GetReturnRequestById(id);
            if (returnRequest == null)
                return RedirectToAction("List");

            _returnRequestService.DeleteReturnRequest(returnRequest);

            //activity log
            _userActivityService.InsertActivity("DeleteReturnRequest",
                string.Format(_localizationService.GetResource("ActivityLog.DeleteReturnRequest"), returnRequest.Id), returnRequest);

            SuccessNotification(_localizationService.GetResource("Admin.ReturnRequests.Deleted"));

            return RedirectToAction("List");
        }

        #region Return request reasons

        public virtual IActionResult ReturnRequestReasonList()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //select "return request" tab
            SaveSelectedTabName("tab-returnrequest");

            //we just redirect a user to the order settings page
            return RedirectToAction("Order", "Setting");
        }

        [HttpPost]
        public virtual IActionResult ReturnRequestReasonList(ReturnRequestReasonSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedKendoGridJson();

            //prepare model
            var model = _returnRequestModelFactory.PrepareReturnRequestReasonListModel(searchModel);

            return Json(model);
        }

        public virtual IActionResult ReturnRequestReasonCreate()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //prepare model
            var model = _returnRequestModelFactory.PrepareReturnRequestReasonModel(new ReturnRequestReasonModel(), null);
            
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult ReturnRequestReasonCreate(ReturnRequestReasonModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var returnRequestReason = model.ToEntity<ReturnRequestReason>();
                _returnRequestService.InsertReturnRequestReason(returnRequestReason);

                //locales
                UpdateLocales(returnRequestReason, model);

                SuccessNotification(_localizationService.GetResource("Admin.Configuration.Settings.Order.ReturnRequestReasons.Added"));

                return continueEditing 
                    ? RedirectToAction("ReturnRequestReasonEdit", new { id = returnRequestReason.Id })
                    : RedirectToAction("ReturnRequestReasonList");
            }

            //prepare model
            model = _returnRequestModelFactory.PrepareReturnRequestReasonModel(model, null, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual IActionResult ReturnRequestReasonEdit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //try to get a return request reason with the specified id
            var returnRequestReason = _returnRequestService.GetReturnRequestReasonById(id);
            if (returnRequestReason == null)
                return RedirectToAction("ReturnRequestReasonList");

            //prepare model
            var model = _returnRequestModelFactory.PrepareReturnRequestReasonModel(null, returnRequestReason);
            
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult ReturnRequestReasonEdit(ReturnRequestReasonModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //try to get a return request reason with the specified id
            var returnRequestReason = _returnRequestService.GetReturnRequestReasonById(model.Id);
            if (returnRequestReason == null)
                return RedirectToAction("ReturnRequestReasonList");

            if (ModelState.IsValid)
            {
                returnRequestReason = model.ToEntity(returnRequestReason);
                _returnRequestService.UpdateReturnRequestReason(returnRequestReason);

                //locales
                UpdateLocales(returnRequestReason, model);

                SuccessNotification(_localizationService.GetResource("Admin.Configuration.Settings.Order.ReturnRequestReasons.Updated"));

                if (!continueEditing)
                    return RedirectToAction("ReturnRequestReasonList");

                //selected tab
                SaveSelectedTabName();

                return RedirectToAction("ReturnRequestReasonEdit", new { id = returnRequestReason.Id });
            }

            //prepare model
            model = _returnRequestModelFactory.PrepareReturnRequestReasonModel(model, returnRequestReason, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public virtual IActionResult ReturnRequestReasonDelete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //try to get a return request reason with the specified id
            var returnRequestReason = _returnRequestService.GetReturnRequestReasonById(id) 
                ?? throw new ArgumentException("No return request reason found with the specified id", nameof(id));

            _returnRequestService.DeleteReturnRequestReason(returnRequestReason);

            SuccessNotification(_localizationService.GetResource("Admin.Configuration.Settings.Order.ReturnRequestReasons.Deleted"));

            return RedirectToAction("ReturnRequestReasonList");
        }

        #endregion

        #region Return request actions

        public virtual IActionResult ReturnRequestActionList()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //select "return request" tab
            SaveSelectedTabName("tab-returnrequest");

            //we just redirect a user to the order settings page
            return RedirectToAction("Order", "Setting");
        }

        [HttpPost]
        public virtual IActionResult ReturnRequestActionList(ReturnRequestActionSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedKendoGridJson();

            //prepare model
            var model = _returnRequestModelFactory.PrepareReturnRequestActionListModel(searchModel);

            return Json(model);
        }

        public virtual IActionResult ReturnRequestActionCreate()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //prepare model
            var model = _returnRequestModelFactory.PrepareReturnRequestActionModel(new ReturnRequestActionModel(), null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult ReturnRequestActionCreate(ReturnRequestActionModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var returnRequestAction = model.ToEntity<ReturnRequestAction>();
                _returnRequestService.InsertReturnRequestAction(returnRequestAction);

                //locales
                UpdateLocales(returnRequestAction, model);

                SuccessNotification(_localizationService.GetResource("Admin.Configuration.Settings.Order.ReturnRequestActions.Added"));

                return continueEditing 
                    ? RedirectToAction("ReturnRequestActionEdit", new { id = returnRequestAction.Id }) 
                    : RedirectToAction("ReturnRequestActionList");
            }

            //prepare model
            model = _returnRequestModelFactory.PrepareReturnRequestActionModel(model, null, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual IActionResult ReturnRequestActionEdit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //try to get a return request action with the specified id
            var returnRequestAction = _returnRequestService.GetReturnRequestActionById(id);
            if (returnRequestAction == null)
                return RedirectToAction("ReturnRequestActionList");

            //prepare model
            var model = _returnRequestModelFactory.PrepareReturnRequestActionModel(null, returnRequestAction);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult ReturnRequestActionEdit(ReturnRequestActionModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //try to get a return request action with the specified id
            var returnRequestAction = _returnRequestService.GetReturnRequestActionById(model.Id);
            if (returnRequestAction == null)
                return RedirectToAction("ReturnRequestActionList");

            if (ModelState.IsValid)
            {
                returnRequestAction = model.ToEntity(returnRequestAction);
                _returnRequestService.UpdateReturnRequestAction(returnRequestAction);

                //locales
                UpdateLocales(returnRequestAction, model);

                SuccessNotification(_localizationService.GetResource("Admin.Configuration.Settings.Order.ReturnRequestActions.Updated"));

                if (!continueEditing)
                    return RedirectToAction("ReturnRequestActionList");

                //selected tab
                SaveSelectedTabName();

                return RedirectToAction("ReturnRequestActionEdit", new { id = returnRequestAction.Id });
            }

            //prepare model
            model = _returnRequestModelFactory.PrepareReturnRequestActionModel(model, returnRequestAction, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public virtual IActionResult ReturnRequestActionDelete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //try to get a return request action with the specified id
            var returnRequestAction = _returnRequestService.GetReturnRequestActionById(id)
                ?? throw new ArgumentException("No return request action found with the specified id", nameof(id));

            _returnRequestService.DeleteReturnRequestAction(returnRequestAction);

            SuccessNotification(_localizationService.GetResource("Admin.Configuration.Settings.Order.ReturnRequestActions.Deleted"));

            return RedirectToAction("ReturnRequestActionList");
        }

        #endregion

        #endregion
    }
}