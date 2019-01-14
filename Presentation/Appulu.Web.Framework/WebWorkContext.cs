using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Appulu.Core;
using Appulu.Core.Domain.Users;
using Appulu.Core.Domain.Directory;
using Appulu.Core.Domain.Localization;
using Appulu.Core.Domain.Tax;
using Appulu.Core.Domain.Vendors;
using Appulu.Core.Http;
using Appulu.Services.Authentication;
using Appulu.Services.Common;
using Appulu.Services.Users;
using Appulu.Services.Directory;
using Appulu.Services.Helpers;
using Appulu.Services.Localization;
using Appulu.Services.Stores;
using Appulu.Services.Tasks;
using Appulu.Services.Vendors;
using Appulu.Web.Framework.Localization;

namespace Appulu.Web.Framework
{
    /// <summary>
    /// Represents work context for web application
    /// </summary>
    public partial class WebWorkContext : IWorkContext
    {
        #region Fields

        private readonly CurrencySettings _currencySettings;
        private readonly IAuthenticationService _authenticationService;
        private readonly ICurrencyService _currencyService;
        private readonly IUserService _userService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILanguageService _languageService;
        private readonly IStoreContext _storeContext;
        private readonly IStoreMappingService _storeMappingService;
        private readonly IUserAgentHelper _userAgentHelper;
        private readonly IVendorService _vendorService;
        private readonly LocalizationSettings _localizationSettings;
        private readonly TaxSettings _taxSettings;

        private User _cachedUser;
        private User _originalUserIfImpersonated;
        private Vendor _cachedVendor;
        private Language _cachedLanguage;
        private Currency _cachedCurrency;
        private TaxDisplayType? _cachedTaxDisplayType;

        #endregion

        #region Ctor

