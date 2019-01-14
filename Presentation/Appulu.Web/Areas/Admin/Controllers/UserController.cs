using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Appulu.Core;
using Appulu.Core.Domain.Catalog;
using Appulu.Core.Domain.Common;
using Appulu.Core.Domain.Users;
using Appulu.Core.Domain.Forums;
using Appulu.Core.Domain.Gdpr;
using Appulu.Core.Domain.Messages;
using Appulu.Core.Domain.Tax;
using Appulu.Services.Common;
using Appulu.Services.Users;
using Appulu.Services.ExportImport;
using Appulu.Services.Forums;
using Appulu.Services.Gdpr;
using Appulu.Services.Helpers;
using Appulu.Services.Localization;
using Appulu.Services.Logging;
using Appulu.Services.Messages;
using Appulu.Services.Orders;
using Appulu.Services.Security;
using Appulu.Services.Stores;
using Appulu.Services.Tax;
using Appulu.Web.Areas.Admin.Factories;
using Appulu.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Appulu.Web.Areas.Admin.Models.Users;
using Appulu.Web.Framework.Controllers;
using Appulu.Web.Framework.Mvc;
using Appulu.Web.Framework.Mvc.Filters;

namespace Appulu.Web.Areas.Admin.Controllers
{
    public partial class UserController : BaseAdminController
    {
        #region Fields

        private readonly UserSettings _userSettings;
        private readonly DateTimeSettings _dateTimeSettings;
        private readonly EmailAccountSettings _emailAccountSettings;
        private readonly ForumSettings _forumSettings;
        private readonly GdprSettings _gdprSettings;
        private readonly IAddressAttributeParser _addressAttributeParser;
        private readonly IAddressService _addressService;
        private readonly IUserActivityService _userActivityService;
        private readonly IUserAttributeParser _userAttributeParser;
        private readonly IUserAttributeService _userAttributeService;
        private readonly IUserModelFactory _userModelFactory;
        private readonly IUserRegistrationService _userRegistrationService;
        private readonly IUserService _userService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IEmailAccountService _emailAccountService;
        private readonly IExportManager _exportManager;
        private readonly IForumService _forumService;
        private readonly IGdprService _gdprService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ILocalizationService _localizationService;
        private readonly INewsLetterSubscriptionService _newsLetterSubscriptionService;
        private readonly IPermissionService _permissionService;
        private readonly IQueuedEmailService _queuedEmailService;
        private readonly IRewardPointService _rewardPointService;
        private readonly IStoreContext _storeContext;
        private readonly IStoreService _storeService;
        private readonly ITaxService _taxService;
        private readonly IWorkContext _workContext;
        private readonly IWorkflowMessageService _workflowMessageService;
        private readonly TaxSettings _taxSettings;

        #endregion

        #region Ctor

        public UserController(UserSettings userSettings,
            DateTimeSettings dateTimeSettings,
            EmailAccountSettings emailAccountSettings,
            ForumSettings forumSettings,
            GdprSettings gdprSettings,
            IAddressAttributeParser addressAttributeParser,
            IAddressService addressService,
            IUserActivityService userActivityService,
            IUserAttributeParser userAttributeParser,
            IUserAttributeService userAttributeService,
            IUserModelFactory userModelFactory,
            IUserRegistrationService userRegistrationService,
            IUserService userService,
            IDateTimeHelper dateTimeHelper,
            IEmailAccountService emailAccountService,
            IExportManager exportManager,
            IForumService forumService,
            IGdprService gdprService,
            IGenericAttributeService genericAttributeService,
            ILocalizationService localizationService,
            INewsLetterSubscriptionService newsLetterSubscriptionService,
            IPermissionService permissionService,
            IQueuedEmailService queuedEmailService,
            IRewardPointService rewardPointService,
            IStoreContext storeContext,
            IStoreService storeService,
            ITaxService taxService,
            IWorkContext workContext,
            IWorkflowMessageService workflowMessageService,
            TaxSettings taxSettings)
        {
            this._userSettings = userSettings;
            this._dateTimeSettings = dateTimeSettings;
            this._emailAccountSettings = emailAccountSettings;
            this._forumSettings = forumSettings;
            this._gdprSettings = gdprSettings;
            this._addressAttributeParser = addressAttributeParser;
            this._addressService = addressService;
            this._userActivityService = userActivityService;
            this._userAttributeParser = userAttributeParser;
            this._userAttributeService = userAttributeService;
            this._userModelFactory = userModelFactory;
            this._userRegistrationService = userRegistrationService;
            this._userService = userService;
            this._dateTimeHelper = dateTimeHelper;
            this._emailAccountService = emailAccountService;
            this._exportManager = exportManager;
            this._forumService = forumService;
            this._gdprService = gdprService;
            this._genericAttributeService = genericAttributeService;
            this._localizationService = localizationService;
            this._newsLetterSubscriptionService = newsLetterSubscriptionService;
            this._permissionService = permissionService;
            this._queuedEmailService = queuedEmailService;
            this._rewardPointService = rewardPointService;
            this._storeContext = storeContext;
            this._storeService = storeService;
            this._taxService = taxService;
            this._workContext = workContext;
            this._workflowMessageService = workflowMessageService;
            this._taxSettings = taxSettings;
        }

        #endregion

        #region Utilities

        protected virtual string ValidateUserRoles(IList<UserRole> userRoles)
        {
            if (userRoles == null)
                throw new ArgumentNullException(nameof(userRoles));

            //ensure a user is not added to both 'Guests' and 'Registered' user roles
            //ensure that a user is in at least one required role ('Guests' and 'Registered')
            var isInGuestsRole = userRoles.FirstOrDefault(cr => cr.SystemName == AppUserDefaults.GuestsRoleName) != null;
            var isInRegisteredRole = userRoles.FirstOrDefault(cr => cr.SystemName == AppUserDefaults.RegisteredRoleName) != null;
            if (isInGuestsRole && isInRegisteredRole)
                return _localizationService.GetResource("Admin.Users.Users.GuestsAndRegisteredRolesError");
            if (!isInGuestsRole && !isInRegisteredRole)
                return _localizationService.GetResource("Admin.Users.Users.AddUserToGuestsOrRegisteredRoleError");

            //no errors
            return string.Empty;
        }

