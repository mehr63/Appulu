using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Appulu.Core;
using Appulu.Core.Domain;
using Appulu.Core.Domain.Catalog;
using Appulu.Core.Domain.Common;
using Appulu.Core.Domain.Users;
using Appulu.Core.Domain.Forums;
using Appulu.Core.Domain.Gdpr;
using Appulu.Core.Domain.Localization;
using Appulu.Core.Domain.Media;
using Appulu.Core.Domain.Messages;
using Appulu.Core.Domain.Security;
using Appulu.Core.Domain.Tax;
using Appulu.Services.Authentication;
using Appulu.Services.Authentication.External;
using Appulu.Services.Catalog;
using Appulu.Services.Common;
using Appulu.Services.Users;
using Appulu.Services.Directory;
using Appulu.Services.Events;
using Appulu.Services.ExportImport;
using Appulu.Services.Gdpr;
using Appulu.Services.Helpers;
using Appulu.Services.Localization;
using Appulu.Services.Logging;
using Appulu.Services.Media;
using Appulu.Services.Messages;
using Appulu.Services.Orders;
using Appulu.Services.Tax;
using Appulu.Web.Extensions;
using Appulu.Web.Factories;
using Appulu.Web.Framework;
using Appulu.Web.Framework.Controllers;
using Appulu.Web.Framework.Mvc.Filters;
using Appulu.Web.Framework.Security;
using Appulu.Web.Framework.Security.Captcha;
using Appulu.Web.Framework.Validators;
using Appulu.Web.Models.User;

namespace Appulu.Web.Controllers
{
    public partial class UserController : BasePublicController
    {
        #region Fields

        private readonly AddressSettings _addressSettings;
        private readonly CaptchaSettings _captchaSettings;
        private readonly UserSettings _userSettings;
        private readonly DateTimeSettings _dateTimeSettings;
        private readonly IDownloadService _downloadService;
        private readonly ForumSettings _forumSettings;
        private readonly GdprSettings _gdprSettings;
        private readonly IAddressAttributeParser _addressAttributeParser;
        private readonly IAddressModelFactory _addressModelFactory;
        private readonly IAddressService _addressService;
        private readonly IAuthenticationService _authenticationService;
        private readonly ICountryService _countryService;
        private readonly ICurrencyService _currencyService;
        private readonly IUserActivityService _userActivityService;
        private readonly IUserAttributeParser _userAttributeParser;
        private readonly IUserAttributeService _userAttributeService;
        private readonly IUserModelFactory _userModelFactory;
        private readonly IUserRegistrationService _userRegistrationService;
        private readonly IUserService _userService;
        private readonly IEventPublisher _eventPublisher;
        private readonly IExportManager _exportManager;
        private readonly IExternalAuthenticationService _externalAuthenticationService;
        private readonly IGdprService _gdprService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IGiftCardService _giftCardService;
        private readonly ILocalizationService _localizationService;
        private readonly INewsLetterSubscriptionService _newsLetterSubscriptionService;
        private readonly IOrderService _orderService;
        private readonly IPictureService _pictureService;
        private readonly IPriceFormatter _priceFormatter;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IStoreContext _storeContext;
        private readonly ITaxService _taxService;
        private readonly IWebHelper _webHelper;
        private readonly IWorkContext _workContext;
        private readonly IWorkflowMessageService _workflowMessageService;
        private readonly LocalizationSettings _localizationSettings;
        private readonly MediaSettings _mediaSettings;
        private readonly StoreInformationSettings _storeInformationSettings;
        private readonly TaxSettings _taxSettings;

        #endregion

        #region Ctor

        public UserController(AddressSettings addressSettings,
            CaptchaSettings captchaSettings,
            UserSettings userSettings,
            DateTimeSettings dateTimeSettings,
            IDownloadService downloadService,
            ForumSettings forumSettings,
            GdprSettings gdprSettings,
            IAddressAttributeParser addressAttributeParser,
            IAddressModelFactory addressModelFactory,
            IAddressService addressService,
            IAuthenticationService authenticationService,
            ICountryService countryService,
            ICurrencyService currencyService,
            IUserActivityService userActivityService,
            IUserAttributeParser userAttributeParser,
            IUserAttributeService userAttributeService,
            IUserModelFactory userModelFactory,
            IUserRegistrationService userRegistrationService,
            IUserService userService,
            IEventPublisher eventPublisher,
            IExportManager exportManager,
            IExternalAuthenticationService externalAuthenticationService,
            IGdprService gdprService,
            IGenericAttributeService genericAttributeService,
            IGiftCardService giftCardService,
            ILocalizationService localizationService,
            INewsLetterSubscriptionService newsLetterSubscriptionService,
            IOrderService orderService,
            IPictureService pictureService,
            IPriceFormatter priceFormatter,
            IShoppingCartService shoppingCartService,
            IStoreContext storeContext,
            ITaxService taxService,
            IWebHelper webHelper,
            IWorkContext workContext,
            IWorkflowMessageService workflowMessageService,
            LocalizationSettings localizationSettings,
            MediaSettings mediaSettings,
            StoreInformationSettings storeInformationSettings,
            TaxSettings taxSettings)
        {
            this._addressSettings = addressSettings;
            this._captchaSettings = captchaSettings;
            this._userSettings = userSettings;
            this._dateTimeSettings = dateTimeSettings;
            this._downloadService = downloadService;
            this._forumSettings = forumSettings;
            this._gdprSettings = gdprSettings;
            this._addressAttributeParser = addressAttributeParser;
            this._addressModelFactory = addressModelFactory;
            this._addressService = addressService;
            this._authenticationService = authenticationService;
            this._countryService = countryService;
            this._currencyService = currencyService;
            this._userActivityService = userActivityService;
            this._userAttributeParser = userAttributeParser;
            this._userAttributeService = userAttributeService;
            this._userModelFactory = userModelFactory;
            this._userRegistrationService = userRegistrationService;
            this._userService = userService;
            this._eventPublisher = eventPublisher;
            this._exportManager = exportManager;
            this._externalAuthenticationService = externalAuthenticationService;
            this._gdprService = gdprService;
            this._genericAttributeService = genericAttributeService;
            this._giftCardService = giftCardService;
            this._localizationService = localizationService;
            this._newsLetterSubscriptionService = newsLetterSubscriptionService;
            this._orderService = orderService;
            this._pictureService = pictureService;
            this._priceFormatter = priceFormatter;
            this._shoppingCartService = shoppingCartService;
            this._storeContext = storeContext;
            this._taxService = taxService;
            this._webHelper = webHelper;
            this._workContext = workContext;
            this._workflowMessageService = workflowMessageService;
            this._localizationSettings = localizationSettings;
            this._mediaSettings = mediaSettings;
            this._storeInformationSettings = storeInformationSettings;
            this._taxSettings = taxSettings;
        }

        #endregion

        #region Utilities

