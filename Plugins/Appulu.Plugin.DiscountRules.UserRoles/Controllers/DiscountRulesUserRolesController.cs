using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Appulu.Core.Domain.Discounts;
using Appulu.Plugin.DiscountRules.UserRoles.Models;
using Appulu.Services.Configuration;
using Appulu.Services.Users;
using Appulu.Services.Discounts;
using Appulu.Services.Localization;
using Appulu.Services.Security;
using Appulu.Web.Framework;
using Appulu.Web.Framework.Controllers;
using Appulu.Web.Framework.Mvc.Filters;

namespace Appulu.Plugin.DiscountRules.UserRoles.Controllers
{
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    public class DiscountRulesUserRolesController : BasePluginController
    {
        #region Fields

        private readonly IUserService _userService;
        private readonly IDiscountService _discountService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly ISettingService _settingService;

        #endregion

        #region Ctor

        public DiscountRulesUserRolesController(IUserService userService,
            IDiscountService discountService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            ISettingService settingService)
        {
            this._userService = userService;
            this._discountService = discountService;
            this._localizationService = localizationService;
            this._permissionService = permissionService;
            this._settingService = settingService;
        }

        #endregion

        #region Methods

        public IActionResult Configure(int discountId, int? discountRequirementId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageDiscounts))
                return Content("Access denied");

            //load the discount
            var discount = _discountService.GetDiscountById(discountId);
            if (discount == null)
                throw new ArgumentException("Discount could not be loaded");

            //check whether the discount requirement exists
            if (discountRequirementId.HasValue && !discount.DiscountRequirements.Any(requirement => requirement.Id == discountRequirementId.Value))
                return Content("Failed to load requirement.");

            //try to get previously saved restricted user role identifier
            var restrictedRoleId = _settingService.GetSettingByKey<int>(string.Format(DiscountRequirementDefaults.SettingsKey, discountRequirementId ?? 0));

            var model = new RequirementModel
            {
                RequirementId = discountRequirementId ?? 0,
                DiscountId = discountId,
                UserRoleId = restrictedRoleId
            };

            //set available user roles
            model.AvailableUserRoles = _userService.GetAllUserRoles(true).Select(role => new SelectListItem
            {
                Text = role.Name,
                Value = role.Id.ToString(),
                Selected = role.Id == restrictedRoleId
            }).ToList();
            model.AvailableUserRoles.Insert(0, new SelectListItem
            {
                Text = _localizationService.GetResource("Plugins.DiscountRules.UserRoles.Fields.UserRole.Select"),
                Value = "0"
            });

            //set the HTML field prefix
            ViewData.TemplateInfo.HtmlFieldPrefix = string.Format(DiscountRequirementDefaults.HtmlFieldPrefix, discountRequirementId ?? 0);

            return View("~/Plugins/DiscountRules.UserRoles/Views/Configure.cshtml", model);
        }

        [HttpPost]
        [AdminAntiForgery]
        public IActionResult Configure(int discountId, int? discountRequirementId, int userRoleId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageDiscounts))
                return Content("Access denied");

            //load the discount
            var discount = _discountService.GetDiscountById(discountId);
            if (discount == null)
                throw new ArgumentException("Discount could not be loaded");

            //get the discount requirement
            var discountRequirement = discountRequirementId.HasValue 
                ? discount.DiscountRequirements.FirstOrDefault(requirement => requirement.Id == discountRequirementId.Value) : null;

            //the discount requirement does not exist, so create a new one
            if (discountRequirement == null)
            {
                discountRequirement = new DiscountRequirement
                {
                    DiscountRequirementRuleSystemName = DiscountRequirementDefaults.SystemName
                };
                discount.DiscountRequirements.Add(discountRequirement);
                _discountService.UpdateDiscount(discount);
            }

            //save restricted user role identifier
            _settingService.SetSetting(string.Format(DiscountRequirementDefaults.SettingsKey, discountRequirement.Id), userRoleId);

            return Json(new { Result = true, NewRequirementId = discountRequirement.Id });
        }

        #endregion
    }
}