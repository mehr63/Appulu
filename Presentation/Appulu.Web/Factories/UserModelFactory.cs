using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Appulu.Core;
using Appulu.Core.Domain.Catalog;
using Appulu.Core.Domain.Common;
using Appulu.Core.Domain.Users;
using Appulu.Core.Domain.Forums;
using Appulu.Core.Domain.Gdpr;
using Appulu.Core.Domain.Media;
using Appulu.Core.Domain.Orders;
using Appulu.Core.Domain.Security;
using Appulu.Core.Domain.Tax;
using Appulu.Core.Domain.Vendors;
using Appulu.Services.Authentication.External;
using Appulu.Services.Common;
using Appulu.Services.Users;
using Appulu.Services.Directory;
using Appulu.Services.Gdpr;
using Appulu.Services.Helpers;
using Appulu.Services.Localization;
using Appulu.Services.Media;
using Appulu.Services.Messages;
using Appulu.Services.Orders;
using Appulu.Services.Seo;
using Appulu.Services.Stores;
using Appulu.Web.Models.Common;
using Appulu.Web.Models.User;

namespace Appulu.Web.Factories
{
    /// <summary>
    /// Represents the user model factory
    /// </summary>
    public partial class UserModelFactory : IUserModelFactory
    {
        #region Fields

        private readonly AddressSettings _addressSettings;
        private readonly CaptchaSettings _captchaSettings;
        private readonly CatalogSettings _catalogSettings;
        private readonly CommonSettings _commonSettings;
        private readonly UserSettings _userSettings;
        private readonly DateTimeSettings _dateTimeSettings;
        private readonly ExternalAuthenticationSettings _externalAuthenticationSettings;
        private readonly ForumSettings _forumSettings;
        private readonly GdprSettings _gdprSettings;
        private readonly IAddressModelFactory _addressModelFactory;
        private readonly ICountryService _countryService;
        private readonly IUserAttributeParser _userAttributeParser;
        private readonly IUserAttributeService _userAttributeService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IDownloadService _downloadService;
        private readonly IExternalAuthenticationService _externalAuthenticationService;
        private readonly IGdprService _gdprService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ILocalizationService _localizationService;
        private readonly INewsLetterSubscriptionService _newsLetterSubscriptionService;
        private readonly IOrderService _orderService;
        private readonly IPictureService _pictureService;
        private readonly IReturnRequestService _returnRequestService;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly IStoreContext _storeContext;
        private readonly IStoreMappingService _storeMappingService;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IWorkContext _workContext;
        private readonly MediaSettings _mediaSettings;
        private readonly OrderSettings _orderSettings;
        private readonly RewardPointsSettings _rewardPointsSettings;
        private readonly SecuritySettings _securitySettings;
        private readonly TaxSettings _taxSettings;
        private readonly VendorSettings _vendorSettings;

        #endregion

        #region Ctor

        public UserModelFactory(AddressSettings addressSettings,
            CaptchaSettings captchaSettings,
            CatalogSettings catalogSettings,
            CommonSettings commonSettings,
            UserSettings userSettings,
            DateTimeSettings dateTimeSettings,
            ExternalAuthenticationSettings externalAuthenticationSettings,
            ForumSettings forumSettings,
            GdprSettings gdprSettings,
            IAddressModelFactory addressModelFactory,
            ICountryService countryService,
            IUserAttributeParser userAttributeParser,
            IUserAttributeService userAttributeService,
            IDateTimeHelper dateTimeHelper,
            IDownloadService downloadService,
            IExternalAuthenticationService externalAuthenticationService,
            IGdprService gdprService,
            IGenericAttributeService genericAttributeService,
            ILocalizationService localizationService,
            INewsLetterSubscriptionService newsLetterSubscriptionService,
            IOrderService orderService,
            IPictureService pictureService,
            IReturnRequestService returnRequestService,
            IStateProvinceService stateProvinceService,
            IStoreContext storeContext,
            IStoreMappingService storeMappingService,
            IUrlRecordService urlRecordService,
            IWorkContext workContext,
            MediaSettings mediaSettings,
            OrderSettings orderSettings,
            RewardPointsSettings rewardPointsSettings,
            SecuritySettings securitySettings,
            TaxSettings taxSettings,
            VendorSettings vendorSettings)
        {
            this._addressSettings = addressSettings;
            this._captchaSettings = captchaSettings;
            this._catalogSettings = catalogSettings;
            this._commonSettings = commonSettings;
            this._userSettings = userSettings;
            this._dateTimeSettings = dateTimeSettings;
            this._externalAuthenticationSettings = externalAuthenticationSettings;
            this._forumSettings = forumSettings;
            this._gdprSettings = gdprSettings;
            this._addressModelFactory = addressModelFactory;
            this._countryService = countryService;
            this._userAttributeParser = userAttributeParser;
            this._userAttributeService = userAttributeService;
            this._dateTimeHelper = dateTimeHelper;
            this._downloadService = downloadService;
            this._externalAuthenticationService = externalAuthenticationService;
            this._gdprService = gdprService;
            this._genericAttributeService = genericAttributeService;
            this._localizationService = localizationService;
            this._newsLetterSubscriptionService = newsLetterSubscriptionService;
            this._orderService = orderService;
            this._pictureService = pictureService;
            this._returnRequestService = returnRequestService;
            this._stateProvinceService = stateProvinceService;
            this._storeContext = storeContext;
            this._storeMappingService = storeMappingService;
            this._urlRecordService = urlRecordService;
            this._workContext = workContext;
            this._mediaSettings = mediaSettings;
            this._orderSettings = orderSettings;
            this._rewardPointsSettings = rewardPointsSettings;
            this._securitySettings = securitySettings;
            this._taxSettings = taxSettings;
            this._vendorSettings = vendorSettings;
        }