        public WebWorkContext(CurrencySettings currencySettings,
            IAuthenticationService authenticationService,
            ICurrencyService currencyService,
            IUserService userService,
            IGenericAttributeService genericAttributeService,
            IHttpContextAccessor httpContextAccessor,
            ILanguageService languageService,
            IStoreContext storeContext,
            IStoreMappingService storeMappingService,
            IUserAgentHelper userAgentHelper,
            IVendorService vendorService,
            LocalizationSettings localizationSettings,
            TaxSettings taxSettings)
        {
            this._currencySettings = currencySettings;
            this._authenticationService = authenticationService;
            this._currencyService = currencyService;
            this._userService = userService;
            this._genericAttributeService = genericAttributeService;
            this._httpContextAccessor = httpContextAccessor;
            this._languageService = languageService;
            this._storeContext = storeContext;
            this._storeMappingService = storeMappingService;
            this._userAgentHelper = userAgentHelper;
            this._vendorService = vendorService;
            this._localizationSettings = localizationSettings;
            this._taxSettings = taxSettings;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Get app user cookie
        /// </summary>
        /// <returns>String value of cookie</returns>
        protected virtual string GetUserCookie()
        {
            var cookieName = $"{AppCookieDefaults.Prefix}{AppCookieDefaults.UserCookie}";
            return _httpContextAccessor.HttpContext?.Request?.Cookies[cookieName];
        }

        /// <summary>
        /// Set app user cookie
        /// </summary>
        /// <param name="userGuid">Guid of the user</param>
        protected virtual void SetUserCookie(Guid userGuid)
        {
            if (_httpContextAccessor.HttpContext?.Response == null)
                return;

            //delete current cookie value
            var cookieName = $"{AppCookieDefaults.Prefix}{AppCookieDefaults.UserCookie}";
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(cookieName);

            //get date of cookie expiration
            var cookieExpires = 24 * 365; //TODO make configurable
            var cookieExpiresDate = DateTime.Now.AddHours(cookieExpires);

            //if passed guid is empty set cookie as expired
            if (userGuid == Guid.Empty)
                cookieExpiresDate = DateTime.Now.AddMonths(-1);

            //set new cookie value
            var options = new CookieOptions
            {
                HttpOnly = true,
                Expires = cookieExpiresDate
            };
            _httpContextAccessor.HttpContext.Response.Cookies.Append(cookieName, userGuid.ToString(), options);
        }

        /// <summary>
        /// Get language from the requested page URL
        /// </summary>
        /// <returns>The found language</returns>
        protected virtual Language GetLanguageFromUrl()
        {
            if (_httpContextAccessor.HttpContext?.Request == null)
                return null;

            //whether the requsted URL is localized
            var path = _httpContextAccessor.HttpContext.Request.Path.Value;
            if (!path.IsLocalizedUrl(_httpContextAccessor.HttpContext.Request.PathBase, false, out Language language))
                return null;

            //check language availability
            if (!_storeMappingService.Authorize(language))
                return null;

            return language;
        }

        /// <summary>
        /// Get language from the request
        /// </summary>
        /// <returns>The found language</returns>
        protected virtual Language GetLanguageFromRequest()
        {
            if (_httpContextAccessor.HttpContext?.Request == null)
                return null;

            //get request culture
            var requestCulture = _httpContextAccessor.HttpContext.Features.Get<IRequestCultureFeature>()?.RequestCulture;
            if (requestCulture == null)
                return null;

            //try to get language by culture name
            var requestLanguage = _languageService.GetAllLanguages().FirstOrDefault(language =>
                language.LanguageCulture.Equals(requestCulture.Culture.Name, StringComparison.InvariantCultureIgnoreCase));

            //check language availability
            if (requestLanguage == null || !requestLanguage.Published || !_storeMappingService.Authorize(requestLanguage))
                return null;

            return requestLanguage;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the current user
        /// </summary>
        public virtual User CurrentUser
        {
            get
            {
                //whether there is a cached value
                if (_cachedUser != null)
                    return _cachedUser;

                User user = null;

                //check whether request is made by a background (schedule) task
                if (_httpContextAccessor.HttpContext == null ||
                    _httpContextAccessor.HttpContext.Request.Path.Equals(new PathString($"/{AppTaskDefaults.ScheduleTaskPath}"), StringComparison.InvariantCultureIgnoreCase))
                {
                    //in this case return built-in user record for background task
                    user = _userService.GetUserBySystemName(AppUserDefaults.BackgroundTaskUserName);
                }

                if (user == null || user.Deleted || !user.Active || user.RequireReLogin)
                {
                    //check whether request is made by a search engine, in this case return built-in user record for search engines
                    if (_userAgentHelper.IsSearchEngine())
                        user = _userService.GetUserBySystemName(AppUserDefaults.SearchEngineUserName);
                }

                if (user == null || user.Deleted || !user.Active || user.RequireReLogin)
                {
                    //try to get registered user
                    user = _authenticationService.GetAuthenticatedUser();
                }

                if (user != null && !user.Deleted && user.Active && !user.RequireReLogin)
                {
                    //get impersonate user if required
                    var impersonatedUserId = _genericAttributeService
                        .GetAttribute<int?>(user, AppUserDefaults.ImpersonatedUserIdAttribute);
                    if (impersonatedUserId.HasValue && impersonatedUserId.Value > 0)
                    {
                        var impersonatedUser = _userService.GetUserById(impersonatedUserId.Value);
                        if (impersonatedUser != null && !impersonatedUser.Deleted && impersonatedUser.Active && !impersonatedUser.RequireReLogin)
                        {
                            //set impersonated user
                            _originalUserIfImpersonated = user;
                            user = impersonatedUser;
                        }
                    }
                }

                if (user == null || user.Deleted || !user.Active || user.RequireReLogin)
                {
                    //get guest user
                    var userCookie = GetUserCookie();
                    if (!string.IsNullOrEmpty(userCookie))
                    {
                        if (Guid.TryParse(userCookie, out Guid userGuid))
                        {
                            //get user from cookie (should not be registered)
                            var userByCookie = _userService.GetUserByGuid(userGuid);
                            if (userByCookie != null && !userByCookie.IsRegistered())
                                user = userByCookie;
                        }
                    }
                }

                if (user == null || user.Deleted || !user.Active || user.RequireReLogin)
                {
                    //create guest if not exists
                    user = _userService.InsertGuestUser();
                }

                if (!user.Deleted && user.Active && !user.RequireReLogin)
                {
                    //set user cookie
                    SetUserCookie(user.UserGuid);

                    //cache the found user
                    _cachedUser = user;
                }

                return _cachedUser;
            }
            set
            {
                SetUserCookie(value.UserGuid);
                _cachedUser = value;
            }
        }

        /// <summary>
        /// Gets the original user (in case the current one is impersonated)
        /// </summary>
        public virtual User OriginalUserIfImpersonated
        {
            get { return _originalUserIfImpersonated; }
        }

        /// <summary>
        /// Gets the current vendor (logged-in manager)
        /// </summary>
        public virtual Vendor CurrentVendor
        {
            get
            {
                //whether there is a cached value
                if (_cachedVendor != null)
                    return _cachedVendor;

                if (this.CurrentUser == null)
                    return null;

                //try to get vendor
                var vendor = _vendorService.GetVendorById(this.CurrentUser.VendorId);

                //check vendor availability
                if (vendor == null || vendor.Deleted || !vendor.Active)
                    return null;

                //cache the found vendor
                _cachedVendor = vendor;

                return _cachedVendor;
            }
        }

        /// <summary>
        /// Gets or sets current user working language
        /// </summary>
        public virtual Language WorkingLanguage
        {
            get
            {
                //whether there is a cached value
                if (_cachedLanguage != null)
                    return _cachedLanguage;

                Language detectedLanguage = null;

                //localized URLs are enabled, so try to get language from the requested page URL
                if (_localizationSettings.SeoFriendlyUrlsForLanguagesEnabled)
                    detectedLanguage = GetLanguageFromUrl();

                //whether we should detect the language from the request
                if (detectedLanguage == null && _localizationSettings.AutomaticallyDetectLanguage)
                {
                    //whether language already detected by this way
                    var alreadyDetected = _genericAttributeService.GetAttribute<bool>(this.CurrentUser,
                        AppUserDefaults.LanguageAutomaticallyDetectedAttribute, _storeContext.CurrentStore.Id);

                    //if not, try to get language from the request
                    if (!alreadyDetected)
                    {
                        detectedLanguage = GetLanguageFromRequest();
                        if (detectedLanguage != null)
                        {
                            //language already detected
                            _genericAttributeService.SaveAttribute(this.CurrentUser,
                                AppUserDefaults.LanguageAutomaticallyDetectedAttribute, true, _storeContext.CurrentStore.Id);
                        }
                    }
                }

                //if the language is detected we need to save it
                if (detectedLanguage != null)
                {
                    //get current saved language identifier
                    var currentLanguageId = _genericAttributeService.GetAttribute<int>(this.CurrentUser,
                        AppUserDefaults.LanguageIdAttribute, _storeContext.CurrentStore.Id);

                    //save the detected language identifier if it differs from the current one
                    if (detectedLanguage.Id != currentLanguageId)
                    {
                        _genericAttributeService.SaveAttribute(this.CurrentUser,
                            AppUserDefaults.LanguageIdAttribute, detectedLanguage.Id, _storeContext.CurrentStore.Id);
                    }
                }

                //get current user language identifier
                var userLanguageId = _genericAttributeService.GetAttribute<int>(this.CurrentUser,
                    AppUserDefaults.LanguageIdAttribute, _storeContext.CurrentStore.Id);

                var allStoreLanguages = _languageService.GetAllLanguages(storeId: _storeContext.CurrentStore.Id);

                //check user language availability
                var userLanguage = allStoreLanguages.FirstOrDefault(language => language.Id == userLanguageId);
                if (userLanguage == null)
                {
                    //it not found, then try to get the default language for the current store (if specified)
                    userLanguage = allStoreLanguages.FirstOrDefault(language => language.Id == _storeContext.CurrentStore.DefaultLanguageId);
                }

                //if the default language for the current store not found, then try to get the first one
                if (userLanguage == null)
                    userLanguage = allStoreLanguages.FirstOrDefault();

                //if there are no languages for the current store try to get the first one regardless of the store
                if (userLanguage == null)
                    userLanguage = _languageService.GetAllLanguages().FirstOrDefault();

                //cache the found language
                _cachedLanguage = userLanguage;

                return _cachedLanguage;
            }
            set
            {
                //get passed language identifier
                var languageId = value?.Id ?? 0;

                //and save it
                _genericAttributeService.SaveAttribute(this.CurrentUser,
                    AppUserDefaults.LanguageIdAttribute, languageId, _storeContext.CurrentStore.Id);

                //then reset the cached value
                _cachedLanguage = null;
            }
        }

        /// <summary>
        /// Gets or sets current user working currency
        /// </summary>
        public virtual Currency WorkingCurrency
        {
            get
            {
                //whether there is a cached value
                if (_cachedCurrency != null)
                    return _cachedCurrency;

                //return primary store currency when we're in admin area/mode
                if (this.IsAdmin)
                {
                    var primaryStoreCurrency = _currencyService.GetCurrencyById(_currencySettings.PrimaryStoreCurrencyId);
                    if (primaryStoreCurrency != null)
                    {
                        _cachedCurrency = primaryStoreCurrency;
                        return primaryStoreCurrency;
                    }
                }

                //find a currency previously selected by a user
                var userCurrencyId = _genericAttributeService.GetAttribute<int>(this.CurrentUser,
                    AppUserDefaults.CurrencyIdAttribute, _storeContext.CurrentStore.Id);

                var allStoreCurrencies = _currencyService.GetAllCurrencies(storeId: _storeContext.CurrentStore.Id);

                //check user currency availability
                var userCurrency = allStoreCurrencies.FirstOrDefault(currency => currency.Id == userCurrencyId);
                if (userCurrency == null)
                {
                    //it not found, then try to get the default currency for the current language (if specified)
                    userCurrency = allStoreCurrencies.FirstOrDefault(currency => currency.Id == this.WorkingLanguage.DefaultCurrencyId);
                }

                //if the default currency for the current store not found, then try to get the first one
                if (userCurrency == null)
                    userCurrency = allStoreCurrencies.FirstOrDefault();

                //if there are no currencies for the current store try to get the first one regardless of the store
                if (userCurrency == null)
                    userCurrency = _currencyService.GetAllCurrencies().FirstOrDefault();

                //cache the found currency
                _cachedCurrency = userCurrency;

                return _cachedCurrency;
            }
            set
            {
                //get passed currency identifier
                var currencyId = value?.Id ?? 0;

                //and save it
                _genericAttributeService.SaveAttribute(this.CurrentUser,
                    AppUserDefaults.CurrencyIdAttribute, currencyId, _storeContext.CurrentStore.Id);

                //then reset the cached value
                _cachedCurrency = null;
            }
        }

        /// <summary>
        /// Gets or sets current tax display type
        /// </summary>
        public virtual TaxDisplayType TaxDisplayType
        {
            get
            {
                //whether there is a cached value
                if (_cachedTaxDisplayType.HasValue)
                    return _cachedTaxDisplayType.Value;

                var taxDisplayType = TaxDisplayType.IncludingTax;

                //whether users are allowed to select tax display type
                if (_taxSettings.AllowUsersToSelectTaxDisplayType && this.CurrentUser != null)
                {
                    //try to get previously saved tax display type
                    var taxDisplayTypeId = _genericAttributeService.GetAttribute<int?>(this.CurrentUser,
                        AppUserDefaults.TaxDisplayTypeIdAttribute, _storeContext.CurrentStore.Id);
                    if (taxDisplayTypeId.HasValue)
                    {
                        taxDisplayType = (TaxDisplayType)taxDisplayTypeId.Value;
                    }
                    else
                    {
                        //default tax type by user roles
                        var defaultRoleTaxDisplayType = _userService.GetUserDefaultTaxDisplayType(this.CurrentUser);
                        if (defaultRoleTaxDisplayType != null)
                        {
                            taxDisplayType = defaultRoleTaxDisplayType.Value;
                        }
                    }
                }
                else
                {
                    //default tax type by user roles
                    var defaultRoleTaxDisplayType = _userService.GetUserDefaultTaxDisplayType(this.CurrentUser);
                    if (defaultRoleTaxDisplayType != null)
                    {
                        taxDisplayType = defaultRoleTaxDisplayType.Value;
                    }
                    else
                    {
                        //or get the default tax display type
                        taxDisplayType = _taxSettings.TaxDisplayType;
                    }
                }

                //cache the value
                _cachedTaxDisplayType = taxDisplayType;

                return _cachedTaxDisplayType.Value;

            }
            set
            {
                //whether users are allowed to select tax display type
                if (!_taxSettings.AllowUsersToSelectTaxDisplayType)
                    return;

                //save passed value
                _genericAttributeService.SaveAttribute(this.CurrentUser,
                    AppUserDefaults.TaxDisplayTypeIdAttribute, (int)value, _storeContext.CurrentStore.Id);

                //then reset the cached value
                _cachedTaxDisplayType = null;
            }
        }

        /// <summary>
        /// Gets or sets value indicating whether we're in admin area
        /// </summary>
        public virtual bool IsAdmin { get; set; }

        #endregion
    }
}