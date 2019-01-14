using System;
using Microsoft.AspNetCore.Mvc;
using Appulu.Core;
using Appulu.Core.Domain.Users;
using Appulu.Services.Catalog;
using Appulu.Services.Users;
using Appulu.Services.Localization;
using Appulu.Services.Logging;
using Appulu.Services.Security;
using Appulu.Web.Areas.Admin.Factories;
using Appulu.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Appulu.Web.Areas.Admin.Models.Users;
using Appulu.Web.Framework.Controllers;
using Appulu.Web.Framework.Mvc.Filters;

namespace Appulu.Web.Areas.Admin.Controllers
{
    public partial class UserRoleController : BaseAdminController
    {
        #region Fields

        private readonly IUserActivityService _userActivityService;
        private readonly IUserRoleModelFactory _userRoleModelFactory;
        private readonly IUserService _userService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly IProductService _productService;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public UserRoleController(IUserActivityService userActivityService,
            IUserRoleModelFactory userRoleModelFactory,
            IUserService userService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            IProductService productService,
            IWorkContext workContext)
        {
            this._userActivityService = userActivityService;
            this._userRoleModelFactory = userRoleModelFactory;
            this._userService = userService;
            this._localizationService = localizationService;
            this._permissionService = permissionService;
            this._productService = productService;
            this._workContext = workContext;
        }

        #endregion

        #region Methods

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            //prepare model
            var model = _userRoleModelFactory.PrepareUserRoleSearchModel(new UserRoleSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult List(UserRoleSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedKendoGridJson();

            //prepare model
            var model = _userRoleModelFactory.PrepareUserRoleListModel(searchModel);

            return Json(model);
        }

        public virtual IActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            //prepare model
            var model = _userRoleModelFactory.PrepareUserRoleModel(new UserRoleModel(), null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Create(UserRoleModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var userRole = model.ToEntity<UserRole>();
                _userService.InsertUserRole(userRole);

                //activity log
                _userActivityService.InsertActivity("AddNewUserRole",
                    string.Format(_localizationService.GetResource("ActivityLog.AddNewUserRole"), userRole.Name), userRole);

                SuccessNotification(_localizationService.GetResource("Admin.Users.UserRoles.Added"));

                return continueEditing ? RedirectToAction("Edit", new { id = userRole.Id }) : RedirectToAction("List");
            }

            //prepare model
            model = _userRoleModelFactory.PrepareUserRoleModel(model, null, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual IActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            //try to get a user role with the specified id
            var userRole = _userService.GetUserRoleById(id);
            if (userRole == null)
                return RedirectToAction("List");

            //prepare model
            var model = _userRoleModelFactory.PrepareUserRoleModel(null, userRole);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Edit(UserRoleModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            //try to get a user role with the specified id
            var userRole = _userService.GetUserRoleById(model.Id);
            if (userRole == null)
                return RedirectToAction("List");

            try
            {
                if (ModelState.IsValid)
                {
                    if (userRole.IsSystemRole && !model.Active)
                        throw new AppException(_localizationService.GetResource("Admin.Users.UserRoles.Fields.Active.CantEditSystem"));

                    if (userRole.IsSystemRole && !userRole.SystemName.Equals(model.SystemName, StringComparison.InvariantCultureIgnoreCase))
                        throw new AppException(_localizationService.GetResource("Admin.Users.UserRoles.Fields.SystemName.CantEditSystem"));

                    if (AppUserDefaults.RegisteredRoleName.Equals(userRole.SystemName, StringComparison.InvariantCultureIgnoreCase) &&
                        model.PurchasedWithProductId > 0)
                        throw new AppException(_localizationService.GetResource("Admin.Users.UserRoles.Fields.PurchasedWithProduct.Registered"));

                    userRole = model.ToEntity(userRole);
                    _userService.UpdateUserRole(userRole);

                    //activity log
                    _userActivityService.InsertActivity("EditUserRole",
                        string.Format(_localizationService.GetResource("ActivityLog.EditUserRole"), userRole.Name), userRole);

                    SuccessNotification(_localizationService.GetResource("Admin.Users.UserRoles.Updated"));

                    return continueEditing ? RedirectToAction("Edit", new { id = userRole.Id }) : RedirectToAction("List");
                }

                //prepare model
                model = _userRoleModelFactory.PrepareUserRoleModel(model, userRole, true);

                //if we got this far, something failed, redisplay form
                return View(model);
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("Edit", new { id = userRole.Id });
            }
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            //try to get a user role with the specified id
            var userRole = _userService.GetUserRoleById(id);
            if (userRole == null)
                return RedirectToAction("List");

            try
            {
                _userService.DeleteUserRole(userRole);

                //activity log
                _userActivityService.InsertActivity("DeleteUserRole",
                    string.Format(_localizationService.GetResource("ActivityLog.DeleteUserRole"), userRole.Name), userRole);

                SuccessNotification(_localizationService.GetResource("Admin.Users.UserRoles.Deleted"));

                return RedirectToAction("List");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc.Message);
                return RedirectToAction("Edit", new { id = userRole.Id });
            }
        }

        public virtual IActionResult AssociateProductToUserRolePopup()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            //prepare model
            var model = _userRoleModelFactory.PrepareUserRoleProductSearchModel(new UserRoleProductSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult AssociateProductToUserRolePopupList(UserRoleProductSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedKendoGridJson();

            //prepare model
            var model = _userRoleModelFactory.PrepareUserRoleProductListModel(searchModel);

            return Json(model);
        }

        [HttpPost]
        [FormValueRequired("save")]
        public virtual IActionResult AssociateProductToUserRolePopup([Bind(Prefix = nameof(AddProductToUserRoleModel))] AddProductToUserRoleModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            //try to get a product with the specified id
            var associatedProduct = _productService.GetProductById(model.AssociatedToProductId);
            if (associatedProduct == null)
                return Content("Cannot load a product");

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null && associatedProduct.VendorId != _workContext.CurrentVendor.Id)
                return Content("This is not your product");

            ViewBag.RefreshPage = true;
            ViewBag.productId = associatedProduct.Id;
            ViewBag.productName = associatedProduct.Name;

            return View(new UserRoleProductSearchModel());
        }

        #endregion
    }
}