        #endregion

        #region Utilities

        protected virtual GdprConsentModel PrepareGdprConsentModel(GdprConsent consent, bool accepted)
        {
            if (consent == null)
                throw new ArgumentNullException(nameof(consent));

            var requiredMessage = _localizationService.GetLocalized(consent, x => x.RequiredMessage);
            return new GdprConsentModel
            {
                Id = consent.Id,
                Message = _localizationService.GetLocalized(consent, x => x.Message),
                IsRequired = consent.IsRequired,
                RequiredMessage = !string.IsNullOrEmpty(requiredMessage) ? requiredMessage : $"'{consent.Message}' is required",
                Accepted = accepted
            };
        }
        #endregion

        #region Methods

        /// <summary>
        /// Prepare the custom user attribute models
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="overrideAttributesXml">Overridden user attributes in XML format; pass null to use CustomUserAttributes of user</param>
        /// <returns>List of the user attribute model</returns>
        public virtual IList<UserAttributeModel> PrepareCustomUserAttributes(User user, string overrideAttributesXml = "")
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var result = new List<UserAttributeModel>();

            var userAttributes = _userAttributeService.GetAllUserAttributes();
            foreach (var attribute in userAttributes)
            {
                var attributeModel = new UserAttributeModel
                {
                    Id = attribute.Id,
                    Name = _localizationService.GetLocalized(attribute, x => x.Name),
                    IsRequired = attribute.IsRequired,
                    AttributeControlType = attribute.AttributeControlType,
                };

                if (attribute.ShouldHaveValues())
                {
                    //values
                    var attributeValues = _userAttributeService.GetUserAttributeValues(attribute.Id);
                    foreach (var attributeValue in attributeValues)
                    {
                        var valueModel = new UserAttributeValueModel
                        {
                            Id = attributeValue.Id,
                            Name = _localizationService.GetLocalized(attributeValue, x => x.Name),
                            IsPreSelected = attributeValue.IsPreSelected
                        };
                        attributeModel.Values.Add(valueModel);
                    }
                }

                //set already selected attributes
                var selectedAttributesXml = !string.IsNullOrEmpty(overrideAttributesXml) ?
                    overrideAttributesXml :
                    _genericAttributeService.GetAttribute<string>(user, AppUserDefaults.CustomUserAttributes);
                switch (attribute.AttributeControlType)
                {
                    case AttributeControlType.DropdownList:
                    case AttributeControlType.RadioList:
                    case AttributeControlType.Checkboxes:
                        {
                            if (!string.IsNullOrEmpty(selectedAttributesXml))
                            {
                                //clear default selection
                                foreach (var item in attributeModel.Values)
                                    item.IsPreSelected = false;

                                //select new values
                                var selectedValues = _userAttributeParser.ParseUserAttributeValues(selectedAttributesXml);
                                foreach (var attributeValue in selectedValues)
                                    foreach (var item in attributeModel.Values)
                                        if (attributeValue.Id == item.Id)
                                            item.IsPreSelected = true;
                            }
                        }
                        break;
                    case AttributeControlType.ReadonlyCheckboxes:
                        {
                            //do nothing
                            //values are already pre-set
                        }
                        break;
                    case AttributeControlType.TextBox:
                    case AttributeControlType.MultilineTextbox:
                        {
                            if (!string.IsNullOrEmpty(selectedAttributesXml))
                            {
                                var enteredText = _userAttributeParser.ParseValues(selectedAttributesXml, attribute.Id);
                                if (enteredText.Any())
                                    attributeModel.DefaultValue = enteredText[0];
                            }
                        }
                        break;
                    case AttributeControlType.ColorSquares:
                    case AttributeControlType.ImageSquares:
                    case AttributeControlType.Datepicker:
                    case AttributeControlType.FileUpload:
                    default:
                        //not supported attribute control types
                        break;
                }

                result.Add(attributeModel);
            }

