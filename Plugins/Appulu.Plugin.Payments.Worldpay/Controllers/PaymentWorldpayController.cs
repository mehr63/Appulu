using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Appulu.Core;
using Appulu.Core.Domain.Users;
using Appulu.Plugin.Payments.Worldpay.Domain;
using Appulu.Plugin.Payments.Worldpay.Domain.Enums;
using Appulu.Plugin.Payments.Worldpay.Domain.Models;
using Appulu.Plugin.Payments.Worldpay.Domain.Requests;
using Appulu.Plugin.Payments.Worldpay.Models;
using Appulu.Plugin.Payments.Worldpay.Models.User;
using Appulu.Plugin.Payments.Worldpay.Services;
using Appulu.Services;
using Appulu.Services.Common;
using Appulu.Services.Configuration;
using Appulu.Services.Users;
using Appulu.Services.Localization;
using Appulu.Services.Security;
using Appulu.Web.Areas.Admin.Controllers;
using Appulu.Web.Framework.Kendoui;
using Appulu.Web.Framework.Mvc;
using Appulu.Web.Framework.Mvc.Filters;

namespace Appulu.Plugin.Payments.Worldpay.Controllers
{
    public class PaymentWorldpayController : BaseAdminController
    {
        #region Fields

        private readonly IUserService _userService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly ISettingService _settingService;
        private readonly WorldpayPaymentManager _worldpayPaymentManager;
        private readonly WorldpayPaymentSettings _worldpayPaymentSettings;

        #endregion

        #region Ctor

        public PaymentWorldpayController(IUserService userService,
            IGenericAttributeService genericAttributeService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            ISettingService settingService,
            WorldpayPaymentManager worldpayPaymentManager,
            WorldpayPaymentSettings worldpayPaymentSettings)
        {
            this._userService = userService;
            this._genericAttributeService = genericAttributeService;
            this._localizationService = localizationService;
            this._permissionService = permissionService;
            this._settingService = settingService;
            this._worldpayPaymentManager = worldpayPaymentManager;
            this._worldpayPaymentSettings = worldpayPaymentSettings;
        }

        #endregion

        #region Methods

        public IActionResult Configure()
        {
            //whether user has the authority
            if (!_permissionService.Authorize(StandardPermissionProvider.ManagePaymentMethods))
                return AccessDeniedView();

            //prepare model
            var model = new ConfigurationModel
            {
                SecureNetId = _worldpayPaymentSettings.SecureNetId,
                SecureKey = _worldpayPaymentSettings.SecureKey,
                PublicKey = _worldpayPaymentSettings.PublicKey,
                TransactionModeId = (int)_worldpayPaymentSettings.TransactionMode,
                TransactionModes = _worldpayPaymentSettings.TransactionMode.ToSelectList(),
                UseSandbox = _worldpayPaymentSettings.UseSandbox,
                ValidateAddress = _worldpayPaymentSettings.ValidateAddress,
                AdditionalFee = _worldpayPaymentSettings.AdditionalFee,
                AdditionalFeePercentage = _worldpayPaymentSettings.AdditionalFeePercentage
            };

            return View("~/Plugins/Payments.Worldpay/Views/Configure.cshtml", model);
        }

        [HttpPost]
        public IActionResult Configure(ConfigurationModel model)
        {
            //whether user has the authority
            if (!_permissionService.Authorize(StandardPermissionProvider.ManagePaymentMethods))
                return AccessDeniedView();

            if (!ModelState.IsValid)
                return Configure();

            //save settings
            _worldpayPaymentSettings.SecureNetId = model.SecureNetId;
            _worldpayPaymentSettings.SecureKey = model.SecureKey;
            _worldpayPaymentSettings.PublicKey = model.PublicKey;
            _worldpayPaymentSettings.TransactionMode = (TransactionMode)model.TransactionModeId;
            _worldpayPaymentSettings.UseSandbox = model.UseSandbox;
            _worldpayPaymentSettings.ValidateAddress = model.ValidateAddress;
            _worldpayPaymentSettings.AdditionalFee = model.AdditionalFee;
            _worldpayPaymentSettings.AdditionalFeePercentage = model.AdditionalFeePercentage;
            _settingService.SaveSetting(_worldpayPaymentSettings);

            SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

            return Configure();
        }