        protected virtual string ParseCustomUserAttributes(IFormCollection form)
        {
            if (form == null)
                throw new ArgumentNullException(nameof(form));

            var attributesXml = string.Empty;
            var userAttributes = _userAttributeService.GetAllUserAttributes();
            foreach (var attribute in userAttributes)
            {
                var controlId = $"user_attribute_{attribute.Id}";
                StringValues ctrlAttributes;

                switch (attribute.AttributeControlType)
                {
                    case AttributeControlType.DropdownList:
                    case AttributeControlType.RadioList:
                        ctrlAttributes = form[controlId];
                        if (!StringValues.IsNullOrEmpty(ctrlAttributes))
                        {
                            var selectedAttributeId = int.Parse(ctrlAttributes);
                            if (selectedAttributeId > 0)
                                attributesXml = _userAttributeParser.AddUserAttribute(attributesXml,
                                    attribute, selectedAttributeId.ToString());
                        }

                        break;
                    case AttributeControlType.Checkboxes:
                        var cblAttributes = form[controlId];
                        if (!StringValues.IsNullOrEmpty(cblAttributes))
                        {
                            foreach (var item in cblAttributes.ToString()
                                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                            {
                                var selectedAttributeId = int.Parse(item);
                                if (selectedAttributeId > 0)
                                    attributesXml = _userAttributeParser.AddUserAttribute(attributesXml,
                                        attribute, selectedAttributeId.ToString());
                            }
                        }

                        break;
                    case AttributeControlType.ReadonlyCheckboxes:
                        //load read-only (already server-side selected) values
                        var attributeValues = _userAttributeService.GetUserAttributeValues(attribute.Id);
                        foreach (var selectedAttributeId in attributeValues
                            .Where(v => v.IsPreSelected)
                            .Select(v => v.Id)
                            .ToList())
                        {
                            attributesXml = _userAttributeParser.AddUserAttribute(attributesXml,
                                attribute, selectedAttributeId.ToString());
                        }

                        break;
                    case AttributeControlType.TextBox:
                    case AttributeControlType.MultilineTextbox:
                        ctrlAttributes = form[controlId];
                        if (!StringValues.IsNullOrEmpty(ctrlAttributes))
                        {
                            var enteredText = ctrlAttributes.ToString().Trim();
                            attributesXml = _userAttributeParser.AddUserAttribute(attributesXml,
                                attribute, enteredText);
                        }

                        break;
                    case AttributeControlType.Datepicker:
                    case AttributeControlType.ColorSquares:
                    case AttributeControlType.ImageSquares:
                    case AttributeControlType.FileUpload:
                    //not supported user attributes
                    default:
                        break;
                }
            }

            return attributesXml;
        }

        private bool SecondAdminAccountExists(User user)
        {
            var users = _userService.GetAllUsers(userRoleIds: new[] { _userService.GetUserRoleBySystemName(AppUserDefaults.AdministratorsRoleName).Id });

            return users.Any(c => c.Active && c.Id != user.Id);
        }

        #endregion

        #region Users

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            //prepare model
            var model = _userModelFactory.PrepareUserSearchModel(new UserSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult UserList(UserSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedKendoGridJson();

            //prepare model
            var model = _userModelFactory.PrepareUserListModel(searchModel);

            return Json(model);
        }

        public virtual IActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            //prepare model
            var model = _userModelFactory.PrepareUserModel(new UserModel(), null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public virtual IActionResult Create(UserModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            if (!string.IsNullOrWhiteSpace(model.Email) && _userService.GetUserByEmail(model.Email) != null)
                ModelState.AddModelError(string.Empty, "Email is already registered");

            if (!string.IsNullOrWhiteSpace(model.Username) && _userSettings.UsernamesEnabled &&
                _userService.GetUserByUsername(model.Username) != null)
            {
                ModelState.AddModelError(string.Empty, "Username is already registered");
            }

            //validate user roles
            var allUserRoles = _userService.GetAllUserRoles(true);
            var newUserRoles = new List<UserRole>();
            foreach (var userRole in allUserRoles)
                if (model.SelectedUserRoleIds.Contains(userRole.Id))
                    newUserRoles.Add(userRole);
            var userRolesError = ValidateUserRoles(newUserRoles);
            if (!string.IsNullOrEmpty(userRolesError))
            {
                ModelState.AddModelError(string.Empty, userRolesError);
                ErrorNotification(userRolesError, false);
            }

            // Ensure that valid email address is entered if Registered role is checked to avoid registered users with empty email address
            if (newUserRoles.Any() && newUserRoles.FirstOrDefault(c => c.SystemName == AppUserDefaults.RegisteredRoleName) != null &&
                !CommonHelper.IsValidEmail(model.Email))
            {
                ModelState.AddModelError(string.Empty, _localizationService.GetResource("Admin.Users.Users.ValidEmailRequiredRegisteredRole"));
                ErrorNotification(_localizationService.GetResource("Admin.Users.Users.ValidEmailRequiredRegisteredRole"), false);
            }

            //custom user attributes
            var userAttributesXml = ParseCustomUserAttributes(model.Form);
            if (newUserRoles.Any() && newUserRoles.FirstOrDefault(c => c.SystemName == AppUserDefaults.RegisteredRoleName) != null)
            {
                var userAttributeWarnings = _userAttributeParser.GetAttributeWarnings(userAttributesXml);
                foreach (var error in userAttributeWarnings)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }

            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserGuid = Guid.NewGuid(),
                    Email = model.Email,
                    Username = model.Username,
                    VendorId = model.VendorId,
                    AdminComment = model.AdminComment,
                    IsTaxExempt = model.IsTaxExempt,
                    Active = model.Active,
                    CreatedOnUtc = DateTime.UtcNow,
                    LastActivityDateUtc = DateTime.UtcNow,
                    RegisteredInStoreId = _storeContext.CurrentStore.Id
                };
                _userService.InsertUser(user);

                //form fields
                if (_dateTimeSettings.AllowUsersToSetTimeZone)
                    _genericAttributeService.SaveAttribute(user, AppUserDefaults.TimeZoneIdAttribute, model.TimeZoneId);
                if (_userSettings.GenderEnabled)
                    _genericAttributeService.SaveAttribute(user, AppUserDefaults.GenderAttribute, model.Gender);
                _genericAttributeService.SaveAttribute(user, AppUserDefaults.FirstNameAttribute, model.FirstName);
                _genericAttributeService.SaveAttribute(user, AppUserDefaults.LastNameAttribute, model.LastName);
                if (_userSettings.DateOfBirthEnabled)
                    _genericAttributeService.SaveAttribute(user, AppUserDefaults.DateOfBirthAttribute, model.DateOfBirth);
                if (_userSettings.CompanyEnabled)
                    _genericAttributeService.SaveAttribute(user, AppUserDefaults.CompanyAttribute, model.Company);
                if (_userSettings.StreetAddressEnabled)
                    _genericAttributeService.SaveAttribute(user, AppUserDefaults.StreetAddressAttribute, model.StreetAddress);
                if (_userSettings.StreetAddress2Enabled)
                    _genericAttributeService.SaveAttribute(user, AppUserDefaults.StreetAddress2Attribute, model.StreetAddress2);
                if (_userSettings.ZipPostalCodeEnabled)
                    _genericAttributeService.SaveAttribute(user, AppUserDefaults.ZipPostalCodeAttribute, model.ZipPostalCode);
                if (_userSettings.CityEnabled)
                    _genericAttributeService.SaveAttribute(user, AppUserDefaults.CityAttribute, model.City);
                if (_userSettings.CountyEnabled)
                    _genericAttributeService.SaveAttribute(user, AppUserDefaults.CountyAttribute, model.County);
                if (_userSettings.CountryEnabled)
                    _genericAttributeService.SaveAttribute(user, AppUserDefaults.CountryIdAttribute, model.CountryId);
                if (_userSettings.CountryEnabled && _userSettings.StateProvinceEnabled)
                    _genericAttributeService.SaveAttribute(user, AppUserDefaults.StateProvinceIdAttribute, model.StateProvinceId);
                if (_userSettings.PhoneEnabled)
                    _genericAttributeService.SaveAttribute(user, AppUserDefaults.PhoneAttribute, model.Phone);
                if (_userSettings.FaxEnabled)
                    _genericAttributeService.SaveAttribute(user, AppUserDefaults.FaxAttribute, model.Fax);

                //custom user attributes
                _genericAttributeService.SaveAttribute(user, AppUserDefaults.CustomUserAttributes, userAttributesXml);

                //newsletter subscriptions
                if (!string.IsNullOrEmpty(user.Email))
                {
                    var allStores = _storeService.GetAllStores();
                    foreach (var store in allStores)
                    {
                        var newsletterSubscription = _newsLetterSubscriptionService
                            .GetNewsLetterSubscriptionByEmailAndStoreId(user.Email, store.Id);
                        if (model.SelectedNewsletterSubscriptionStoreIds != null &&
                            model.SelectedNewsletterSubscriptionStoreIds.Contains(store.Id))
                        {
                            //subscribed
                            if (newsletterSubscription == null)
                            {
                                _newsLetterSubscriptionService.InsertNewsLetterSubscription(new NewsLetterSubscription
                                {
                                    NewsLetterSubscriptionGuid = Guid.NewGuid(),
                                    Email = user.Email,
                                    Active = true,
                                    StoreId = store.Id,
                                    CreatedOnUtc = DateTime.UtcNow
                                });
                            }
                        }
                        else
                        {
                            //not subscribed
                            if (newsletterSubscription != null)
                            {
                                _newsLetterSubscriptionService.DeleteNewsLetterSubscription(newsletterSubscription);
                            }
                        }
                    }
                }

                //password
                if (!string.IsNullOrWhiteSpace(model.Password))
                {
                    var changePassRequest = new ChangePasswordRequest(model.Email, false, _userSettings.DefaultPasswordFormat, model.Password);
                    var changePassResult = _userRegistrationService.ChangePassword(changePassRequest);
                    if (!changePassResult.Success)
                    {
                        foreach (var changePassError in changePassResult.Errors)
                            ErrorNotification(changePassError);
                    }
                }

                //user roles
                foreach (var userRole in newUserRoles)
                {
                    //ensure that the current user cannot add to "Administrators" system role if he's not an admin himself
                    if (userRole.SystemName == AppUserDefaults.AdministratorsRoleName && !_workContext.CurrentUser.IsAdmin())
                        continue;

                    user.UserUserRoleMappings.Add(new UserUserRoleMapping { UserRole = userRole });
                }

                _userService.UpdateUser(user);

                //ensure that a user with a vendor associated is not in "Administrators" role
                //otherwise, he won't have access to other functionality in admin area
                if (user.IsAdmin() && user.VendorId > 0)
                {
                    user.VendorId = 0;
                    _userService.UpdateUser(user);
                    ErrorNotification(_localizationService.GetResource("Admin.Users.Users.AdminCouldNotbeVendor"));
                }

                //ensure that a user in the Vendors role has a vendor account associated.
                //otherwise, he will have access to ALL products
                if (user.IsVendor() && user.VendorId == 0)
                {
                    var vendorRole = user
                        .UserRoles
                        .FirstOrDefault(x => x.SystemName == AppUserDefaults.VendorsRoleName);
                    //user.UserRoles.Remove(vendorRole);
                    user.UserUserRoleMappings
                        .Remove(user.UserUserRoleMappings.FirstOrDefault(mapping => mapping.UserRoleId == vendorRole.Id));
                    _userService.UpdateUser(user);
                    ErrorNotification(_localizationService.GetResource("Admin.Users.Users.CannotBeInVendoRoleWithoutVendorAssociated"));
                }

                //activity log
                _userActivityService.InsertActivity("AddNewUser",
                    string.Format(_localizationService.GetResource("ActivityLog.AddNewUser"), user.Id), user);

                SuccessNotification(_localizationService.GetResource("Admin.Users.Users.Added"));

                if (!continueEditing)
                    return RedirectToAction("List");

                //selected tab
                SaveSelectedTabName();

                return RedirectToAction("Edit", new { id = user.Id });
            }

            //prepare model
            model = _userModelFactory.PrepareUserModel(model, null, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual IActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            //try to get a user with the specified id
            var user = _userService.GetUserById(id);
            if (user == null || user.Deleted)
                return RedirectToAction("List");

            //prepare model
            var model = _userModelFactory.PrepareUserModel(null, user);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public virtual IActionResult Edit(UserModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            //try to get a user with the specified id
            var user = _userService.GetUserById(model.Id);
            if (user == null || user.Deleted)
                return RedirectToAction("List");

            //validate user roles
            var allUserRoles = _userService.GetAllUserRoles(true);
            var newUserRoles = new List<UserRole>();
            foreach (var userRole in allUserRoles)
                if (model.SelectedUserRoleIds.Contains(userRole.Id))
                    newUserRoles.Add(userRole);
            var userRolesError = ValidateUserRoles(newUserRoles);
            if (!string.IsNullOrEmpty(userRolesError))
            {
                ModelState.AddModelError(string.Empty, userRolesError);
                ErrorNotification(userRolesError, false);
            }

            // Ensure that valid email address is entered if Registered role is checked to avoid registered users with empty email address
            if (newUserRoles.Any() && newUserRoles.FirstOrDefault(c => c.SystemName == AppUserDefaults.RegisteredRoleName) != null &&
                !CommonHelper.IsValidEmail(model.Email))
            {
                ModelState.AddModelError(string.Empty, _localizationService.GetResource("Admin.Users.Users.ValidEmailRequiredRegisteredRole"));
                ErrorNotification(_localizationService.GetResource("Admin.Users.Users.ValidEmailRequiredRegisteredRole"), false);
            }

            //custom user attributes
            var userAttributesXml = ParseCustomUserAttributes(model.Form);
            if (newUserRoles.Any() && newUserRoles.FirstOrDefault(c => c.SystemName == AppUserDefaults.RegisteredRoleName) != null)
            {
                var userAttributeWarnings = _userAttributeParser.GetAttributeWarnings(userAttributesXml);
                foreach (var error in userAttributeWarnings)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    user.AdminComment = model.AdminComment;
                    user.IsTaxExempt = model.IsTaxExempt;

                    //prevent deactivation of the last active administrator
                    if (!user.IsAdmin() || model.Active || SecondAdminAccountExists(user))
                        user.Active = model.Active;
                    else
                        ErrorNotification(_localizationService.GetResource("Admin.Users.Users.AdminAccountShouldExists.Deactivate"));

                    //email
                    if (!string.IsNullOrWhiteSpace(model.Email))
                        _userRegistrationService.SetEmail(user, model.Email, false);
                    else
                        user.Email = model.Email;

                    //username
                    if (_userSettings.UsernamesEnabled)
                    {
                        if (!string.IsNullOrWhiteSpace(model.Username))
                            _userRegistrationService.SetUsername(user, model.Username);
                        else
                            user.Username = model.Username;
                    }

                    //VAT number
                    if (_taxSettings.EuVatEnabled)
                    {
                        var prevVatNumber = _genericAttributeService.GetAttribute<string>(user, AppUserDefaults.VatNumberAttribute);

                        _genericAttributeService.SaveAttribute(user, AppUserDefaults.VatNumberAttribute, model.VatNumber);
                        //set VAT number status
                        if (!string.IsNullOrEmpty(model.VatNumber))
                        {
                            if (!model.VatNumber.Equals(prevVatNumber, StringComparison.InvariantCultureIgnoreCase))
                            {
                                _genericAttributeService.SaveAttribute(user,
                                    AppUserDefaults.VatNumberStatusIdAttribute,
                                    (int)_taxService.GetVatNumberStatus(model.VatNumber));
                            }
                        }
                        else
                        {
                            _genericAttributeService.SaveAttribute(user,
                                AppUserDefaults.VatNumberStatusIdAttribute,
                                (int)VatNumberStatus.Empty);
                        }
                    }

                    //vendor
                    user.VendorId = model.VendorId;

                    //form fields
                    if (_dateTimeSettings.AllowUsersToSetTimeZone)
                        _genericAttributeService.SaveAttribute(user, AppUserDefaults.TimeZoneIdAttribute, model.TimeZoneId);
                    if (_userSettings.GenderEnabled)
                        _genericAttributeService.SaveAttribute(user, AppUserDefaults.GenderAttribute, model.Gender);
                    _genericAttributeService.SaveAttribute(user, AppUserDefaults.FirstNameAttribute, model.FirstName);
                    _genericAttributeService.SaveAttribute(user, AppUserDefaults.LastNameAttribute, model.LastName);
                    if (_userSettings.DateOfBirthEnabled)
                        _genericAttributeService.SaveAttribute(user, AppUserDefaults.DateOfBirthAttribute, model.DateOfBirth);
                    if (_userSettings.CompanyEnabled)
                        _genericAttributeService.SaveAttribute(user, AppUserDefaults.CompanyAttribute, model.Company);
                    if (_userSettings.StreetAddressEnabled)
                        _genericAttributeService.SaveAttribute(user, AppUserDefaults.StreetAddressAttribute, model.StreetAddress);
                    if (_userSettings.StreetAddress2Enabled)
                        _genericAttributeService.SaveAttribute(user, AppUserDefaults.StreetAddress2Attribute, model.StreetAddress2);
                    if (_userSettings.ZipPostalCodeEnabled)
                        _genericAttributeService.SaveAttribute(user, AppUserDefaults.ZipPostalCodeAttribute, model.ZipPostalCode);
                    if (_userSettings.CityEnabled)
                        _genericAttributeService.SaveAttribute(user, AppUserDefaults.CityAttribute, model.City);
                    if (_userSettings.CountyEnabled)
                        _genericAttributeService.SaveAttribute(user, AppUserDefaults.CountyAttribute, model.County);
                    if (_userSettings.CountryEnabled)
                        _genericAttributeService.SaveAttribute(user, AppUserDefaults.CountryIdAttribute, model.CountryId);
                    if (_userSettings.CountryEnabled && _userSettings.StateProvinceEnabled)
                        _genericAttributeService.SaveAttribute(user, AppUserDefaults.StateProvinceIdAttribute, model.StateProvinceId);
                    if (_userSettings.PhoneEnabled)
                        _genericAttributeService.SaveAttribute(user, AppUserDefaults.PhoneAttribute, model.Phone);
                    if (_userSettings.FaxEnabled)
                        _genericAttributeService.SaveAttribute(user, AppUserDefaults.FaxAttribute, model.Fax);

                    //custom user attributes
                    _genericAttributeService.SaveAttribute(user, AppUserDefaults.CustomUserAttributes, userAttributesXml);

                    //newsletter subscriptions
                    if (!string.IsNullOrEmpty(user.Email))
                    {
                        var allStores = _storeService.GetAllStores();
                        foreach (var store in allStores)
                        {
                            var newsletterSubscription = _newsLetterSubscriptionService
                                .GetNewsLetterSubscriptionByEmailAndStoreId(user.Email, store.Id);
                            if (model.SelectedNewsletterSubscriptionStoreIds != null &&
                                model.SelectedNewsletterSubscriptionStoreIds.Contains(store.Id))
                            {
                                //subscribed
                                if (newsletterSubscription == null)
                                {
                                    _newsLetterSubscriptionService.InsertNewsLetterSubscription(new NewsLetterSubscription
                                    {
                                        NewsLetterSubscriptionGuid = Guid.NewGuid(),
                                        Email = user.Email,
                                        Active = true,
                                        StoreId = store.Id,
                                        CreatedOnUtc = DateTime.UtcNow
                                    });
                                }
                            }
                            else
                            {
                                //not subscribed
                                if (newsletterSubscription != null)
                                {
                                    _newsLetterSubscriptionService.DeleteNewsLetterSubscription(newsletterSubscription);
                                }
                            }
                        }
                    }

                    //user roles
                    foreach (var userRole in allUserRoles)
                    {
                        //ensure that the current user cannot add/remove to/from "Administrators" system role
                        //if he's not an admin himself
                        if (userRole.SystemName == AppUserDefaults.AdministratorsRoleName &&
                            !_workContext.CurrentUser.IsAdmin())
                            continue;

                        if (model.SelectedUserRoleIds.Contains(userRole.Id))
                        {
                            //new role
                            if (user.UserUserRoleMappings.Count(mapping => mapping.UserRoleId == userRole.Id) == 0)
                                user.UserUserRoleMappings.Add(new UserUserRoleMapping { UserRole = userRole });
                        }
                        else
                        {
                            //prevent attempts to delete the administrator role from the user, if the user is the last active administrator
                            if (userRole.SystemName == AppUserDefaults.AdministratorsRoleName && !SecondAdminAccountExists(user))
                            {
                                ErrorNotification(_localizationService.GetResource("Admin.Users.Users.AdminAccountShouldExists.DeleteRole"));
                                continue;
                            }

                            //remove role
                            if (user.UserUserRoleMappings.Count(mapping => mapping.UserRoleId == userRole.Id) > 0)
                            {
                                user.UserUserRoleMappings
                                    .Remove(user.UserUserRoleMappings.FirstOrDefault(mapping => mapping.UserRoleId == userRole.Id));
                            }
                        }
                    }

                    _userService.UpdateUser(user);

                    //ensure that a user with a vendor associated is not in "Administrators" role
                    //otherwise, he won't have access to the other functionality in admin area
                    if (user.IsAdmin() && user.VendorId > 0)
                    {
                        user.VendorId = 0;
                        _userService.UpdateUser(user);
                        ErrorNotification(_localizationService.GetResource("Admin.Users.Users.AdminCouldNotbeVendor"));
                    }

                    //ensure that a user in the Vendors role has a vendor account associated.
                    //otherwise, he will have access to ALL products
                    if (user.IsVendor() && user.VendorId == 0)
                    {
                        var vendorRole = user
                            .UserRoles
                            .FirstOrDefault(x => x.SystemName == AppUserDefaults.VendorsRoleName);
                        //user.UserRoles.Remove(vendorRole);
                        user.UserUserRoleMappings
                            .Remove(user.UserUserRoleMappings.FirstOrDefault(mapping => mapping.UserRoleId == vendorRole.Id));
                        _userService.UpdateUser(user);
                        ErrorNotification(_localizationService.GetResource("Admin.Users.Users.CannotBeInVendoRoleWithoutVendorAssociated"));
                    }

                    //activity log
                    _userActivityService.InsertActivity("EditUser",
                        string.Format(_localizationService.GetResource("ActivityLog.EditUser"), user.Id), user);

                    SuccessNotification(_localizationService.GetResource("Admin.Users.Users.Updated"));

                    if (!continueEditing)
                        return RedirectToAction("List");

                    //selected tab
                    SaveSelectedTabName();

                    return RedirectToAction("Edit", new { id = user.Id });
                }
                catch (Exception exc)
                {
                    ErrorNotification(exc.Message, false);
                }
            }

            //prepare model
            model = _userModelFactory.PrepareUserModel(model, user, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        [FormValueRequired("changepassword")]
        public virtual IActionResult ChangePassword(UserModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            //try to get a user with the specified id
            var user = _userService.GetUserById(model.Id);
            if (user == null)
                return RedirectToAction("List");

            //ensure that the current user cannot change passwords of "Administrators" if he's not an admin himself
            if (user.IsAdmin() && !_workContext.CurrentUser.IsAdmin())
            {
                ErrorNotification(_localizationService.GetResource("Admin.Users.Users.OnlyAdminCanChangePassword"));
                return RedirectToAction("Edit", new { id = user.Id });
            }

            if (!ModelState.IsValid)
                return RedirectToAction("Edit", new { id = user.Id });

            var changePassRequest = new ChangePasswordRequest(model.Email,
                false, _userSettings.DefaultPasswordFormat, model.Password);
            var changePassResult = _userRegistrationService.ChangePassword(changePassRequest);
            if (changePassResult.Success)
                SuccessNotification(_localizationService.GetResource("Admin.Users.Users.PasswordChanged"));
            else
                foreach (var error in changePassResult.Errors)
                    ErrorNotification(error);

            return RedirectToAction("Edit", new { id = user.Id });
        }

        [HttpPost, ActionName("Edit")]
        [FormValueRequired("markVatNumberAsValid")]
        public virtual IActionResult MarkVatNumberAsValid(UserModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            //try to get a user with the specified id
            var user = _userService.GetUserById(model.Id);
            if (user == null)
                return RedirectToAction("List");

            _genericAttributeService.SaveAttribute(user,
                AppUserDefaults.VatNumberStatusIdAttribute,
                (int)VatNumberStatus.Valid);

            return RedirectToAction("Edit", new { id = user.Id });
        }

        [HttpPost, ActionName("Edit")]
        [FormValueRequired("markVatNumberAsInvalid")]
        public virtual IActionResult MarkVatNumberAsInvalid(UserModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            //try to get a user with the specified id
            var user = _userService.GetUserById(model.Id);
            if (user == null)
                return RedirectToAction("List");

            _genericAttributeService.SaveAttribute(user,
                AppUserDefaults.VatNumberStatusIdAttribute,
                (int)VatNumberStatus.Invalid);

            return RedirectToAction("Edit", new { id = user.Id });
        }

        [HttpPost, ActionName("Edit")]
        [FormValueRequired("remove-affiliate")]
        public virtual IActionResult RemoveAffiliate(UserModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            //try to get a user with the specified id
            var user = _userService.GetUserById(model.Id);
            if (user == null)
                return RedirectToAction("List");

            user.AffiliateId = 0;
            _userService.UpdateUser(user);

            return RedirectToAction("Edit", new { id = user.Id });
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            //try to get a user with the specified id
            var user = _userService.GetUserById(id);
            if (user == null)
                return RedirectToAction("List");

            try
            {
                //prevent attempts to delete the user, if it is the last active administrator
                if (user.IsAdmin() && !SecondAdminAccountExists(user))
                {
                    ErrorNotification(_localizationService.GetResource("Admin.Users.Users.AdminAccountShouldExists.DeleteAdministrator"));
                    return RedirectToAction("Edit", new { id = user.Id });
                }

                //ensure that the current user cannot delete "Administrators" if he's not an admin himself
                if (user.IsAdmin() && !_workContext.CurrentUser.IsAdmin())
                {
                    ErrorNotification(_localizationService.GetResource("Admin.Users.Users.OnlyAdminCanDeleteAdmin"));
                    return RedirectToAction("Edit", new { id = user.Id });
                }

                //delete
                _userService.DeleteUser(user);

                //remove newsletter subscription (if exists)
                foreach (var store in _storeService.GetAllStores())
                {
                    var subscription = _newsLetterSubscriptionService.GetNewsLetterSubscriptionByEmailAndStoreId(user.Email, store.Id);
                    if (subscription != null)
                        _newsLetterSubscriptionService.DeleteNewsLetterSubscription(subscription);
                }

                //activity log
                _userActivityService.InsertActivity("DeleteUser",
                    string.Format(_localizationService.GetResource("ActivityLog.DeleteUser"), user.Id), user);

                SuccessNotification(_localizationService.GetResource("Admin.Users.Users.Deleted"));

                return RedirectToAction("List");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc.Message);
                return RedirectToAction("Edit", new { id = user.Id });
            }
        }

        [HttpPost, ActionName("Edit")]
        [FormValueRequired("impersonate")]
        public virtual IActionResult Impersonate(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.AllowUserImpersonation))
                return AccessDeniedView();

            //try to get a user with the specified id
            var user = _userService.GetUserById(id);
            if (user == null)
                return RedirectToAction("List");

            //ensure that a non-admin user cannot impersonate as an administrator
            //otherwise, that user can simply impersonate as an administrator and gain additional administrative privileges
            if (!_workContext.CurrentUser.IsAdmin() && user.IsAdmin())
            {
                ErrorNotification(_localizationService.GetResource("Admin.Users.Users.NonAdminNotImpersonateAsAdminError"));
                return RedirectToAction("Edit", user.Id);
            }

            //activity log
            _userActivityService.InsertActivity("Impersonation.Started",
                string.Format(_localizationService.GetResource("ActivityLog.Impersonation.Started.StoreOwner"), user.Email, user.Id), user);
            _userActivityService.InsertActivity(user, "Impersonation.Started",
                string.Format(_localizationService.GetResource("ActivityLog.Impersonation.Started.User"), _workContext.CurrentUser.Email, _workContext.CurrentUser.Id), _workContext.CurrentUser);

            //ensure login is not required
            user.RequireReLogin = false;
            _userService.UpdateUser(user);
            _genericAttributeService.SaveAttribute<int?>(_workContext.CurrentUser, AppUserDefaults.ImpersonatedUserIdAttribute, user.Id);

            return RedirectToAction("Index", "Home", new { area = string.Empty });
        }

        [HttpPost, ActionName("Edit")]
        [FormValueRequired("send-welcome-message")]
        public virtual IActionResult SendWelcomeMessage(UserModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            //try to get a user with the specified id
            var user = _userService.GetUserById(model.Id);
            if (user == null)
                return RedirectToAction("List");

            _workflowMessageService.SendUserWelcomeMessage(user, _workContext.WorkingLanguage.Id);

            SuccessNotification(_localizationService.GetResource("Admin.Users.Users.SendWelcomeMessage.Success"));

            return RedirectToAction("Edit", new { id = user.Id });
        }

        [HttpPost, ActionName("Edit")]
        [FormValueRequired("resend-activation-message")]
        public virtual IActionResult ReSendActivationMessage(UserModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            //try to get a user with the specified id
            var user = _userService.GetUserById(model.Id);
            if (user == null)
                return RedirectToAction("List");

            //email validation message
            _genericAttributeService.SaveAttribute(user, AppUserDefaults.AccountActivationTokenAttribute, Guid.NewGuid().ToString());
            _workflowMessageService.SendUserEmailValidationMessage(user, _workContext.WorkingLanguage.Id);

            SuccessNotification(_localizationService.GetResource("Admin.Users.Users.ReSendActivationMessage.Success"));

            return RedirectToAction("Edit", new { id = user.Id });
        }

        public virtual IActionResult SendEmail(UserModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            //try to get a user with the specified id
            var user = _userService.GetUserById(model.Id);
            if (user == null)
                return RedirectToAction("List");

            try
            {
                if (string.IsNullOrWhiteSpace(user.Email))
                    throw new AppException("User email is empty");
                if (!CommonHelper.IsValidEmail(user.Email))
                    throw new AppException("User email is not valid");
                if (string.IsNullOrWhiteSpace(model.SendEmail.Subject))
                    throw new AppException("Email subject is empty");
                if (string.IsNullOrWhiteSpace(model.SendEmail.Body))
                    throw new AppException("Email body is empty");

                var emailAccount = _emailAccountService.GetEmailAccountById(_emailAccountSettings.DefaultEmailAccountId);
                if (emailAccount == null)
                    emailAccount = _emailAccountService.GetAllEmailAccounts().FirstOrDefault();
                if (emailAccount == null)
                    throw new AppException("Email account can't be loaded");
                var email = new QueuedEmail
                {
                    Priority = QueuedEmailPriority.High,
                    EmailAccountId = emailAccount.Id,
                    FromName = emailAccount.DisplayName,
                    From = emailAccount.Email,
                    ToName = _userService.GetUserFullName(user),
                    To = user.Email,
                    Subject = model.SendEmail.Subject,
                    Body = model.SendEmail.Body,
                    CreatedOnUtc = DateTime.UtcNow,
                    DontSendBeforeDateUtc = model.SendEmail.SendImmediately || !model.SendEmail.DontSendBeforeDate.HasValue ?
                        null : (DateTime?)_dateTimeHelper.ConvertToUtcTime(model.SendEmail.DontSendBeforeDate.Value)
                };
                _queuedEmailService.InsertQueuedEmail(email);

                SuccessNotification(_localizationService.GetResource("Admin.Users.Users.SendEmail.Queued"));
            }
            catch (Exception exc)
            {
                ErrorNotification(exc.Message);
            }

            return RedirectToAction("Edit", new { id = user.Id });
        }

        public virtual IActionResult SendPm(UserModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            //try to get a user with the specified id
            var user = _userService.GetUserById(model.Id);
            if (user == null)
                return RedirectToAction("List");

            try
            {
                if (!_forumSettings.AllowPrivateMessages)
                    throw new AppException("Private messages are disabled");
                if (user.IsGuest())
                    throw new AppException("User should be registered");
                if (string.IsNullOrWhiteSpace(model.SendPm.Subject))
                    throw new AppException("PM subject is empty");
                if (string.IsNullOrWhiteSpace(model.SendPm.Message))
                    throw new AppException("PM message is empty");

                var privateMessage = new PrivateMessage
                {
                    StoreId = _storeContext.CurrentStore.Id,
                    ToUserId = user.Id,
                    FromUserId = _workContext.CurrentUser.Id,
                    Subject = model.SendPm.Subject,
                    Text = model.SendPm.Message,
                    IsDeletedByAuthor = false,
                    IsDeletedByRecipient = false,
                    IsRead = false,
                    CreatedOnUtc = DateTime.UtcNow
                };

                _forumService.InsertPrivateMessage(privateMessage);

                SuccessNotification(_localizationService.GetResource("Admin.Users.Users.SendPM.Sent"));
            }
            catch (Exception exc)
            {
                ErrorNotification(exc.Message);
            }

            return RedirectToAction("Edit", new { id = user.Id });
        }

        #endregion

        #region Reward points history

        [HttpPost]
        public virtual IActionResult RewardPointsHistorySelect(UserRewardPointsSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedKendoGridJson();

            //try to get a user with the specified id
            var user = _userService.GetUserById(searchModel.UserId)
                ?? throw new ArgumentException("No user found with the specified id");

            //prepare model
            var model = _userModelFactory.PrepareRewardPointsListModel(searchModel, user);

            return Json(model);
        }

        public virtual IActionResult RewardPointsHistoryAdd(AddRewardPointsToUserModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            //prevent adding a new row with zero value
            if (model.Points == 0)
                return Json(new { Result = false, Error = _localizationService.GetResource("Admin.Users.Users.RewardPoints.AddingZeroValueNotAllowed") });

            //try to get a user with the specified id
            var user = _userService.GetUserById(model.UserId);
            if (user == null)
                return Json(new { Result = false });

            //check whether delay is set
            DateTime? activatingDate = null;
            if (!model.ActivatePointsImmediately && model.ActivationDelay > 0)
            {
                var delayPeriod = (RewardPointsActivatingDelayPeriod)model.ActivationDelayPeriodId;
                var delayInHours = delayPeriod.ToHours(model.ActivationDelay);
                activatingDate = DateTime.UtcNow.AddHours(delayInHours);
            }

            //whether points validity is set
            DateTime? endDate = null;
            if (model.PointsValidity > 0)
                endDate = (activatingDate ?? DateTime.UtcNow).AddDays(model.PointsValidity.Value);

            //add reward points
            _rewardPointService.AddRewardPointsHistoryEntry(user, model.Points, model.StoreId, model.Message,
                activatingDate: activatingDate, endDate: endDate);

            return Json(new { Result = true });
        }

        #endregion

        #region Addresses

        [HttpPost]
        public virtual IActionResult AddressesSelect(UserAddressSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedKendoGridJson();

            //try to get a user with the specified id
            var user = _userService.GetUserById(searchModel.UserId)
                ?? throw new ArgumentException("No user found with the specified id");

            //prepare model
            var model = _userModelFactory.PrepareUserAddressListModel(searchModel, user);

            return Json(model);
        }

        [HttpPost]
        public virtual IActionResult AddressDelete(int id, int userId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            //try to get a user with the specified id
            var user = _userService.GetUserById(userId)
                ?? throw new ArgumentException("No user found with the specified id", nameof(userId));

            //try to get an address with the specified id
            var address = user.Addresses.FirstOrDefault(a => a.Id == id);
            if (address == null)
                return Content("No address found with the specified id");

            _userService.RemoveUserAddress(user, address);
            _userService.UpdateUser(user);

            //now delete the address record
            _addressService.DeleteAddress(address);

            return new NullJsonResult();
        }

        public virtual IActionResult AddressCreate(int userId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            //try to get a user with the specified id
            var user = _userService.GetUserById(userId);
            if (user == null)
                return RedirectToAction("List");

            //prepare model
            var model = _userModelFactory.PrepareUserAddressModel(new UserAddressModel(), user, null);

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult AddressCreate(UserAddressModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            //try to get a user with the specified id
            var user = _userService.GetUserById(model.UserId);
            if (user == null)
                return RedirectToAction("List");

            //custom address attributes
            var customAttributes = _addressAttributeParser.ParseCustomAddressAttributes(model.Form);
            var customAttributeWarnings = _addressAttributeParser.GetAttributeWarnings(customAttributes);
            foreach (var error in customAttributeWarnings)
            {
                ModelState.AddModelError(string.Empty, error);
            }

            if (ModelState.IsValid)
            {
                var address = model.Address.ToEntity<Address>();
                address.CustomAttributes = customAttributes;
                address.CreatedOnUtc = DateTime.UtcNow;

                //some validation
                if (address.CountryId == 0)
                    address.CountryId = null;
                if (address.StateProvinceId == 0)
                    address.StateProvinceId = null;
                //user.Addresses.Add(address);
                user.UserAddressMappings.Add(new UserAddressMapping { Address = address });
                _userService.UpdateUser(user);

                SuccessNotification(_localizationService.GetResource("Admin.Users.Users.Addresses.Added"));

                return RedirectToAction("AddressEdit", new { addressId = address.Id, userId = model.UserId });
            }

            //prepare model
            model = _userModelFactory.PrepareUserAddressModel(model, user, null, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual IActionResult AddressEdit(int addressId, int userId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            //try to get a user with the specified id
            var user = _userService.GetUserById(userId);
            if (user == null)
                return RedirectToAction("List");

            //try to get an address with the specified id
            var address = _addressService.GetAddressById(addressId);
            if (address == null)
                return RedirectToAction("Edit", new { id = user.Id });

            //prepare model
            var model = _userModelFactory.PrepareUserAddressModel(null, user, address);

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult AddressEdit(UserAddressModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            //try to get a user with the specified id
            var user = _userService.GetUserById(model.UserId);
            if (user == null)
                return RedirectToAction("List");

            //try to get an address with the specified id
            var address = _addressService.GetAddressById(model.Address.Id);
            if (address == null)
                return RedirectToAction("Edit", new { id = user.Id });

            //custom address attributes
            var customAttributes = _addressAttributeParser.ParseCustomAddressAttributes(model.Form);
            var customAttributeWarnings = _addressAttributeParser.GetAttributeWarnings(customAttributes);
            foreach (var error in customAttributeWarnings)
            {
                ModelState.AddModelError(string.Empty, error);
            }

            if (ModelState.IsValid)
            {
                address = model.Address.ToEntity(address);
                address.CustomAttributes = customAttributes;
                _addressService.UpdateAddress(address);

                SuccessNotification(_localizationService.GetResource("Admin.Users.Users.Addresses.Updated"));

                return RedirectToAction("AddressEdit", new { addressId = model.Address.Id, userId = model.UserId });
            }

            //prepare model
            model = _userModelFactory.PrepareUserAddressModel(model, user, address, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        #endregion

        #region Orders

        [HttpPost]
        public virtual IActionResult OrderList(UserOrderSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedKendoGridJson();

            //try to get a user with the specified id
            var user = _userService.GetUserById(searchModel.UserId)
                ?? throw new ArgumentException("No user found with the specified id");

            //prepare model
            var model = _userModelFactory.PrepareUserOrderListModel(searchModel, user);

            return Json(model);
        }

        #endregion

        #region User

        public virtual IActionResult LoadUserStatistics(string period)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return Content(string.Empty);

            var result = new List<object>();

            var nowDt = _dateTimeHelper.ConvertToUserTime(DateTime.Now);
            var timeZone = _dateTimeHelper.CurrentTimeZone;
            var searchUserRoleIds = new[] { _userService.GetUserRoleBySystemName(AppUserDefaults.RegisteredRoleName).Id };

            var culture = new CultureInfo(_workContext.WorkingLanguage.LanguageCulture);

            switch (period)
            {
                case "year":
                    //year statistics
                    var yearAgoDt = nowDt.AddYears(-1).AddMonths(1);
                    var searchYearDateUser = new DateTime(yearAgoDt.Year, yearAgoDt.Month, 1);
                    for (var i = 0; i <= 12; i++)
                    {
                        result.Add(new
                        {
                            date = searchYearDateUser.Date.ToString("Y", culture),
                            value = _userService.GetAllUsers(
                                createdFromUtc: _dateTimeHelper.ConvertToUtcTime(searchYearDateUser, timeZone),
                                createdToUtc: _dateTimeHelper.ConvertToUtcTime(searchYearDateUser.AddMonths(1), timeZone),
                                userRoleIds: searchUserRoleIds,
                                pageIndex: 0,
                                pageSize: 1, getOnlyTotalCount: true).TotalCount.ToString()
                        });

                        searchYearDateUser = searchYearDateUser.AddMonths(1);
                    }

                    break;
                case "month":
                    //month statistics
                    var monthAgoDt = nowDt.AddDays(-30);
                    var searchMonthDateUser = new DateTime(monthAgoDt.Year, monthAgoDt.Month, monthAgoDt.Day);
                    for (var i = 0; i <= 30; i++)
                    {
                        result.Add(new
                        {
                            date = searchMonthDateUser.Date.ToString("M", culture),
                            value = _userService.GetAllUsers(
                                createdFromUtc: _dateTimeHelper.ConvertToUtcTime(searchMonthDateUser, timeZone),
                                createdToUtc: _dateTimeHelper.ConvertToUtcTime(searchMonthDateUser.AddDays(1), timeZone),
                                userRoleIds: searchUserRoleIds,
                                pageIndex: 0,
                                pageSize: 1, getOnlyTotalCount: true).TotalCount.ToString()
                        });

                        searchMonthDateUser = searchMonthDateUser.AddDays(1);
                    }

                    break;
                case "week":
                default:
                    //week statistics
                    var weekAgoDt = nowDt.AddDays(-7);
                    var searchWeekDateUser = new DateTime(weekAgoDt.Year, weekAgoDt.Month, weekAgoDt.Day);
                    for (var i = 0; i <= 7; i++)
                    {
                        result.Add(new
                        {
                            date = searchWeekDateUser.Date.ToString("d dddd", culture),
                            value = _userService.GetAllUsers(
                                createdFromUtc: _dateTimeHelper.ConvertToUtcTime(searchWeekDateUser, timeZone),
                                createdToUtc: _dateTimeHelper.ConvertToUtcTime(searchWeekDateUser.AddDays(1), timeZone),
                                userRoleIds: searchUserRoleIds,
                                pageIndex: 0,
                                pageSize: 1, getOnlyTotalCount: true).TotalCount.ToString()
                        });

                        searchWeekDateUser = searchWeekDateUser.AddDays(1);
                    }

                    break;
            }

            return Json(result);
        }

        #endregion

        #region Current shopping cart/ wishlist

        [HttpPost]
        public virtual IActionResult GetCartList(UserShoppingCartSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedKendoGridJson();

            //try to get a user with the specified id
            var user = _userService.GetUserById(searchModel.UserId)
                ?? throw new ArgumentException("No user found with the specified id");

            //prepare model
            var model = _userModelFactory.PrepareUserShoppingCartListModel(searchModel, user);

            return Json(model);
        }

        #endregion

        #region Activity log

        [HttpPost]
        public virtual IActionResult ListActivityLog(UserActivityLogSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedKendoGridJson();

            //try to get a user with the specified id
            var user = _userService.GetUserById(searchModel.UserId)
                ?? throw new ArgumentException("No user found with the specified id");

            //prepare model
            var model = _userModelFactory.PrepareUserActivityLogListModel(searchModel, user);

            return Json(model);
        }

        #endregion

        #region Back in stock subscriptions

        [HttpPost]
        public virtual IActionResult BackInStockSubscriptionList(UserBackInStockSubscriptionSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedKendoGridJson();

            //try to get a user with the specified id
            var user = _userService.GetUserById(searchModel.UserId)
                ?? throw new ArgumentException("No user found with the specified id");

            //prepare model
            var model = _userModelFactory.PrepareUserBackInStockSubscriptionListModel(searchModel, user);

            return Json(model);
        }

        #endregion

        #region GDPR

        public virtual IActionResult GdprLog()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            //prepare model
            var model = _userModelFactory.PrepareGdprLogSearchModel(new GdprLogSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult GdprLogList(GdprLogSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedKendoGridJson();

            //prepare model
            var model = _userModelFactory.PrepareGdprLogListModel(searchModel);

            return Json(model);
        }

        [HttpPost]
        public virtual IActionResult GdprDelete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            //try to get a user with the specified id
            var user = _userService.GetUserById(id);
            if (user == null)
                return RedirectToAction("List");

            if (!_gdprSettings.GdprEnabled)
                return RedirectToAction("List");

            try
            {
                //prevent attempts to delete the user, if it is the last active administrator
                if (user.IsAdmin() && !SecondAdminAccountExists(user))
                {
                    ErrorNotification(_localizationService.GetResource("Admin.Users.Users.AdminAccountShouldExists.DeleteAdministrator"));
                    return RedirectToAction("Edit", new { id = user.Id });
                }

                //ensure that the current user cannot delete "Administrators" if he's not an admin himself
                if (user.IsAdmin() && !_workContext.CurrentUser.IsAdmin())
                {
                    ErrorNotification(_localizationService.GetResource("Admin.Users.Users.OnlyAdminCanDeleteAdmin"));
                    return RedirectToAction("Edit", new { id = user.Id });
                }

                //delete
                _gdprService.PermanentDeleteUser(user);

                //activity log
                _userActivityService.InsertActivity("DeleteUser",
                    string.Format(_localizationService.GetResource("ActivityLog.DeleteUser"), user.Id), user);

                SuccessNotification(_localizationService.GetResource("Admin.Users.Users.Deleted"));

                return RedirectToAction("List");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc.Message);
                return RedirectToAction("Edit", new { id = user.Id });
            }
        }

        public virtual IActionResult GdprExport(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            //try to get a user with the specified id
            var user = _userService.GetUserById(id);
            if (user == null)
                return RedirectToAction("List");

            try
            {
                //log
                //_gdprService.InsertLog(user, 0, GdprRequestType.ExportData, _localizationService.GetResource("Gdpr.Exported"));
                //export
                //export
                var bytes = _exportManager.ExportUserGdprInfoToXlsx(_workContext.CurrentUser, _storeContext.CurrentStore.Id);

                return File(bytes, MimeTypes.TextXlsx, $"userdata-{user.Id}.xlsx");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("Edit", new { id = user.Id });
            }
        }
        #endregion

        #region Export / Import

        [HttpPost, ActionName("List")]
        [FormValueRequired("exportexcel-all")]
        public virtual IActionResult ExportExcelAll(UserSearchModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            var users = _userService.GetAllUsers(loadOnlyWithShoppingCart: false,
                userRoleIds: model.SelectedUserRoleIds.ToArray(),
                email: model.SearchEmail,
                username: model.SearchUsername,
                firstName: model.SearchFirstName,
                lastName: model.SearchLastName,
                dayOfBirth: int.TryParse(model.SearchDayOfBirth, out var dayOfBirth) ? dayOfBirth : 0,
                monthOfBirth: int.TryParse(model.SearchMonthOfBirth, out var monthOfBirth) ? monthOfBirth : 0,
                company: model.SearchCompany,
                phone: model.SearchPhone,
                zipPostalCode: model.SearchZipPostalCode);

            try
            {
                var bytes = _exportManager.ExportUsersToXlsx(users);
                return File(bytes, MimeTypes.TextXlsx, "users.xlsx");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("List");
            }
        }

        [HttpPost]
        public virtual IActionResult ExportExcelSelected(string selectedIds)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            var users = new List<User>();
            if (selectedIds != null)
            {
                var ids = selectedIds
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => Convert.ToInt32(x))
                    .ToArray();
                users.AddRange(_userService.GetUsersByIds(ids));
            }

            try
            {
                var bytes = _exportManager.ExportUsersToXlsx(users);
                return File(bytes, MimeTypes.TextXlsx, "users.xlsx");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("List");
            }
        }

        [HttpPost, ActionName("List")]
        [FormValueRequired("exportxml-all")]
        public virtual IActionResult ExportXmlAll(UserSearchModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            var users = _userService.GetAllUsers(loadOnlyWithShoppingCart: false,
                userRoleIds: model.SelectedUserRoleIds.ToArray(),
                email: model.SearchEmail,
                username: model.SearchUsername,
                firstName: model.SearchFirstName,
                lastName: model.SearchLastName,
                dayOfBirth: int.TryParse(model.SearchDayOfBirth, out var dayOfBirth) ? dayOfBirth : 0,
                monthOfBirth: int.TryParse(model.SearchMonthOfBirth, out var monthOfBirth) ? monthOfBirth : 0,
                company: model.SearchCompany,
                phone: model.SearchPhone,
                zipPostalCode: model.SearchZipPostalCode);

            try
            {
                var xml = _exportManager.ExportUsersToXml(users);
                return File(Encoding.UTF8.GetBytes(xml), "application/xml", "users.xml");
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
                return RedirectToAction("List");
            }
        }

        [HttpPost]
        public virtual IActionResult ExportXmlSelected(string selectedIds)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers))
                return AccessDeniedView();

            var users = new List<User>();
            if (selectedIds != null)
            {
                var ids = selectedIds
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => Convert.ToInt32(x))
                    .ToArray();
                users.AddRange(_userService.GetUsersByIds(ids));
            }

            var xml = _exportManager.ExportUsersToXml(users);
            return File(Encoding.UTF8.GetBytes(xml), "application/xml", "users.xml");
        }

        #endregion
    }
}