            return result;
        }

        /// <summary>
        /// Prepare the user info model
        /// </summary>
        /// <param name="model">User info model</param>
        /// <param name="user">User</param>
        /// <param name="excludeProperties">Whether to exclude populating of model properties from the entity</param>
        /// <param name="overrideCustomUserAttributesXml">Overridden user attributes in XML format; pass null to use CustomUserAttributes of user</param>
        /// <returns>User info model</returns>
        public virtual UserInfoModel PrepareUserInfoModel(UserInfoModel model, User user,
            bool excludeProperties, string overrideCustomUserAttributesXml = "")
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            model.AllowUsersToSetTimeZone = _dateTimeSettings.AllowUsersToSetTimeZone;
            foreach (var tzi in _dateTimeHelper.GetSystemTimeZones())
                model.AvailableTimeZones.Add(new SelectListItem { Text = tzi.DisplayName, Value = tzi.Id, Selected = (excludeProperties ? tzi.Id == model.TimeZoneId : tzi.Id == _dateTimeHelper.CurrentTimeZone.Id) });

            if (!excludeProperties)
            {
                model.VatNumber = _genericAttributeService.GetAttribute<string>(user, AppUserDefaults.VatNumberAttribute);
                model.FirstName = _genericAttributeService.GetAttribute<string>(user, AppUserDefaults.FirstNameAttribute);
                model.LastName = _genericAttributeService.GetAttribute<string>(user, AppUserDefaults.LastNameAttribute);
                model.Gender = _genericAttributeService.GetAttribute<string>(user, AppUserDefaults.GenderAttribute);
                var dateOfBirth = _genericAttributeService.GetAttribute<DateTime?>(user, AppUserDefaults.DateOfBirthAttribute);
                if (dateOfBirth.HasValue)
                {
                    model.DateOfBirthDay = dateOfBirth.Value.Day;
                    model.DateOfBirthMonth = dateOfBirth.Value.Month;
                    model.DateOfBirthYear = dateOfBirth.Value.Year;
                }
                model.Company = _genericAttributeService.GetAttribute<string>(user, AppUserDefaults.CompanyAttribute);
                model.StreetAddress = _genericAttributeService.GetAttribute<string>(user, AppUserDefaults.StreetAddressAttribute);
                model.StreetAddress2 = _genericAttributeService.GetAttribute<string>(user, AppUserDefaults.StreetAddress2Attribute);
                model.ZipPostalCode = _genericAttributeService.GetAttribute<string>(user, AppUserDefaults.ZipPostalCodeAttribute);
                model.City = _genericAttributeService.GetAttribute<string>(user, AppUserDefaults.CityAttribute);
                model.County = _genericAttributeService.GetAttribute<string>(user, AppUserDefaults.CountyAttribute);
                model.CountryId = _genericAttributeService.GetAttribute<int>(user, AppUserDefaults.CountryIdAttribute);
                model.StateProvinceId = _genericAttributeService.GetAttribute<int>(user, AppUserDefaults.StateProvinceIdAttribute);
                model.Phone = _genericAttributeService.GetAttribute<string>(user, AppUserDefaults.PhoneAttribute);
                model.Fax = _genericAttributeService.GetAttribute<string>(user, AppUserDefaults.FaxAttribute);

                //newsletter
                var newsletter = _newsLetterSubscriptionService.GetNewsLetterSubscriptionByEmailAndStoreId(user.Email, _storeContext.CurrentStore.Id);
                model.Newsletter = newsletter != null && newsletter.Active;

                model.Signature = _genericAttributeService.GetAttribute<string>(user, AppUserDefaults.SignatureAttribute);

                model.Email = user.Email;
                model.Username = user.Username;
            }
            else
            {
                if (_userSettings.UsernamesEnabled && !_userSettings.AllowUsersToChangeUsernames)
                    model.Username = user.Username;
            }

            if (_userSettings.UserRegistrationType == UserRegistrationType.EmailValidation)
                model.EmailToRevalidate = user.EmailToRevalidate;