        [HttpPost]
        [AdminAntiForgery(true)]
        public IActionResult CreateUpdateUser(int userId, string worldpayUserId)
        {
            //whether user has the authority
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            //check whether user exists
            var user = _userService.GetUserById(userId);
            if (user == null)
                throw new ArgumentException("No user found with the specified id", nameof(userId));

            if (!ModelState.IsValid)
                return Json(new DataSourceResult { Errors = ModelState.SerializeErrors() });

            //ensure that user identifier not exceed 25 characters
            if ((worldpayUserId?.Length ?? 0) > 25)
                throw new AppException("Worldpay Vault error: User ID must not exceed 25 characters");

            //create request parameters to store a user in Vault
            var createUserRequest = new CreateUserRequest
            {
                UserId = worldpayUserId,
                UserDuplicateCheckType = UserDuplicateCheckType.Error,
                EmailReceiptEnabled = !string.IsNullOrEmpty(user.Email),
                Email = user.Email,
                FirstName = _genericAttributeService.GetAttribute<string>(user, AppUserDefaults.FirstNameAttribute),
                LastName = _genericAttributeService.GetAttribute<string>(user, AppUserDefaults.LastNameAttribute),
                Company = _genericAttributeService.GetAttribute<string>(user, AppUserDefaults.CompanyAttribute),
                Phone = _genericAttributeService.GetAttribute<string>(user, AppUserDefaults.PhoneAttribute),
                BillingAddress = new Address
                {
                    Line1 = user.BillingAddress?.Address1,
                    City = user.BillingAddress?.City,
                    State = user.BillingAddress?.StateProvince?.Abbreviation,
                    Country = user.BillingAddress?.Country?.TwoLetterIsoCode,
                    Zip = user.BillingAddress?.ZipPostalCode,
                    Company = user.BillingAddress?.Company,
                    Phone = user.BillingAddress?.PhoneNumber
                }
            };

            //check whether user is already stored in the Vault and try to store, if it is not so
            var vaultUser = _worldpayPaymentManager.GetUser(_genericAttributeService.GetAttribute<string>(user, WorldpayPaymentDefaults.UserIdAttribute))
                ?? _worldpayPaymentManager.CreateUser(createUserRequest);

            if (vaultUser == null)
                throw new AppException("Worldpay Vault error: Failed to create user. Error details in the log");

            //save Vault user identifier as a generic attribute
            _genericAttributeService.SaveAttribute(user, WorldpayPaymentDefaults.UserIdAttribute, vaultUser.UserId);

            //selected tab
            SaveSelectedTabName();

            return Json(new { Result = true });
        }

        [HttpPost]
        public IActionResult CardList(int userId)
        {
            //whether user has the authority
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedKendoGridJson();

            //check whether user exists
            var user = _userService.GetUserById(userId);
            if (user == null)
                throw new ArgumentException("No user found with the specified id", nameof(userId));

            //try to get stored credit cards of the Vault user
            var storedCards = _worldpayPaymentManager.GetUser(_genericAttributeService.GetAttribute<string>(user, WorldpayPaymentDefaults.UserIdAttribute))?
                .PaymentMethods?.Where(method => method?.Card != null).ToList() ?? new List<PaymentMethod>();

            //prepare grid model
            var gridModel = new DataSourceResult
            {
                Data = storedCards.Select(card => new WorldpayCardModel
                {
                    Id = card.PaymentId,
                    CardId = card.PaymentId,
                    CardType = card.Card.CreditCardType.HasValue ? _localizationService.GetLocalizedEnum(card.Card.CreditCardType.Value) : null,
                    ExpirationDate = card.Card.ExpirationDate,
                    MaskedNumber = card.Card.MaskedNumber
                }),
                Total = storedCards.Count
            };

            return Json(gridModel);
        }

        [HttpPost]
        public IActionResult CardDelete(string id, int userId)
        {
            //whether user has the authority
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            //check whether user exists
            var user = _userService.GetUserById(userId);
            if (user == null)
                throw new ArgumentException("No user found with the specified id", nameof(userId));

            //try to delete selected card from the Vault
            var deleted = _worldpayPaymentManager.Deletecard(new DeleteCardRequest
            {
                UserId = _genericAttributeService.GetAttribute<string>(user, WorldpayPaymentDefaults.UserIdAttribute),
                PaymentId = id
            });
            if (!deleted)
                throw new AppException("Worldpay Vault error: Failed to delete card. Error details in the log");

            return new NullJsonResult();
        }

        #endregion
    }
}