        protected virtual string ParseCustomUserAttributes(IFormCollection form)
        {
            if (form == null)
                throw new ArgumentNullException(nameof(form));

            var attributesXml = "";
            var attributes = _userAttributeService.GetAllUserAttributes();
            foreach (var attribute in attributes)
            {
                var controlId = $"user_attribute_{attribute.Id}";
                switch (attribute.AttributeControlType)
                {
                    case AttributeControlType.DropdownList:
                    case AttributeControlType.RadioList:
                        {
                            var ctrlAttributes = form[controlId];
                            if (!StringValues.IsNullOrEmpty(ctrlAttributes))
                            {
                                var selectedAttributeId = int.Parse(ctrlAttributes);
                                if (selectedAttributeId > 0)
                                    attributesXml = _userAttributeParser.AddUserAttribute(attributesXml,
                                        attribute, selectedAttributeId.ToString());
                            }
                        }
                        break;
                    case AttributeControlType.Checkboxes:
                        {
                            var cblAttributes = form[controlId];
                            if (!StringValues.IsNullOrEmpty(cblAttributes))
                            {
                                foreach (var item in cblAttributes.ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                                {
                                    var selectedAttributeId = int.Parse(item);
                                    if (selectedAttributeId > 0)
                                        attributesXml = _userAttributeParser.AddUserAttribute(attributesXml,
                                            attribute, selectedAttributeId.ToString());
                                }
                            }
                        }
                        break;
                    case AttributeControlType.ReadonlyCheckboxes:
                        {
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
                        }
                        break;
                    case AttributeControlType.TextBox:
                    case AttributeControlType.MultilineTextbox:
                        {
                            var ctrlAttributes = form[controlId];
                            if (!StringValues.IsNullOrEmpty(ctrlAttributes))
                            {
                                var enteredText = ctrlAttributes.ToString().Trim();
                                attributesXml = _userAttributeParser.AddUserAttribute(attributesXml,
                                    attribute, enteredText);
                            }
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

        #endregion

        #region Methods

        #region Login / logout

        [HttpsRequirement(SslRequirement.Yes)]
        //available even when a store is closed
        [CheckAccessClosedStore(true)]
        //available even when navigation is not allowed
        [CheckAccessPublicStore(true)]
        public virtual IActionResult Login(bool? checkoutAsGuest)
        {
            var model = _userModelFactory.PrepareLoginModel(checkoutAsGuest);
            return View(model);
        }

        [HttpPost]
        [ValidateCaptcha]
        //available even when a store is closed
        [CheckAccessClosedStore(true)]
        //available even when navigation is not allowed
        [CheckAccessPublicStore(true)]
        [PublicAntiForgery]
        public virtual IActionResult Login(LoginModel model, string returnUrl, bool captchaValid)
        {
            //validate CAPTCHA
            if (_captchaSettings.Enabled && _captchaSettings.ShowOnLoginPage && !captchaValid)
            {
                ModelState.AddModelError("", _captchaSettings.GetWrongCaptchaMessage(_localizationService));
            }

            if (ModelState.IsValid)
            {
                if (_userSettings.UsernamesEnabled && model.Username != null)
                {
                    model.Username = model.Username.Trim();
                }
                var loginResult = _userRegistrationService.ValidateUser(_userSettings.UsernamesEnabled ? model.Username : model.Email, model.Password);
                switch (loginResult)
                {
                    case UserLoginResults.Successful:
                        {
                            var user = _userSettings.UsernamesEnabled
                                ? _userService.GetUserByUsername(model.Username)
                                : _userService.GetUserByEmail(model.Email);

                            //migrate shopping cart
                            _shoppingCartService.MigrateShoppingCart(_workContext.CurrentUser, user, true);

                            //sign in new user
                            _authenticationService.SignIn(user, model.RememberMe);

                            //raise event       
                            _eventPublisher.Publish(new UserLoggedinEvent(user));

                            //activity log
                            _userActivityService.InsertActivity(user, "PublicStore.Login",
                                _localizationService.GetResource("ActivityLog.PublicStore.Login"), user);

                            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
                                return RedirectToRoute("HomePage");

                            return Redirect(returnUrl);
                        }
                    case UserLoginResults.UserNotExist:
                        ModelState.AddModelError("", _localizationService.GetResource("Account.Login.WrongCredentials.UserNotExist"));
                        break;
                    case UserLoginResults.Deleted:
                        ModelState.AddModelError("", _localizationService.GetResource("Account.Login.WrongCredentials.Deleted"));
                        break;
                    case UserLoginResults.NotActive:
                        ModelState.AddModelError("", _localizationService.GetResource("Account.Login.WrongCredentials.NotActive"));
                        break;
                    case UserLoginResults.NotRegistered:
                        ModelState.AddModelError("", _localizationService.GetResource("Account.Login.WrongCredentials.NotRegistered"));
                        break;
                    case UserLoginResults.LockedOut:
                        ModelState.AddModelError("", _localizationService.GetResource("Account.Login.WrongCredentials.LockedOut"));
                        break;
                    case UserLoginResults.WrongPassword:
                    default:
                        ModelState.AddModelError("", _localizationService.GetResource("Account.Login.WrongCredentials"));
                        break;
                }
            }

            //If we got this far, something failed, redisplay form
            model = _userModelFactory.PrepareLoginModel(model.CheckoutAsGuest);
            return View(model);
        }

        //available even when a store is closed
        [CheckAccessClosedStore(true)]
        //available even when navigation is not allowed
        [CheckAccessPublicStore(true)]
        public virtual IActionResult Logout()
        {
            if (_workContext.OriginalUserIfImpersonated != null)
            {
                //activity log
                _userActivityService.InsertActivity(_workContext.OriginalUserIfImpersonated, "Impersonation.Finished",
                    string.Format(_localizationService.GetResource("ActivityLog.Impersonation.Finished.StoreOwner"),
                        _workContext.CurrentUser.Email, _workContext.CurrentUser.Id),
                    _workContext.CurrentUser);

                _userActivityService.InsertActivity("Impersonation.Finished",
                    string.Format(_localizationService.GetResource("ActivityLog.Impersonation.Finished.User"),
                        _workContext.OriginalUserIfImpersonated.Email, _workContext.OriginalUserIfImpersonated.Id),
                    _workContext.OriginalUserIfImpersonated);

                //logout impersonated user
                _genericAttributeService
                    .SaveAttribute<int?>(_workContext.OriginalUserIfImpersonated, AppUserDefaults.ImpersonatedUserIdAttribute, null);

                //redirect back to user details page (admin area)
                return this.RedirectToAction("Edit", "User", new { id = _workContext.CurrentUser.Id, area = AreaNames.Admin });
            }

            //activity log
            _userActivityService.InsertActivity(_workContext.CurrentUser, "PublicStore.Logout",
                _localizationService.GetResource("ActivityLog.PublicStore.Logout"), _workContext.CurrentUser);

            //standard logout 
            _authenticationService.SignOut();

            //raise logged out event       
            _eventPublisher.Publish(new UserLoggedOutEvent(_workContext.CurrentUser));

            //EU Cookie
            if (_storeInformationSettings.DisplayEuCookieLawWarning)
            {
                //the cookie law message should not pop up immediately after logout.
                //otherwise, the user will have to click it again...
                //and thus next visitor will not click it... so violation for that cookie law..
                //the only good solution in this case is to store a temporary variable
                //indicating that the EU cookie popup window should not be displayed on the next page open (after logout redirection to homepage)
                //but it'll be displayed for further page loads
                TempData["app.IgnoreEuCookieLawWarning"] = true;
            }

            return RedirectToRoute("HomePage");
        }

        #endregion

        #region Password recovery

        [HttpsRequirement(SslRequirement.Yes)]
        //available even when navigation is not allowed
        [CheckAccessPublicStore(true)]
        public virtual IActionResult PasswordRecovery()
        {
            var model = _userModelFactory.PreparePasswordRecoveryModel();
            return View(model);
        }

        [HttpPost, ActionName("PasswordRecovery")]
        [PublicAntiForgery]
        [FormValueRequired("send-email")]
        //available even when navigation is not allowed
        [CheckAccessPublicStore(true)]
        public virtual IActionResult PasswordRecoverySend(PasswordRecoveryModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _userService.GetUserByEmail(model.Email);
                if (user != null && user.Active && !user.Deleted)
                {
                    //save token and current date
                    var passwordRecoveryToken = Guid.NewGuid();
                    _genericAttributeService.SaveAttribute(user, AppUserDefaults.PasswordRecoveryTokenAttribute,
                        passwordRecoveryToken.ToString());
                    DateTime? generatedDateTime = DateTime.UtcNow;
                    _genericAttributeService.SaveAttribute(user,
                        AppUserDefaults.PasswordRecoveryTokenDateGeneratedAttribute, generatedDateTime);

                    //send email
                    _workflowMessageService.SendUserPasswordRecoveryMessage(user,
                        _workContext.WorkingLanguage.Id);

                    model.Result = _localizationService.GetResource("Account.PasswordRecovery.EmailHasBeenSent");
                }
                else
                {
                    model.Result = _localizationService.GetResource("Account.PasswordRecovery.EmailNotFound");
                }

                return View(model);
            }

            //If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpsRequirement(SslRequirement.Yes)]
        //available even when navigation is not allowed
        [CheckAccessPublicStore(true)]
        public virtual IActionResult PasswordRecoveryConfirm(string token, string email)
        {
            var user = _userService.GetUserByEmail(email);
            if (user == null)
                return RedirectToRoute("HomePage");

            if (string.IsNullOrEmpty(_genericAttributeService.GetAttribute<string>(user, AppUserDefaults.PasswordRecoveryTokenAttribute)))
            {
                return View(new PasswordRecoveryConfirmModel
                {
                    DisablePasswordChanging = true,
                    Result = _localizationService.GetResource("Account.PasswordRecovery.PasswordAlreadyHasBeenChanged")
                });
            }

            var model = _userModelFactory.PreparePasswordRecoveryConfirmModel();

            //validate token
            if (!_userService.IsPasswordRecoveryTokenValid(user, token))
            {
                model.DisablePasswordChanging = true;
                model.Result = _localizationService.GetResource("Account.PasswordRecovery.WrongToken");
            }

            //validate token expiration date
            if (_userService.IsPasswordRecoveryLinkExpired(user))
            {
                model.DisablePasswordChanging = true;
                model.Result = _localizationService.GetResource("Account.PasswordRecovery.LinkExpired");
            }

            return View(model);
        }

        [HttpPost, ActionName("PasswordRecoveryConfirm")]
        [PublicAntiForgery]
        [FormValueRequired("set-password")]
        //available even when navigation is not allowed
        [CheckAccessPublicStore(true)]
        public virtual IActionResult PasswordRecoveryConfirmPOST(string token, string email, PasswordRecoveryConfirmModel model)
        {
            var user = _userService.GetUserByEmail(email);
            if (user == null)
                return RedirectToRoute("HomePage");

            //validate token
            if (!_userService.IsPasswordRecoveryTokenValid(user, token))
            {
                model.DisablePasswordChanging = true;
                model.Result = _localizationService.GetResource("Account.PasswordRecovery.WrongToken");
                return View(model);
            }

            //validate token expiration date
            if (_userService.IsPasswordRecoveryLinkExpired(user))
            {
                model.DisablePasswordChanging = true;
                model.Result = _localizationService.GetResource("Account.PasswordRecovery.LinkExpired");
                return View(model);
            }

            if (ModelState.IsValid)
            {
                var response = _userRegistrationService.ChangePassword(new ChangePasswordRequest(email,
                    false, _userSettings.DefaultPasswordFormat, model.NewPassword));
                if (response.Success)
                {
                    _genericAttributeService.SaveAttribute(user, AppUserDefaults.PasswordRecoveryTokenAttribute, "");

                    model.DisablePasswordChanging = true;
                    model.Result = _localizationService.GetResource("Account.PasswordRecovery.PasswordHasBeenChanged");
                }
                else
                {
                    model.Result = response.Errors.FirstOrDefault();
                }

                return View(model);
            }

            //If we got this far, something failed, redisplay form
            return View(model);
        }

        #endregion     

        #region Register

        [HttpsRequirement(SslRequirement.Yes)]
        //available even when navigation is not allowed
        [CheckAccessPublicStore(true)]
        public virtual IActionResult Register()
        {
            //check whether registration is allowed
            if (_userSettings.UserRegistrationType == UserRegistrationType.Disabled)
                return RedirectToRoute("RegisterResult", new { resultId = (int)UserRegistrationType.Disabled });

            var model = new RegisterModel();
            model = _userModelFactory.PrepareRegisterModel(model, false, setDefaultValues: true);

            return View(model);
        }

        [HttpPost]
        [ValidateCaptcha]
        [ValidateHoneypot]
        [PublicAntiForgery]
        //available even when navigation is not allowed
        [CheckAccessPublicStore(true)]
        public virtual IActionResult Register(RegisterModel model, string returnUrl, bool captchaValid)
        {
            //check whether registration is allowed
            if (_userSettings.UserRegistrationType == UserRegistrationType.Disabled)
                return RedirectToRoute("RegisterResult", new { resultId = (int)UserRegistrationType.Disabled });

            if (_workContext.CurrentUser.IsRegistered())
            {
                //Already registered user. 
                _authenticationService.SignOut();

                //raise logged out event       
                _eventPublisher.Publish(new UserLoggedOutEvent(_workContext.CurrentUser));

                //Save a new record
                _workContext.CurrentUser = _userService.InsertGuestUser();
            }
            var user = _workContext.CurrentUser;
            user.RegisteredInStoreId = _storeContext.CurrentStore.Id;

            //custom user attributes
            var userAttributesXml = ParseCustomUserAttributes(model.Form);
            var userAttributeWarnings = _userAttributeParser.GetAttributeWarnings(userAttributesXml);
            foreach (var error in userAttributeWarnings)
            {
                ModelState.AddModelError("", error);
            }

            //validate CAPTCHA
            if (_captchaSettings.Enabled && _captchaSettings.ShowOnRegistrationPage && !captchaValid)
            {
                ModelState.AddModelError("", _captchaSettings.GetWrongCaptchaMessage(_localizationService));
            }

            if (ModelState.IsValid)
            {
                if (_userSettings.UsernamesEnabled && model.Username != null)
                {
                    model.Username = model.Username.Trim();
                }

                var isApproved = _userSettings.UserRegistrationType == UserRegistrationType.Standard;
                var registrationRequest = new UserRegistrationRequest(user,
                    model.Email,
                    _userSettings.UsernamesEnabled ? model.Username : model.Email,
                    model.Password,
                    _userSettings.DefaultPasswordFormat,
                    _storeContext.CurrentStore.Id,
                    isApproved);
                var registrationResult = _userRegistrationService.RegisterUser(registrationRequest);
                if (registrationResult.Success)
                {
                    //properties
                    if (_dateTimeSettings.AllowUsersToSetTimeZone)
                    {
                        _genericAttributeService.SaveAttribute(user, AppUserDefaults.TimeZoneIdAttribute, model.TimeZoneId);
                    }
                    //VAT number
                    if (_taxSettings.EuVatEnabled)
                    {
                        _genericAttributeService.SaveAttribute(user, AppUserDefaults.VatNumberAttribute, model.VatNumber);

                        var vatNumberStatus = _taxService.GetVatNumberStatus(model.VatNumber, out string _, out string vatAddress);
                        _genericAttributeService.SaveAttribute(user, AppUserDefaults.VatNumberStatusIdAttribute, (int)vatNumberStatus);
                        //send VAT number admin notification
                        if (!string.IsNullOrEmpty(model.VatNumber) && _taxSettings.EuVatEmailAdminWhenNewVatSubmitted)
                            _workflowMessageService.SendNewVatSubmittedStoreOwnerNotification(user, model.VatNumber, vatAddress, _localizationSettings.DefaultAdminLanguageId);
                    }

                    //form fields
                    if (_userSettings.GenderEnabled)
                        _genericAttributeService.SaveAttribute(user, AppUserDefaults.GenderAttribute, model.Gender);
                    _genericAttributeService.SaveAttribute(user, AppUserDefaults.FirstNameAttribute, model.FirstName);
                    _genericAttributeService.SaveAttribute(user, AppUserDefaults.LastNameAttribute, model.LastName);
                    if (_userSettings.DateOfBirthEnabled)
                    {
                        var dateOfBirth = model.ParseDateOfBirth();
                        _genericAttributeService.SaveAttribute(user, AppUserDefaults.DateOfBirthAttribute, dateOfBirth);
                    }
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
                        _genericAttributeService.SaveAttribute(user, AppUserDefaults.StateProvinceIdAttribute,
                            model.StateProvinceId);
                    if (_userSettings.PhoneEnabled)
                        _genericAttributeService.SaveAttribute(user, AppUserDefaults.PhoneAttribute, model.Phone);
                    if (_userSettings.FaxEnabled)
                        _genericAttributeService.SaveAttribute(user, AppUserDefaults.FaxAttribute, model.Fax);

                    //newsletter
                    if (_userSettings.NewsletterEnabled)
                    {
                        //save newsletter value
                        var newsletter = _newsLetterSubscriptionService.GetNewsLetterSubscriptionByEmailAndStoreId(model.Email, _storeContext.CurrentStore.Id);
                        if (newsletter != null)
                        {
                            if (model.Newsletter)
                            {
                                newsletter.Active = true;
                                _newsLetterSubscriptionService.UpdateNewsLetterSubscription(newsletter);

                                //GDPR
                                if (_gdprSettings.GdprEnabled && _gdprSettings.LogNewsletterConsent)
                                {
                                    _gdprService.InsertLog(user, 0, GdprRequestType.ConsentAgree, _localizationService.GetResource("Gdpr.Consent.Newsletter"));
                                }
                            }
                            //else
                            //{
                            //When registering, not checking the newsletter check box should not take an existing email address off of the subscription list.
                            //_newsLetterSubscriptionService.DeleteNewsLetterSubscription(newsletter);
                            //}
                        }
                        else
                        {
                            if (model.Newsletter)
                            {
                                _newsLetterSubscriptionService.InsertNewsLetterSubscription(new NewsLetterSubscription
                                {
                                    NewsLetterSubscriptionGuid = Guid.NewGuid(),
                                    Email = model.Email,
                                    Active = true,
                                    StoreId = _storeContext.CurrentStore.Id,
                                    CreatedOnUtc = DateTime.UtcNow
                                });

                                //GDPR
                                if (_gdprSettings.GdprEnabled && _gdprSettings.LogNewsletterConsent)
                                {
                                    _gdprService.InsertLog(user, 0, GdprRequestType.ConsentAgree, _localizationService.GetResource("Gdpr.Consent.Newsletter"));
                                }
                            }
                        }
                    }

                    if (_userSettings.AcceptPrivacyPolicyEnabled)
                    {
                        //privacy policy is required
                        //GDPR
                        if (_gdprSettings.GdprEnabled && _gdprSettings.LogPrivacyPolicyConsent)
                        {
                            _gdprService.InsertLog(user, 0, GdprRequestType.ConsentAgree, _localizationService.GetResource("Gdpr.Consent.PrivacyPolicy"));
                        }
                    }

                    //GDPR
                    if (_gdprSettings.GdprEnabled)
                    {
                        var consents = _gdprService.GetAllConsents().Where(consent => consent.DisplayDuringRegistration).ToList();
                        foreach (var consent in consents)
                        {
                            var controlId = $"consent{consent.Id}";
                            var cbConsent = model.Form[controlId];
                            if (!StringValues.IsNullOrEmpty(cbConsent) && cbConsent.ToString().Equals("on"))
                            {
                                //agree
                                _gdprService.InsertLog(user, consent.Id, GdprRequestType.ConsentAgree, consent.Message);
                            }
                            else
                            {
                                //disagree
                                _gdprService.InsertLog(user, consent.Id, GdprRequestType.ConsentDisagree, consent.Message);
                            }
                        }
                    }

                    //save user attributes
                    _genericAttributeService.SaveAttribute(user, AppUserDefaults.CustomUserAttributes, userAttributesXml);

                    //login user now
                    if (isApproved)
                        _authenticationService.SignIn(user, true);

                    //insert default address (if possible)
                    var defaultAddress = new Address
                    {
                        FirstName = _genericAttributeService.GetAttribute<string>(user, AppUserDefaults.FirstNameAttribute),
                        LastName = _genericAttributeService.GetAttribute<string>(user, AppUserDefaults.LastNameAttribute),
                        Email = user.Email,
                        Company = _genericAttributeService.GetAttribute<string>(user, AppUserDefaults.CompanyAttribute),
                        CountryId = _genericAttributeService.GetAttribute<int>(user, AppUserDefaults.CountryIdAttribute) > 0
                            ? (int?)_genericAttributeService.GetAttribute<int>(user, AppUserDefaults.CountryIdAttribute)
                            : null,
                        StateProvinceId = _genericAttributeService.GetAttribute<int>(user, AppUserDefaults.StateProvinceIdAttribute) > 0
                            ? (int?)_genericAttributeService.GetAttribute<int>(user, AppUserDefaults.StateProvinceIdAttribute)
                            : null,
                        County = _genericAttributeService.GetAttribute<string>(user, AppUserDefaults.CountyAttribute),
                        City = _genericAttributeService.GetAttribute<string>(user, AppUserDefaults.CityAttribute),
                        Address1 = _genericAttributeService.GetAttribute<string>(user, AppUserDefaults.StreetAddressAttribute),
                        Address2 = _genericAttributeService.GetAttribute<string>(user, AppUserDefaults.StreetAddress2Attribute),
                        ZipPostalCode = _genericAttributeService.GetAttribute<string>(user, AppUserDefaults.ZipPostalCodeAttribute),
                        PhoneNumber = _genericAttributeService.GetAttribute<string>(user, AppUserDefaults.PhoneAttribute),
                        FaxNumber = _genericAttributeService.GetAttribute<string>(user, AppUserDefaults.FaxAttribute),
                        CreatedOnUtc = user.CreatedOnUtc
                    };
                    if (this._addressService.IsAddressValid(defaultAddress))
                    {
                        //some validation
                        if (defaultAddress.CountryId == 0)
                            defaultAddress.CountryId = null;
                        if (defaultAddress.StateProvinceId == 0)
                            defaultAddress.StateProvinceId = null;
                        //set default address
                        //user.Addresses.Add(defaultAddress);
                        user.UserAddressMappings.Add(new UserAddressMapping { Address = defaultAddress });
                        user.BillingAddress = defaultAddress;
                        user.ShippingAddress = defaultAddress;
                        _userService.UpdateUser(user);
                    }

                    //notifications
                    if (_userSettings.NotifyNewUserRegistration)
                        _workflowMessageService.SendUserRegisteredNotificationMessage(user,
                            _localizationSettings.DefaultAdminLanguageId);

                    //raise event       
                    _eventPublisher.Publish(new UserRegisteredEvent(user));

                    switch (_userSettings.UserRegistrationType)
                    {
                        case UserRegistrationType.EmailValidation:
                            {
                                //email validation message
                                _genericAttributeService.SaveAttribute(user, AppUserDefaults.AccountActivationTokenAttribute, Guid.NewGuid().ToString());
                                _workflowMessageService.SendUserEmailValidationMessage(user, _workContext.WorkingLanguage.Id);

                                //result
                                return RedirectToRoute("RegisterResult",
                                    new { resultId = (int)UserRegistrationType.EmailValidation });
                            }
                        case UserRegistrationType.AdminApproval:
                            {
                                return RedirectToRoute("RegisterResult",
                                    new { resultId = (int)UserRegistrationType.AdminApproval });
                            }
                        case UserRegistrationType.Standard:
                            {
                                //send user welcome message
                                _workflowMessageService.SendUserWelcomeMessage(user, _workContext.WorkingLanguage.Id);

                                var redirectUrl = Url.RouteUrl("RegisterResult", new { resultId = (int)UserRegistrationType.Standard }, _webHelper.CurrentRequestProtocol);
                                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                                    redirectUrl = _webHelper.ModifyQueryString(redirectUrl, "returnurl", returnUrl);
                                return Redirect(redirectUrl);
                            }
                        default:
                            {
                                return RedirectToRoute("HomePage");
                            }
                    }
                }

                //errors
                foreach (var error in registrationResult.Errors)
                    ModelState.AddModelError("", error);
            }

            //If we got this far, something failed, redisplay form
            model = _userModelFactory.PrepareRegisterModel(model, true, userAttributesXml);
            return View(model);
        }

        //available even when navigation is not allowed
        [CheckAccessPublicStore(true)]
        public virtual IActionResult RegisterResult(int resultId)
        {
            var model = _userModelFactory.PrepareRegisterResultModel(resultId);
            return View(model);
        }

        //available even when navigation is not allowed
        [CheckAccessPublicStore(true)]
        [HttpPost]
        public virtual IActionResult RegisterResult(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
                return RedirectToRoute("HomePage");

            return Redirect(returnUrl);
        }

        [HttpPost]
        [PublicAntiForgery]
        //available even when navigation is not allowed
        [CheckAccessPublicStore(true)]
        public virtual IActionResult CheckUsernameAvailability(string username)
        {
            var usernameAvailable = false;
            var statusText = _localizationService.GetResource("Account.CheckUsernameAvailability.NotAvailable");

            if (!UsernamePropertyValidator.IsValid(username, _userSettings))
            {
                statusText = _localizationService.GetResource("Account.Fields.Username.NotValid");
            }
            else if (_userSettings.UsernamesEnabled && !string.IsNullOrWhiteSpace(username))
            {
                if (_workContext.CurrentUser != null &&
                    _workContext.CurrentUser.Username != null &&
                    _workContext.CurrentUser.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase))
                {
                    statusText = _localizationService.GetResource("Account.CheckUsernameAvailability.CurrentUsername");
                }
                else
                {
                    var user = _userService.GetUserByUsername(username);
                    if (user == null)
                    {
                        statusText = _localizationService.GetResource("Account.CheckUsernameAvailability.Available");
                        usernameAvailable = true;
                    }
                }
            }

            return Json(new { Available = usernameAvailable, Text = statusText });
        }

        [HttpsRequirement(SslRequirement.Yes)]
        //available even when navigation is not allowed
        [CheckAccessPublicStore(true)]
        public virtual IActionResult AccountActivation(string token, string email)
        {
            var user = _userService.GetUserByEmail(email);
            if (user == null)
                return RedirectToRoute("HomePage");

            var cToken = _genericAttributeService.GetAttribute<string>(user, AppUserDefaults.AccountActivationTokenAttribute);
            if (string.IsNullOrEmpty(cToken))
                return
                    View(new AccountActivationModel
                    {
                        Result = _localizationService.GetResource("Account.AccountActivation.AlreadyActivated")
                    });

            if (!cToken.Equals(token, StringComparison.InvariantCultureIgnoreCase))
                return RedirectToRoute("HomePage");

            //activate user account
            user.Active = true;
            _userService.UpdateUser(user);
            _genericAttributeService.SaveAttribute(user, AppUserDefaults.AccountActivationTokenAttribute, "");
            //send welcome message
            _workflowMessageService.SendUserWelcomeMessage(user, _workContext.WorkingLanguage.Id);

            var model = new AccountActivationModel
            {
                Result = _localizationService.GetResource("Account.AccountActivation.Activated")
            };
            return View(model);
        }

        #endregion

        #region My account / Info

        [HttpsRequirement(SslRequirement.Yes)]
        public virtual IActionResult Info()
        {
            if (!_workContext.CurrentUser.IsRegistered())
                return Challenge();

            var model = new UserInfoModel();
            model = _userModelFactory.PrepareUserInfoModel(model, _workContext.CurrentUser, false);

            return View(model);
        }

        [HttpPost]
        [PublicAntiForgery]
        public virtual IActionResult Info(UserInfoModel model)
        {
            if (!_workContext.CurrentUser.IsRegistered())
                return Challenge();

            var user = _workContext.CurrentUser;

            //custom user attributes
            var userAttributesXml = ParseCustomUserAttributes(model.Form);
            var userAttributeWarnings = _userAttributeParser.GetAttributeWarnings(userAttributesXml);
            foreach (var error in userAttributeWarnings)
            {
                ModelState.AddModelError("", error);
            }

            try
            {
                if (ModelState.IsValid)
                {
                    //username 
                    if (_userSettings.UsernamesEnabled && this._userSettings.AllowUsersToChangeUsernames)
                    {
                        if (
                            !user.Username.Equals(model.Username.Trim(), StringComparison.InvariantCultureIgnoreCase))
                        {
                            //change username
                            _userRegistrationService.SetUsername(user, model.Username.Trim());

                            //re-authenticate
                            //do not authenticate users in impersonation mode
                            if (_workContext.OriginalUserIfImpersonated == null)
                                _authenticationService.SignIn(user, true);
                        }
                    }
                    //email
                    if (!user.Email.Equals(model.Email.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        //change email
                        var requireValidation = _userSettings.UserRegistrationType == UserRegistrationType.EmailValidation;
                        _userRegistrationService.SetEmail(user, model.Email.Trim(), requireValidation);

                        //do not authenticate users in impersonation mode
                        if (_workContext.OriginalUserIfImpersonated == null)
                        {
                            //re-authenticate (if usernames are disabled)
                            if (!_userSettings.UsernamesEnabled && !requireValidation)
                                _authenticationService.SignIn(user, true);
                        }
                    }

                    //properties
                    if (_dateTimeSettings.AllowUsersToSetTimeZone)
                    {
                        _genericAttributeService.SaveAttribute(user, AppUserDefaults.TimeZoneIdAttribute,
                            model.TimeZoneId);
                    }
                    //VAT number
                    if (_taxSettings.EuVatEnabled)
                    {
                        var prevVatNumber = _genericAttributeService.GetAttribute<string>(user, AppUserDefaults.VatNumberAttribute);

                        _genericAttributeService.SaveAttribute(user, AppUserDefaults.VatNumberAttribute,
                            model.VatNumber);
                        if (prevVatNumber != model.VatNumber)
                        {
                            var vatNumberStatus = _taxService.GetVatNumberStatus(model.VatNumber, out string _, out string vatAddress);
                            _genericAttributeService.SaveAttribute(user, AppUserDefaults.VatNumberStatusIdAttribute, (int)vatNumberStatus);
                            //send VAT number admin notification
                            if (!string.IsNullOrEmpty(model.VatNumber) && _taxSettings.EuVatEmailAdminWhenNewVatSubmitted)
                                _workflowMessageService.SendNewVatSubmittedStoreOwnerNotification(user,
                                    model.VatNumber, vatAddress, _localizationSettings.DefaultAdminLanguageId);
                        }
                    }

                    //form fields
                    if (_userSettings.GenderEnabled)
                        _genericAttributeService.SaveAttribute(user, AppUserDefaults.GenderAttribute, model.Gender);
                    _genericAttributeService.SaveAttribute(user, AppUserDefaults.FirstNameAttribute, model.FirstName);
                    _genericAttributeService.SaveAttribute(user, AppUserDefaults.LastNameAttribute, model.LastName);
                    if (_userSettings.DateOfBirthEnabled)
                    {
                        var dateOfBirth = model.ParseDateOfBirth();
                        _genericAttributeService.SaveAttribute(user, AppUserDefaults.DateOfBirthAttribute, dateOfBirth);
                    }
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

                    //newsletter
                    if (_userSettings.NewsletterEnabled)
                    {
                        //save newsletter value
                        var newsletter = _newsLetterSubscriptionService.GetNewsLetterSubscriptionByEmailAndStoreId(user.Email, _storeContext.CurrentStore.Id);
                        if (newsletter != null)
                        {
                            if (model.Newsletter)
                            {
                                var wasActive = newsletter.Active;
                                newsletter.Active = true;
                                _newsLetterSubscriptionService.UpdateNewsLetterSubscription(newsletter);

                                //GDPR
                                if (!wasActive && _gdprSettings.GdprEnabled && _gdprSettings.LogNewsletterConsent)
                                {
                                    _gdprService.InsertLog(user, 0, GdprRequestType.ConsentAgree, _localizationService.GetResource("Gdpr.Consent.Newsletter"));
                                }
                            }
                            else
                            {
                                _newsLetterSubscriptionService.DeleteNewsLetterSubscription(newsletter);

                                //GDPR
                                if (_gdprSettings.GdprEnabled && _gdprSettings.LogNewsletterConsent)
                                {
                                    _gdprService.InsertLog(user, 0, GdprRequestType.ConsentDisagree, _localizationService.GetResource("Gdpr.Consent.Newsletter"));
                                }
                            }
                        }
                        else
                        {
                            if (model.Newsletter)
                            {
                                _newsLetterSubscriptionService.InsertNewsLetterSubscription(new NewsLetterSubscription
                                {
                                    NewsLetterSubscriptionGuid = Guid.NewGuid(),
                                    Email = user.Email,
                                    Active = true,
                                    StoreId = _storeContext.CurrentStore.Id,
                                    CreatedOnUtc = DateTime.UtcNow
                                });

                                //GDPR
                                if (_gdprSettings.GdprEnabled && _gdprSettings.LogNewsletterConsent)
                                {
                                    _gdprService.InsertLog(user, 0, GdprRequestType.ConsentAgree, _localizationService.GetResource("Gdpr.Consent.Newsletter"));
                                }
                            }
                        }
                    }

                    if (_forumSettings.ForumsEnabled && _forumSettings.SignaturesEnabled)
                        _genericAttributeService.SaveAttribute(user, AppUserDefaults.SignatureAttribute, model.Signature);

                    //save user attributes
                    _genericAttributeService.SaveAttribute(_workContext.CurrentUser,
                        AppUserDefaults.CustomUserAttributes, userAttributesXml);

                    //GDPR
                    if (_gdprSettings.GdprEnabled)
                    {
                        var consents = _gdprService.GetAllConsents().Where(consent => consent.DisplayOnUserInfoPage).ToList();
                        foreach (var consent in consents)
                        {
                            var previousConsentValue = _gdprService.IsConsentAccepted(consent.Id, _workContext.CurrentUser.Id);
                            var controlId = $"consent{consent.Id}";
                            var cbConsent = model.Form[controlId];
                            if (!StringValues.IsNullOrEmpty(cbConsent) && cbConsent.ToString().Equals("on"))
                            {
                                //agree
                                if (!previousConsentValue.HasValue || !previousConsentValue.Value)
                                {
                                    _gdprService.InsertLog(user, consent.Id, GdprRequestType.ConsentAgree, consent.Message);
                                }
                            }
                            else
                            {
                                //disagree
                                if (!previousConsentValue.HasValue || previousConsentValue.Value)
                                {
                                    _gdprService.InsertLog(user, consent.Id, GdprRequestType.ConsentDisagree, consent.Message);
                                }
                            }
                        }
                    }

                    return RedirectToRoute("UserInfo");
                }
            }
            catch (Exception exc)
            {
                ModelState.AddModelError("", exc.Message);
            }

            //If we got this far, something failed, redisplay form
            model = _userModelFactory.PrepareUserInfoModel(model, user, true, userAttributesXml);
            return View(model);
        }

        [HttpPost]
        [PublicAntiForgery]
        public virtual IActionResult RemoveExternalAssociation(int id)
        {
            if (!_workContext.CurrentUser.IsRegistered())
                return Challenge();

            //ensure it's our record
            var ear = _workContext.CurrentUser.ExternalAuthenticationRecords.FirstOrDefault(x => x.Id == id);

            if (ear == null)
            {
                return Json(new
                {
                    redirect = Url.Action("Info"),
                });
            }

            _externalAuthenticationService.DeleteExternalAuthenticationRecord(ear);

            return Json(new
            {
                redirect = Url.Action("Info"),
            });
        }

        [HttpsRequirement(SslRequirement.Yes)]
        //available even when navigation is not allowed
        [CheckAccessPublicStore(true)]
        public virtual IActionResult EmailRevalidation(string token, string email)
        {
            var user = _userService.GetUserByEmail(email);
            if (user == null)
                return RedirectToRoute("HomePage");

            var cToken = _genericAttributeService.GetAttribute<string>(user, AppUserDefaults.EmailRevalidationTokenAttribute);
            if (string.IsNullOrEmpty(cToken))
                return View(new EmailRevalidationModel
                {
                    Result = _localizationService.GetResource("Account.EmailRevalidation.AlreadyChanged")
                });

            if (!cToken.Equals(token, StringComparison.InvariantCultureIgnoreCase))
                return RedirectToRoute("HomePage");

            if (string.IsNullOrEmpty(user.EmailToRevalidate))
                return RedirectToRoute("HomePage");

            if (_userSettings.UserRegistrationType != UserRegistrationType.EmailValidation)
                return RedirectToRoute("HomePage");

            //change email
            try
            {
                _userRegistrationService.SetEmail(user, user.EmailToRevalidate, false);
            }
            catch (Exception exc)
            {
                return View(new EmailRevalidationModel
                {
                    Result = _localizationService.GetResource(exc.Message)
                });
            }
            user.EmailToRevalidate = null;
            _userService.UpdateUser(user);
            _genericAttributeService.SaveAttribute(user, AppUserDefaults.EmailRevalidationTokenAttribute, "");

            //re-authenticate (if usernames are disabled)
            if (!_userSettings.UsernamesEnabled)
            {
                _authenticationService.SignIn(user, true);
            }

            var model = new EmailRevalidationModel()
            {
                Result = _localizationService.GetResource("Account.EmailRevalidation.Changed")
            };
            return View(model);
        }

        #endregion

        #region My account / Addresses

        [HttpsRequirement(SslRequirement.Yes)]
        public virtual IActionResult Addresses()
        {
            if (!_workContext.CurrentUser.IsRegistered())
                return Challenge();

            var model = _userModelFactory.PrepareUserAddressListModel();
            return View(model);
        }

        [HttpPost]
        [PublicAntiForgery]
        [HttpsRequirement(SslRequirement.Yes)]
        public virtual IActionResult AddressDelete(int addressId)
        {
            if (!_workContext.CurrentUser.IsRegistered())
                return Challenge();

            var user = _workContext.CurrentUser;

            //find address (ensure that it belongs to the current user)
            var address = user.Addresses.FirstOrDefault(a => a.Id == addressId);
            if (address != null)
            {
                _userService.RemoveUserAddress(user, address);
                _userService.UpdateUser(user);
                //now delete the address record
                _addressService.DeleteAddress(address);
            }

            //redirect to the address list page
            return Json(new
            {
                redirect = Url.RouteUrl("UserAddresses"),
            });
        }

        [HttpsRequirement(SslRequirement.Yes)]
        public virtual IActionResult AddressAdd()
        {
            if (!_workContext.CurrentUser.IsRegistered())
                return Challenge();

            var model = new UserAddressEditModel();
            _addressModelFactory.PrepareAddressModel(model.Address,
                address: null,
                excludeProperties: false,
                addressSettings: _addressSettings,
                loadCountries: () => _countryService.GetAllCountries(_workContext.WorkingLanguage.Id));

            return View(model);
        }

        [HttpPost]
        [PublicAntiForgery]
        public virtual IActionResult AddressAdd(UserAddressEditModel model)
        {
            if (!_workContext.CurrentUser.IsRegistered())
                return Challenge();

            var user = _workContext.CurrentUser;

            //custom address attributes
            var customAttributes = _addressAttributeParser.ParseCustomAddressAttributes(model.Form);
            var customAttributeWarnings = _addressAttributeParser.GetAttributeWarnings(customAttributes);
            foreach (var error in customAttributeWarnings)
            {
                ModelState.AddModelError("", error);
            }

            if (ModelState.IsValid)
            {
                var address = model.Address.ToEntity();
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

                return RedirectToRoute("UserAddresses");
            }

            //If we got this far, something failed, redisplay form
            _addressModelFactory.PrepareAddressModel(model.Address,
                address: null,
                excludeProperties: true,
                addressSettings: _addressSettings,
                loadCountries: () => _countryService.GetAllCountries(_workContext.WorkingLanguage.Id),
                overrideAttributesXml: customAttributes);

            return View(model);
        }

        [HttpsRequirement(SslRequirement.Yes)]
        public virtual IActionResult AddressEdit(int addressId)
        {
            if (!_workContext.CurrentUser.IsRegistered())
                return Challenge();

            var user = _workContext.CurrentUser;
            //find address (ensure that it belongs to the current user)
            var address = user.Addresses.FirstOrDefault(a => a.Id == addressId);
            if (address == null)
                //address is not found
                return RedirectToRoute("UserAddresses");

            var model = new UserAddressEditModel();
            _addressModelFactory.PrepareAddressModel(model.Address,
                address: address,
                excludeProperties: false,
                addressSettings: _addressSettings,
                loadCountries: () => _countryService.GetAllCountries(_workContext.WorkingLanguage.Id));

            return View(model);
        }

        [HttpPost]
        [PublicAntiForgery]
        public virtual IActionResult AddressEdit(UserAddressEditModel model, int addressId)
        {
            if (!_workContext.CurrentUser.IsRegistered())
                return Challenge();

            var user = _workContext.CurrentUser;
            //find address (ensure that it belongs to the current user)
            var address = user.Addresses.FirstOrDefault(a => a.Id == addressId);
            if (address == null)
                //address is not found
                return RedirectToRoute("UserAddresses");

            //custom address attributes
            var customAttributes = _addressAttributeParser.ParseCustomAddressAttributes(model.Form);
            var customAttributeWarnings = _addressAttributeParser.GetAttributeWarnings(customAttributes);
            foreach (var error in customAttributeWarnings)
            {
                ModelState.AddModelError("", error);
            }

            if (ModelState.IsValid)
            {
                address = model.Address.ToEntity(address);
                address.CustomAttributes = customAttributes;
                _addressService.UpdateAddress(address);

                return RedirectToRoute("UserAddresses");
            }

            //If we got this far, something failed, redisplay form
            _addressModelFactory.PrepareAddressModel(model.Address,
                address: address,
                excludeProperties: true,
                addressSettings: _addressSettings,
                loadCountries: () => _countryService.GetAllCountries(_workContext.WorkingLanguage.Id),
                overrideAttributesXml: customAttributes);
            return View(model);
        }

        #endregion

        #region My account / Downloadable products

        [HttpsRequirement(SslRequirement.Yes)]
        public virtual IActionResult DownloadableProducts()
        {
            if (!_workContext.CurrentUser.IsRegistered())
                return Challenge();

            if (_userSettings.HideDownloadableProductsTab)
                return RedirectToRoute("UserInfo");

            var model = _userModelFactory.PrepareUserDownloadableProductsModel();
            return View(model);
        }

        public virtual IActionResult UserAgreement(Guid orderItemId)
        {
            var orderItem = _orderService.GetOrderItemByGuid(orderItemId);
            if (orderItem == null)
                return RedirectToRoute("HomePage");

            var product = orderItem.Product;
            if (product == null || !product.HasUserAgreement)
                return RedirectToRoute("HomePage");

            var model = _userModelFactory.PrepareUserAgreementModel(orderItem, product);
            return View(model);
        }

        #endregion

        #region My account / Change password

        [HttpsRequirement(SslRequirement.Yes)]
        public virtual IActionResult ChangePassword()
        {
            if (!_workContext.CurrentUser.IsRegistered())
                return Challenge();

            var model = _userModelFactory.PrepareChangePasswordModel();

            //display the cause of the change password 
            if (_userService.PasswordIsExpired(_workContext.CurrentUser))
                ModelState.AddModelError(string.Empty, _localizationService.GetResource("Account.ChangePassword.PasswordIsExpired"));

            return View(model);
        }

        [HttpPost]
        [PublicAntiForgery]
        public virtual IActionResult ChangePassword(ChangePasswordModel model)
        {
            if (!_workContext.CurrentUser.IsRegistered())
                return Challenge();

            var user = _workContext.CurrentUser;

            if (ModelState.IsValid)
            {
                var changePasswordRequest = new ChangePasswordRequest(user.Email,
                    true, _userSettings.DefaultPasswordFormat, model.NewPassword, model.OldPassword);
                var changePasswordResult = _userRegistrationService.ChangePassword(changePasswordRequest);
                if (changePasswordResult.Success)
                {
                    model.Result = _localizationService.GetResource("Account.ChangePassword.Success");
                    return View(model);
                }

                //errors
                foreach (var error in changePasswordResult.Errors)
                    ModelState.AddModelError("", error);
            }

            //If we got this far, something failed, redisplay form
            return View(model);
        }

        #endregion

        #region My account / Avatar

        [HttpsRequirement(SslRequirement.Yes)]
        public virtual IActionResult Avatar()
        {
            if (!_workContext.CurrentUser.IsRegistered())
                return Challenge();

            if (!_userSettings.AllowUsersToUploadAvatars)
                return RedirectToRoute("UserInfo");

            var model = new UserAvatarModel();
            model = _userModelFactory.PrepareUserAvatarModel(model);
            return View(model);
        }

        [HttpPost, ActionName("Avatar")]
        [PublicAntiForgery]
        [FormValueRequired("upload-avatar")]
        public virtual IActionResult UploadAvatar(UserAvatarModel model, IFormFile uploadedFile)
        {
            if (!_workContext.CurrentUser.IsRegistered())
                return Challenge();

            if (!_userSettings.AllowUsersToUploadAvatars)
                return RedirectToRoute("UserInfo");

            var user = _workContext.CurrentUser;

            if (ModelState.IsValid)
            {
                try
                {
                    var userAvatar = _pictureService.GetPictureById(_genericAttributeService.GetAttribute<int>(user, AppUserDefaults.AvatarPictureIdAttribute));
                    if (uploadedFile != null && !string.IsNullOrEmpty(uploadedFile.FileName))
                    {
                        var avatarMaxSize = _userSettings.AvatarMaximumSizeBytes;
                        if (uploadedFile.Length > avatarMaxSize)
                            throw new AppException(string.Format(_localizationService.GetResource("Account.Avatar.MaximumUploadedFileSize"), avatarMaxSize));

                        var userPictureBinary = _downloadService.GetDownloadBits(uploadedFile);
                        if (userAvatar != null)
                            userAvatar = _pictureService.UpdatePicture(userAvatar.Id, userPictureBinary, uploadedFile.ContentType, null);
                        else
                            userAvatar = _pictureService.InsertPicture(userPictureBinary, uploadedFile.ContentType, null);
                    }

                    var userAvatarId = 0;
                    if (userAvatar != null)
                        userAvatarId = userAvatar.Id;

                    _genericAttributeService.SaveAttribute(user, AppUserDefaults.AvatarPictureIdAttribute, userAvatarId);

                    model.AvatarUrl = _pictureService.GetPictureUrl(
                        _genericAttributeService.GetAttribute<int>(user, AppUserDefaults.AvatarPictureIdAttribute),
                        _mediaSettings.AvatarPictureSize,
                        false);
                    return View(model);
                }
                catch (Exception exc)
                {
                    ModelState.AddModelError("", exc.Message);
                }
            }

            //If we got this far, something failed, redisplay form
            model = _userModelFactory.PrepareUserAvatarModel(model);
            return View(model);
        }

        [HttpPost, ActionName("Avatar")]
        [PublicAntiForgery]
        [FormValueRequired("remove-avatar")]
        public virtual IActionResult RemoveAvatar(UserAvatarModel model)
        {
            if (!_workContext.CurrentUser.IsRegistered())
                return Challenge();

            if (!_userSettings.AllowUsersToUploadAvatars)
                return RedirectToRoute("UserInfo");

            var user = _workContext.CurrentUser;

            var userAvatar = _pictureService.GetPictureById(_genericAttributeService.GetAttribute<int>(user, AppUserDefaults.AvatarPictureIdAttribute));
            if (userAvatar != null)
                _pictureService.DeletePicture(userAvatar);
            _genericAttributeService.SaveAttribute(user, AppUserDefaults.AvatarPictureIdAttribute, 0);

            return RedirectToRoute("UserAvatar");
        }

        #endregion

        #region GDPR tools

        [HttpsRequirement(SslRequirement.Yes)]
        public virtual IActionResult GdprTools()
        {
            if (!_workContext.CurrentUser.IsRegistered())
                return Challenge();

            if (!_gdprSettings.GdprEnabled)
                return RedirectToRoute("UserInfo");

            var model = _userModelFactory.PrepareGdprToolsModel();
            return View(model);
        }

        [HttpPost, ActionName("GdprTools")]
        [PublicAntiForgery]
        [FormValueRequired("export-data")]
        public virtual IActionResult GdprToolsExport()
        {
            if (!_workContext.CurrentUser.IsRegistered())
                return Challenge();

            if (!_gdprSettings.GdprEnabled)
                return RedirectToRoute("UserInfo");

            //log
            _gdprService.InsertLog(_workContext.CurrentUser, 0, GdprRequestType.ExportData, _localizationService.GetResource("Gdpr.Exported"));

            //export
            var bytes = _exportManager.ExportUserGdprInfoToXlsx(_workContext.CurrentUser, _storeContext.CurrentStore.Id);

            return File(bytes, MimeTypes.TextXlsx, "userdata.xlsx");
        }

        [HttpPost, ActionName("GdprTools")]
        [PublicAntiForgery]
        [FormValueRequired("delete-account")]
        public virtual IActionResult GdprToolsDelete()
        {
            if (!_workContext.CurrentUser.IsRegistered())
                return Challenge();

            if (!_gdprSettings.GdprEnabled)
                return RedirectToRoute("UserInfo");

            //log
            _gdprService.InsertLog(_workContext.CurrentUser, 0, GdprRequestType.DeleteUser, _localizationService.GetResource("Gdpr.DeleteRequested"));

            var model = _userModelFactory.PrepareGdprToolsModel();
            model.Result = _localizationService.GetResource("Gdpr.DeleteRequested.Success");
            return View(model);
        }

        #endregion

        #region Check gift card balance

        //check gift card balance page
        [HttpsRequirement(SslRequirement.Yes)]
        //available even when a store is closed
        [CheckAccessClosedStore(true)]
        public virtual IActionResult CheckGiftCardBalance()
        {
            if (!(_captchaSettings.Enabled && _userSettings.AllowUsersToCheckGiftCardBalance))
            {
                return RedirectToRoute("UserInfo");
            }

            var model = _userModelFactory.PrepareCheckGiftCardBalanceModel();
            return View(model);
        }

        [HttpPost, ActionName("CheckGiftCardBalance")]
        [FormValueRequired("checkbalancegiftcard")]
        [ValidateCaptcha]
        public virtual IActionResult CheckBalance(CheckGiftCardBalanceModel model, bool captchaValid)
        {
            //validate CAPTCHA
            if (_captchaSettings.Enabled && !captchaValid)
            {
                ModelState.AddModelError("", _captchaSettings.GetWrongCaptchaMessage(_localizationService));
            }

            if (ModelState.IsValid)
            {
                var giftCard = _giftCardService.GetAllGiftCards(giftCardCouponCode: model.GiftCardCode).FirstOrDefault();
                if (giftCard != null && _giftCardService.IsGiftCardValid(giftCard))
                {
                    var remainingAmount = _currencyService.ConvertFromPrimaryStoreCurrency(_giftCardService.GetGiftCardRemainingAmount(giftCard), _workContext.WorkingCurrency);
                    model.Result = _priceFormatter.FormatPrice(remainingAmount, true, false);
                }
                else
                {
                    model.Message = _localizationService.GetResource("CheckGiftCardBalance.GiftCardCouponCode.Invalid");
                }
            }

            return View(model);
        }

        #endregion

        #endregion
    }
}