            //countries and states
            if (_userSettings.CountryEnabled)
            {
                model.AvailableCountries.Add(new SelectListItem { Text = _localizationService.GetResource("Address.SelectCountry"), Value = "0" });
                foreach (var c in _countryService.GetAllCountries(_workContext.WorkingLanguage.Id))
                {
                    model.AvailableCountries.Add(new SelectListItem
                    {
                        Text = _localizationService.GetLocalized(c, x => x.Name),
                        Value = c.Id.ToString(),
                        Selected = c.Id == model.CountryId
                    });
                }

                if (_userSettings.StateProvinceEnabled)
                {
                    //states
                    var states = _stateProvinceService.GetStateProvincesByCountryId(model.CountryId, _workContext.WorkingLanguage.Id).ToList();
                    if (states.Any())
                    {
                        model.AvailableStates.Add(new SelectListItem { Text = _localizationService.GetResource("Address.SelectState"), Value = "0" });

                        foreach (var s in states)
                        {
                            model.AvailableStates.Add(new SelectListItem { Text = _localizationService.GetLocalized(s, x => x.Name), Value = s.Id.ToString(), Selected = (s.Id == model.StateProvinceId) });
                        }
                    }
                    else
                    {
                        var anyCountrySelected = model.AvailableCountries.Any(x => x.Selected);

                        model.AvailableStates.Add(new SelectListItem
                        {
                            Text = _localizationService.GetResource(anyCountrySelected ? "Address.OtherNonUS" : "Address.SelectState"),
                            Value = "0"
                        });
                    }

                }
            }

            model.DisplayVatNumber = _taxSettings.EuVatEnabled;
            model.VatNumberStatusNote = _localizationService.GetLocalizedEnum((VatNumberStatus)_genericAttributeService
                .GetAttribute<int>(user, AppUserDefaults.VatNumberStatusIdAttribute));
            model.GenderEnabled = _userSettings.GenderEnabled;
            model.DateOfBirthEnabled = _userSettings.DateOfBirthEnabled;
            model.DateOfBirthRequired = _userSettings.DateOfBirthRequired;
            model.CompanyEnabled = _userSettings.CompanyEnabled;
            model.CompanyRequired = _userSettings.CompanyRequired;
            model.StreetAddressEnabled = _userSettings.StreetAddressEnabled;
            model.StreetAddressRequired = _userSettings.StreetAddressRequired;
            model.StreetAddress2Enabled = _userSettings.StreetAddress2Enabled;
            model.StreetAddress2Required = _userSettings.StreetAddress2Required;
            model.ZipPostalCodeEnabled = _userSettings.ZipPostalCodeEnabled;
            model.ZipPostalCodeRequired = _userSettings.ZipPostalCodeRequired;
            model.CityEnabled = _userSettings.CityEnabled;
            model.CityRequired = _userSettings.CityRequired;
            model.CountyEnabled = _userSettings.CountyEnabled;
            model.CountyRequired = _userSettings.CountyRequired;
            model.CountryEnabled = _userSettings.CountryEnabled;
            model.CountryRequired = _userSettings.CountryRequired;
            model.StateProvinceEnabled = _userSettings.StateProvinceEnabled;
            model.StateProvinceRequired = _userSettings.StateProvinceRequired;
            model.PhoneEnabled = _userSettings.PhoneEnabled;
            model.PhoneRequired = _userSettings.PhoneRequired;
            model.FaxEnabled = _userSettings.FaxEnabled;
            model.FaxRequired = _userSettings.FaxRequired;
            model.NewsletterEnabled = _userSettings.NewsletterEnabled;
            model.UsernamesEnabled = _userSettings.UsernamesEnabled;
            model.AllowUsersToChangeUsernames = _userSettings.AllowUsersToChangeUsernames;
            model.CheckUsernameAvailabilityEnabled = _userSettings.CheckUsernameAvailabilityEnabled;
            model.SignatureEnabled = _forumSettings.ForumsEnabled && _forumSettings.SignaturesEnabled;

            //external authentication
            model.AllowUsersToRemoveAssociations = _externalAuthenticationSettings.AllowUsersToRemoveAssociations;
            model.NumberOfExternalAuthenticationProviders = _externalAuthenticationService
                .LoadActiveExternalAuthenticationMethods(_workContext.CurrentUser, _storeContext.CurrentStore.Id).Count;
            foreach (var record in user.ExternalAuthenticationRecords)
            {
                var authMethod = _externalAuthenticationService.LoadExternalAuthenticationMethodBySystemName(record.ProviderSystemName);
                if (authMethod == null || !_externalAuthenticationService.IsExternalAuthenticationMethodActive(authMethod))
                    continue;

                model.AssociatedExternalAuthRecords.Add(new UserInfoModel.AssociatedExternalAuthModel
                {
                    Id = record.Id,
                    Email = record.Email,
                    ExternalIdentifier = !string.IsNullOrEmpty(record.ExternalDisplayIdentifier)
                        ? record.ExternalDisplayIdentifier : record.ExternalIdentifier,
                    AuthMethodName = _localizationService.GetLocalizedFriendlyName(authMethod, _workContext.WorkingLanguage.Id)
                });
            }

