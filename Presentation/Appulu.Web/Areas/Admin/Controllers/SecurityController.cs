using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Appulu.Core;
using Appulu.Core.Domain.Users;
using Appulu.Core.Domain.Security;
using Appulu.Services.Users;
using Appulu.Services.Localization;
using Appulu.Services.Logging;
using Appulu.Services.Security;
using Appulu.Web.Areas.Admin.Factories;
using Appulu.Web.Areas.Admin.Models.Security;

namespace Appulu.Web.Areas.Admin.Controllers
{
    public partial class SecurityController : BaseAdminController
    {
        #region Fields

        private readonly IUserService _userService;
        private readonly ILocalizationService _localizationService;
        private readonly ILogger _logger;
        private readonly IPermissionService _permissionService;
        private readonly ISecurityModelFactory _securityModelFactory;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public SecurityController(IUserService userService,
            ILocalizationService localizationService,
            ILogger logger,
            IPermissionService permissionService,
            ISecurityModelFactory securityModelFactory,
            IWorkContext workContext)
        {
            this._userService = userService;
            this._localizationService = localizationService;
            this._logger = logger;
            this._permissionService = permissionService;
            this._securityModelFactory = securityModelFactory;
            this._workContext = workContext;
        }

        #endregion

        #region Methods

        public virtual IActionResult AccessDenied(string pageUrl)
        {
            var currentUser = _workContext.CurrentUser;
            if (currentUser == null || currentUser.IsGuest())
            {
                _logger.Information($"Access denied to anonymous request on {pageUrl}");
                return View();
            }

            _logger.Information($"Access denied to user #{currentUser.Email} '{currentUser.Email}' on {pageUrl}");

            return View();
        }

        public virtual IActionResult Permissions()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageAcl))
                return AccessDeniedView();

            //prepare model
            var model = _securityModelFactory.PreparePermissionMappingModel(new PermissionMappingModel());

            return View(model);
        }

        [HttpPost, ActionName("Permissions")]
        public virtual IActionResult PermissionsSave(PermissionMappingModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageAcl))
                return AccessDeniedView();

            var permissionRecords = _permissionService.GetAllPermissionRecords();
            var userRoles = _userService.GetAllUserRoles(true);

            foreach (var cr in userRoles)
            {
                var formKey = "allow_" + cr.Id;
                var permissionRecordSystemNamesToRestrict = !StringValues.IsNullOrEmpty(model.Form[formKey])
                    ? model.Form[formKey].ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList()
                    : new List<string>();

                foreach (var pr in permissionRecords)
                {
                    var allow = permissionRecordSystemNamesToRestrict.Contains(pr.SystemName);
                    if (allow)
                    {
                        if (pr.PermissionRecordUserRoleMappings.FirstOrDefault(x => x.UserRoleId == cr.Id) != null)
                            continue;

                        pr.PermissionRecordUserRoleMappings.Add(new PermissionRecordUserRoleMapping { UserRole = cr });
                        _permissionService.UpdatePermissionRecord(pr);
                    }
                    else
                    {
                        if (pr.PermissionRecordUserRoleMappings.FirstOrDefault(x => x.UserRoleId == cr.Id) == null)
                            continue;

                        pr.PermissionRecordUserRoleMappings
                            .Remove(pr.PermissionRecordUserRoleMappings.FirstOrDefault(mapping => mapping.UserRoleId == cr.Id));
                        _permissionService.UpdatePermissionRecord(pr);
                    }
                }
            }

            SuccessNotification(_localizationService.GetResource("Admin.Configuration.ACL.Updated"));

            return RedirectToAction("Permissions");
        }

        #endregion
    }
}