            //custom user attributes
            var customAttributes = PrepareCustomUserAttributes(user, overrideCustomUserAttributesXml);
            foreach (var attribute in customAttributes)
                model.UserAttributes.Add(attribute);

            //GDPR
            if (_gdprSettings.GdprEnabled)
            {
                var consents = _gdprService.GetAllConsents().Where(consent => consent.DisplayOnUserInfoPage).ToList();
                foreach (var consent in consents)
                {
                    var accepted = _gdprService.IsConsentAccepted(consent.Id, _workContext.CurrentUser.Id);
                    model.GdprConsents.Add(PrepareGdprConsentModel(consent, accepted.HasValue && accepted.Value));
                }
            }

            return model;
        }

        /// <summary>
        /// Prepare the user register model
        /// </summary>
        /// <param name="model">User register model</param>
        /// <param name="excludeProperties">Whether to exclude populating of model properties from the entity</param>
        /// <param name="overrideCustomUserAttributesXml">Overridden user attributes in XML format; pass null to use CustomUserAttributes of user</param>
        /// <param name="setDefaultValues">Whether to populate model properties by default values</param>
        /// <returns>User register model</returns>
        public virtual RegisterModel PrepareRegisterModel(RegisterModel model, bool excludeProperties,
            string overrideCustomUserAttributesXml = "", bool setDefaultValues = false)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            model.AllowUsersToSetTimeZone = _dateTimeSettings.AllowUsersToSetTimeZone;
            foreach (var tzi in _dateTimeHelper.GetSystemTimeZones())
                model.AvailableTimeZones.Add(new SelectListItem { Text = tzi.DisplayName, Value = tzi.Id, Selected = (excludeProperties ? tzi.Id == model.TimeZoneId : tzi.Id == _dateTimeHelper.CurrentTimeZone.Id) });

            model.DisplayVatNumber = _taxSettings.EuVatEnabled;
            //form fields
            model.GenderEnabled = _userSettings.GenderEnabled;
            model.DateOfBirthEnabled = _userSettings.DateOfBirthEnabled;
            model.DateOfBirthRequired = _userSettings.DateOfBirthRequired;
            model.CompanyEnabled = _userSettings.CompanyEnabled;
            model.CompanyRequired = _userSettings.CompanyRequired;
            model.StreetAddressEnabled = _userSettings.StreetAddressEnabled;
            model.StreetAddressRequired = _userSettings.StreetAddressRequired;
            model.StreetAddress2Enabled = _userSettings.StreetAddress2Enabled;
            model.StreetAddress2Required = _userSettings.StreetAddress2Required;
            model.ZipPostalCodeEnabled = _userSettings.ZipPostalCodeEnabled;
            model.ZipPostalCodeRequired = _userSettings.ZipPostalCodeRequired;
            model.CityEnabled = _userSettings.CityEnabled;
            model.CityRequired = _userSettings.CityRequired;
            model.CountyEnabled = _userSettings.CountyEnabled;
            model.CountyRequired = _userSettings.CountyRequired;
            model.CountryEnabled = _userSettings.CountryEnabled;
            model.CountryRequired = _userSettings.CountryRequired;
            model.StateProvinceEnabled = _userSettings.StateProvinceEnabled;
            model.StateProvinceRequired = _userSettings.StateProvinceRequired;
            model.PhoneEnabled = _userSettings.PhoneEnabled;
            model.PhoneRequired = _userSettings.PhoneRequired;
            model.FaxEnabled = _userSettings.FaxEnabled;
            model.FaxRequired = _userSettings.FaxRequired;
            model.NewsletterEnabled = _userSettings.NewsletterEnabled;
            model.AcceptPrivacyPolicyEnabled = _userSettings.AcceptPrivacyPolicyEnabled;
            model.AcceptPrivacyPolicyPopup = _commonSettings.PopupForTermsOfServiceLinks;
            model.UsernamesEnabled = _userSettings.UsernamesEnabled;
            model.CheckUsernameAvailabilityEnabled = _userSettings.CheckUsernameAvailabilityEnabled;
            model.HoneypotEnabled = _securitySettings.HoneypotEnabled;
            model.DisplayCaptcha = _captchaSettings.Enabled && _captchaSettings.ShowOnRegistrationPage;
            model.EnteringEmailTwice = _userSettings.EnteringEmailTwice;
            if (setDefaultValues)
            {
                //enable newsletter by default
                model.Newsletter = _userSettings.NewsletterTickedByDefault;
            }

            //countries and states
            if (_userSettings.CountryEnabled)
            {
                model.AvailableCountries.Add(new SelectListItem { Text = _localizationService.GetResource("Address.SelectCountry"), Value = "0" });

                foreach (var c in _countryService.GetAllCountries(_workContext.WorkingLanguage.Id))
                {
                    model.AvailableCountries.Add(new SelectListItem
                    {
                        Text = _localizationService.GetLocalized(c, x => x.Name),
                        Value = c.Id.ToString(),
                        Selected = c.Id == model.CountryId
                    });
                }

                if (_userSettings.StateProvinceEnabled)
                {
                    //states
                    var states = _stateProvinceService.GetStateProvincesByCountryId(model.CountryId, _workContext.WorkingLanguage.Id).ToList();
                    if (states.Any())
                    {
                        model.AvailableStates.Add(new SelectListItem { Text = _localizationService.GetResource("Address.SelectState"), Value = "0" });

                        foreach (var s in states)
                        {
                            model.AvailableStates.Add(new SelectListItem { Text = _localizationService.GetLocalized(s, x => x.Name), Value = s.Id.ToString(), Selected = (s.Id == model.StateProvinceId) });
                        }
                    }
                    else
                    {
                        var anyCountrySelected = model.AvailableCountries.Any(x => x.Selected);

                        model.AvailableStates.Add(new SelectListItem
                        {
                            Text = _localizationService.GetResource(anyCountrySelected ? "Address.OtherNonUS" : "Address.SelectState"),
                            Value = "0"
                        });
                    }

                }
            }

            //custom user attributes
            var customAttributes = PrepareCustomUserAttributes(_workContext.CurrentUser, overrideCustomUserAttributesXml); foreach (var attribute in customAttributes)
                model.UserAttributes.Add(attribute);

            //GDPR
            if (_gdprSettings.GdprEnabled)
            {
                var consents = _gdprService.GetAllConsents().Where(consent => consent.DisplayDuringRegistration).ToList();
                foreach (var consent in consents)
                {
                    model.GdprConsents.Add(PrepareGdprConsentModel(consent, false));
                }
            }

            return model;
        }

        /// <summary>
        /// Prepare the login model
        /// </summary>
        /// <param name="checkoutAsGuest">Whether to checkout as guest is enabled</param>
        /// <returns>Login model</returns>
        public virtual LoginModel PrepareLoginModel(bool? checkoutAsGuest)
        {
            var model = new LoginModel
            {
                UsernamesEnabled = _userSettings.UsernamesEnabled,
                CheckoutAsGuest = checkoutAsGuest.GetValueOrDefault(),
                DisplayCaptcha = _captchaSettings.Enabled && _captchaSettings.ShowOnLoginPage
            };
            return model;
        }

        /// <summary>
        /// Prepare the password recovery model
        /// </summary>
        /// <returns>Password recovery model</returns>
        public virtual PasswordRecoveryModel PreparePasswordRecoveryModel()
        {
            var model = new PasswordRecoveryModel();
            return model;
        }

        /// <summary>
        /// Prepare the password recovery confirm model
        /// </summary>
        /// <returns>Password recovery confirm model</returns>
        public virtual PasswordRecoveryConfirmModel PreparePasswordRecoveryConfirmModel()
        {
            var model = new PasswordRecoveryConfirmModel();
            return model;
        }

        /// <summary>
        /// Prepare the register result model
        /// </summary>
        /// <param name="resultId">Value of UserRegistrationType enum</param>
        /// <returns>Register result model</returns>
        public virtual RegisterResultModel PrepareRegisterResultModel(int resultId)
        {
            var resultText = "";
            switch ((UserRegistrationType)resultId)
            {
                case UserRegistrationType.Disabled:
                    resultText = _localizationService.GetResource("Account.Register.Result.Disabled");
                    break;
                case UserRegistrationType.Standard:
                    resultText = _localizationService.GetResource("Account.Register.Result.Standard");
                    break;
                case UserRegistrationType.AdminApproval:
                    resultText = _localizationService.GetResource("Account.Register.Result.AdminApproval");
                    break;
                case UserRegistrationType.EmailValidation:
                    resultText = _localizationService.GetResource("Account.Register.Result.EmailValidation");
                    break;
                default:
                    break;
            }
            var model = new RegisterResultModel
            {
                Result = resultText
            };
            return model;
        }

        /// <summary>
        /// Prepare the user navigation model
        /// </summary>
        /// <param name="selectedTabId">Identifier of the selected tab</param>
        /// <returns>User navigation model</returns>
        public virtual UserNavigationModel PrepareUserNavigationModel(int selectedTabId = 0)
        {
            var model = new UserNavigationModel();

            model.UserNavigationItems.Add(new UserNavigationItemModel
            {
                RouteName = "UserInfo",
                Title = _localizationService.GetResource("Account.UserInfo"),
                Tab = UserNavigationEnum.Info,
                ItemClass = "user-info"
            });

            model.UserNavigationItems.Add(new UserNavigationItemModel
            {
                RouteName = "UserAddresses",
                Title = _localizationService.GetResource("Account.UserAddresses"),
                Tab = UserNavigationEnum.Addresses,
                ItemClass = "user-addresses"
            });

            model.UserNavigationItems.Add(new UserNavigationItemModel
            {
                RouteName = "UserOrders",
                Title = _localizationService.GetResource("Account.UserOrders"),
                Tab = UserNavigationEnum.Orders,
                ItemClass = "user-orders"
            });

            if (_orderSettings.ReturnRequestsEnabled &&
                _returnRequestService.SearchReturnRequests(_storeContext.CurrentStore.Id,
                    _workContext.CurrentUser.Id, pageIndex: 0, pageSize: 1).Any())
            {
                model.UserNavigationItems.Add(new UserNavigationItemModel
                {
                    RouteName = "UserReturnRequests",
                    Title = _localizationService.GetResource("Account.UserReturnRequests"),
                    Tab = UserNavigationEnum.ReturnRequests,
                    ItemClass = "return-requests"
                });
            }

            if (!_userSettings.HideDownloadableProductsTab)
            {
                model.UserNavigationItems.Add(new UserNavigationItemModel
                {
                    RouteName = "UserDownloadableProducts",
                    Title = _localizationService.GetResource("Account.DownloadableProducts"),
                    Tab = UserNavigationEnum.DownloadableProducts,
                    ItemClass = "downloadable-products"
                });
            }

            if (!_userSettings.HideBackInStockSubscriptionsTab)
            {
                model.UserNavigationItems.Add(new UserNavigationItemModel
                {
                    RouteName = "UserBackInStockSubscriptions",
                    Title = _localizationService.GetResource("Account.BackInStockSubscriptions"),
                    Tab = UserNavigationEnum.BackInStockSubscriptions,
                    ItemClass = "back-in-stock-subscriptions"
                });
            }

            if (_rewardPointsSettings.Enabled)
            {
                model.UserNavigationItems.Add(new UserNavigationItemModel
                {
                    RouteName = "UserRewardPoints",
                    Title = _localizationService.GetResource("Account.RewardPoints"),
                    Tab = UserNavigationEnum.RewardPoints,
                    ItemClass = "reward-points"
                });
            }

            model.UserNavigationItems.Add(new UserNavigationItemModel
            {
                RouteName = "UserChangePassword",
                Title = _localizationService.GetResource("Account.ChangePassword"),
                Tab = UserNavigationEnum.ChangePassword,
                ItemClass = "change-password"
            });

            if (_userSettings.AllowUsersToUploadAvatars)
            {
                model.UserNavigationItems.Add(new UserNavigationItemModel
                {
                    RouteName = "UserAvatar",
                    Title = _localizationService.GetResource("Account.Avatar"),
                    Tab = UserNavigationEnum.Avatar,
                    ItemClass = "user-avatar"
                });
            }

            if (_forumSettings.ForumsEnabled && _forumSettings.AllowUsersToManageSubscriptions)
            {
                model.UserNavigationItems.Add(new UserNavigationItemModel
                {
                    RouteName = "UserForumSubscriptions",
                    Title = _localizationService.GetResource("Account.ForumSubscriptions"),
                    Tab = UserNavigationEnum.ForumSubscriptions,
                    ItemClass = "forum-subscriptions"
                });
            }
            if (_catalogSettings.ShowProductReviewsTabOnAccountPage)
            {
                model.UserNavigationItems.Add(new UserNavigationItemModel
                {
                    RouteName = "UserProductReviews",
                    Title = _localizationService.GetResource("Account.UserProductReviews"),
                    Tab = UserNavigationEnum.ProductReviews,
                    ItemClass = "user-reviews"
                });
            }
            if (_vendorSettings.AllowVendorsToEditInfo && _workContext.CurrentVendor != null)
            {
                model.UserNavigationItems.Add(new UserNavigationItemModel
                {
                    RouteName = "UserVendorInfo",
                    Title = _localizationService.GetResource("Account.VendorInfo"),
                    Tab = UserNavigationEnum.VendorInfo,
                    ItemClass = "user-vendor-info"
                });
            }
            if (_gdprSettings.GdprEnabled)
            {
                model.UserNavigationItems.Add(new UserNavigationItemModel
                {
                    RouteName = "GdprTools",
                    Title = _localizationService.GetResource("Account.Gdpr"),
                    Tab = UserNavigationEnum.GdprTools,
                    ItemClass = "user-gdpr"
                });
            }

            if (_captchaSettings.Enabled && _userSettings.AllowUsersToCheckGiftCardBalance)
            {
                model.UserNavigationItems.Add(new UserNavigationItemModel
                {
                    RouteName = "CheckGiftCardBalance",
                    Title = _localizationService.GetResource("CheckGiftCardBalance"),
                    Tab = UserNavigationEnum.CheckGiftCardBalance,
                    ItemClass = "user-check-gift-card-balance"
                });
            }

            model.SelectedTab = (UserNavigationEnum)selectedTabId;

            return model;
        }

        /// <summary>
        /// Prepare the user address list model
        /// </summary>
        /// <returns>User address list model</returns>
        public virtual UserAddressListModel PrepareUserAddressListModel()
        {
            var addresses = _workContext.CurrentUser.Addresses
                //enabled for the current store
                .Where(a => a.Country == null || _storeMappingService.Authorize(a.Country))
                .ToList();

            var model = new UserAddressListModel();
            foreach (var address in addresses)
            {
                var addressModel = new AddressModel();
                _addressModelFactory.PrepareAddressModel(addressModel,
                    address: address,
                    excludeProperties: false,
                    addressSettings: _addressSettings,
                    loadCountries: () => _countryService.GetAllCountries(_workContext.WorkingLanguage.Id));
                model.Addresses.Add(addressModel);
            }
            return model;
        }

        /// <summary>
        /// Prepare the user downloadable products model
        /// </summary>
        /// <returns>User downloadable products model</returns>
        public virtual UserDownloadableProductsModel PrepareUserDownloadableProductsModel()
        {
            var model = new UserDownloadableProductsModel();
            var items = _orderService.GetDownloadableOrderItems(_workContext.CurrentUser.Id);
            foreach (var item in items)
            {
                var itemModel = new UserDownloadableProductsModel.DownloadableProductsModel
                {
                    OrderItemGuid = item.OrderItemGuid,
                    OrderId = item.OrderId,
                    CustomOrderNumber = item.Order.CustomOrderNumber,
                    CreatedOn = _dateTimeHelper.ConvertToUserTime(item.Order.CreatedOnUtc, DateTimeKind.Utc),
                    ProductName = _localizationService.GetLocalized(item.Product, x => x.Name),
                    ProductSeName = _urlRecordService.GetSeName(item.Product),
                    ProductAttributes = item.AttributeDescription,
                    ProductId = item.ProductId
                };
                model.Items.Add(itemModel);

                if (_downloadService.IsDownloadAllowed(item))
                    itemModel.DownloadId = item.Product.DownloadId;

                if (_downloadService.IsLicenseDownloadAllowed(item))
                    itemModel.LicenseId = item.LicenseDownloadId.HasValue ? item.LicenseDownloadId.Value : 0;
            }

            return model;
        }

        /// <summary>
        /// Prepare the user agreement model
        /// </summary>
        /// <param name="orderItem">Order item</param>
        /// <param name="product">Product</param>
        /// <returns>User agreement model</returns>
        public virtual UserAgreementModel PrepareUserAgreementModel(OrderItem orderItem, Product product)
        {
            if (orderItem == null)
                throw new ArgumentNullException(nameof(orderItem));

            if (product == null)
                throw new ArgumentNullException(nameof(product));

            var model = new UserAgreementModel
            {
                UserAgreementText = product.UserAgreementText,
                OrderItemGuid = orderItem.OrderItemGuid
            };

            return model;
        }

        /// <summary>
        /// Prepare the change password model
        /// </summary>
        /// <returns>Change password model</returns>
        public virtual ChangePasswordModel PrepareChangePasswordModel()
        {
            var model = new ChangePasswordModel();
            return model;
        }

        /// <summary>
        /// Prepare the user avatar model
        /// </summary>
        /// <param name="model">User avatar model</param>
        /// <returns>User avatar model</returns>
        public virtual UserAvatarModel PrepareUserAvatarModel(UserAvatarModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            model.AvatarUrl = _pictureService.GetPictureUrl(
                _genericAttributeService.GetAttribute<int>(_workContext.CurrentUser, AppUserDefaults.AvatarPictureIdAttribute),
                _mediaSettings.AvatarPictureSize,
                false);

            return model;
        }

        /// <summary>
        /// Prepare the GDPR tools model
        /// </summary>
        /// <returns>GDPR tools model</returns>
        public virtual GdprToolsModel PrepareGdprToolsModel()
        {
            var model = new GdprToolsModel();
            return model;
        }

        /// <summary>
        /// Prepare the check gift card balance madel
        /// </summary>
        /// <returns>Check gift card balance madel</returns>
        public virtual CheckGiftCardBalanceModel PrepareCheckGiftCardBalanceModel()
        {
            var model = new CheckGiftCardBalanceModel();
            return model;
        }

        #endregion
    }
}