using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using Appulu.Core;
using Appulu.Core.Domain.Catalog;
using Appulu.Core.Domain.Common;
using Appulu.Core.Domain.Users;
using Appulu.Core.Domain.Gdpr;
using Appulu.Core.Domain.Media;
using Appulu.Core.Domain.Orders;
using Appulu.Core.Domain.Tax;
using Appulu.Services.Affiliates;
using Appulu.Services.Authentication.External;
using Appulu.Services.Catalog;
using Appulu.Services.Common;
using Appulu.Services.Users;
using Appulu.Services.Directory;
using Appulu.Services.Gdpr;
using Appulu.Services.Helpers;
using Appulu.Services.Localization;
using Appulu.Services.Logging;
using Appulu.Services.Media;
using Appulu.Services.Messages;
using Appulu.Services.Orders;
using Appulu.Services.Stores;
using Appulu.Services.Tax;
using Appulu.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Appulu.Web.Areas.Admin.Models.Common;
using Appulu.Web.Areas.Admin.Models.Users;
using Appulu.Web.Areas.Admin.Models.ShoppingCart;
using Appulu.Web.Framework.Extensions;
using Appulu.Web.Framework.Factories;

namespace Appulu.Web.Areas.Admin.Factories
{
    /// <summary>
    /// Represents the user model factory implementation
    /// </summary>
    public partial class UserModelFactory : IUserModelFactory
    {
        #region Fields

        private readonly AddressSettings _addressSettings;
        private readonly UserSettings _userSettings;
        private readonly DateTimeSettings _dateTimeSettings;
        private readonly GdprSettings _gdprSettings;
        private readonly IAclSupportedModelFactory _aclSupportedModelFactory;
        private readonly IAddressAttributeFormatter _addressAttributeFormatter;
        private readonly IAddressAttributeModelFactory _addressAttributeModelFactory;
        private readonly IAffiliateService _affiliateService;
        private readonly IBackInStockSubscriptionService _backInStockSubscriptionService;
        private readonly IBaseAdminModelFactory _baseAdminModelFactory;
        private readonly IUserActivityService _userActivityService;
        private readonly IUserAttributeParser _userAttributeParser;
        private readonly IUserAttributeService _userAttributeService;
        private readonly IUserService _userService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IExternalAuthenticationService _externalAuthenticationService;
        private readonly IGdprService _gdprService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IGeoLookupService _geoLookupService;
        private readonly ILocalizationService _localizationService;
        private readonly INewsLetterSubscriptionService _newsLetterSubscriptionService;
        private readonly IOrderService _orderService;
        private readonly IPictureService _pictureService;
        private readonly IPriceCalculationService _priceCalculationService;
        private readonly IPriceFormatter _priceFormatter;
        private readonly IProductAttributeFormatter _productAttributeFormatter;
        private readonly IRewardPointService _rewardPointService;
        private readonly IStoreContext _storeContext;
        private readonly IStoreService _storeService;
        private readonly ITaxService _taxService;
        private readonly MediaSettings _mediaSettings;
        private readonly RewardPointsSettings _rewardPointsSettings;
        private readonly TaxSettings _taxSettings;

        #endregion

        #region Ctor

        public UserModelFactory(AddressSettings addressSettings,
            UserSettings userSettings,
            DateTimeSettings dateTimeSettings,
            GdprSettings gdprSettings,
            IAclSupportedModelFactory aclSupportedModelFactory,
            IAddressAttributeFormatter addressAttributeFormatter,
            IAddressAttributeModelFactory addressAttributeModelFactory,
            IAffiliateService affiliateService,
            IBackInStockSubscriptionService backInStockSubscriptionService,
            IBaseAdminModelFactory baseAdminModelFactory,
            IUserActivityService userActivityService,
            IUserAttributeParser userAttributeParser,
            IUserAttributeService userAttributeService,
            IUserService userService,
            IDateTimeHelper dateTimeHelper,
            IExternalAuthenticationService externalAuthenticationService,
            IGdprService gdprService,
            IGenericAttributeService genericAttributeService,
            IGeoLookupService geoLookupService,
            ILocalizationService localizationService,
            INewsLetterSubscriptionService newsLetterSubscriptionService,
            IOrderService orderService,
            IPictureService pictureService,
            IPriceCalculationService priceCalculationService,
            IPriceFormatter priceFormatter,
            IProductAttributeFormatter productAttributeFormatter,
            IRewardPointService rewardPointService,
            IStoreContext storeContext,
            IStoreService storeService,
            ITaxService taxService,
            MediaSettings mediaSettings,
            RewardPointsSettings rewardPointsSettings,
            TaxSettings taxSettings)
        {
            this._addressSettings = addressSettings;
            this._userSettings = userSettings;
            this._dateTimeSettings = dateTimeSettings;
            this._gdprSettings = gdprSettings;
            this._aclSupportedModelFactory = aclSupportedModelFactory;
            this._addressAttributeFormatter = addressAttributeFormatter;
            this._addressAttributeModelFactory = addressAttributeModelFactory;
            this._affiliateService = affiliateService;
            this._backInStockSubscriptionService = backInStockSubscriptionService;
            this._baseAdminModelFactory = baseAdminModelFactory;
            this._userActivityService = userActivityService;
            this._userAttributeParser = userAttributeParser;
            this._userAttributeService = userAttributeService;
            this._userService = userService;
            this._dateTimeHelper = dateTimeHelper;
            this._externalAuthenticationService = externalAuthenticationService;
            this._gdprService = gdprService;
            this._genericAttributeService = genericAttributeService;
            this._geoLookupService = geoLookupService;
            this._localizationService = localizationService;
            this._newsLetterSubscriptionService = newsLetterSubscriptionService;
            this._orderService = orderService;
            this._pictureService = pictureService;
            this._priceCalculationService = priceCalculationService;
            this._priceFormatter = priceFormatter;
            this._productAttributeFormatter = productAttributeFormatter;
            this._rewardPointService = rewardPointService;
            this._storeContext = storeContext;
            this._storeService = storeService;
            this._taxService = taxService;
            this._mediaSettings = mediaSettings;
            this._rewardPointsSettings = rewardPointsSettings;
            this._taxSettings = taxSettings;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Prepare the reward points model to add to the user
        /// </summary>
        /// <param name="model">Reward points model to add to the user</param>
        protected virtual void PrepareAddRewardPointsToUserModel(AddRewardPointsToUserModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            model.Message = _localizationService.GetResource("Admin.Users.Users.SomeComment");
            model.ActivatePointsImmediately = true;
            model.StoreId = _storeContext.CurrentStore.Id;

            //prepare available stores
            _baseAdminModelFactory.PrepareStores(model.AvailableStores, false);
        }

        /// <summary>
        /// Prepare user associated external authorization models
        /// </summary>
        /// <param name="models">List of user associated external authorization models</param>
        /// <param name="user">User</param>
        protected virtual void PrepareAssociatedExternalAuthModels(IList<UserAssociatedExternalAuthModel> models, User user)
        {
            if (models == null)
                throw new ArgumentNullException(nameof(models));

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            foreach (var record in user.ExternalAuthenticationRecords)
            {
                var method = _externalAuthenticationService.LoadExternalAuthenticationMethodBySystemName(record.ProviderSystemName);
                if (method == null)
                    continue;

                models.Add(new UserAssociatedExternalAuthModel
                {
                    Id = record.Id,
                    Email = record.Email,
                    ExternalIdentifier = !string.IsNullOrEmpty(record.ExternalDisplayIdentifier)
                        ? record.ExternalDisplayIdentifier : record.ExternalIdentifier,
                    AuthMethodName = method.PluginDescriptor.FriendlyName
                });
            }
        }

        /// <summary>
        /// Prepare user attribute models
        /// </summary>
        /// <param name="models">List of user attribute models</param>
        /// <param name="user">User</param>
        protected virtual void PrepareUserAttributeModels(IList<UserModel.UserAttributeModel> models, User user)
        {
            if (models == null)
                throw new ArgumentNullException(nameof(models));

            //get available user attributes
            var userAttributes = _userAttributeService.GetAllUserAttributes();
            foreach (var attribute in userAttributes)
            {
                var attributeModel = new UserModel.UserAttributeModel
                {
                    Id = attribute.Id,
                    Name = attribute.Name,
                    IsRequired = attribute.IsRequired,
                    AttributeControlType = attribute.AttributeControlType
                };

                if (attribute.ShouldHaveValues())
                {
                    //values
                    var attributeValues = _userAttributeService.GetUserAttributeValues(attribute.Id);
                    foreach (var attributeValue in attributeValues)
                    {
                        var attributeValueModel = new UserModel.UserAttributeValueModel
                        {
                            Id = attributeValue.Id,
                            Name = attributeValue.Name,
                            IsPreSelected = attributeValue.IsPreSelected
                        };
                        attributeModel.Values.Add(attributeValueModel);
                    }
                }

                //set already selected attributes
                if (user != null)
                {
                    var selectedUserAttributes = _genericAttributeService
                        .GetAttribute<string>(user, AppUserDefaults.CustomUserAttributes);
                    switch (attribute.AttributeControlType)
                    {
                        case AttributeControlType.DropdownList:
                        case AttributeControlType.RadioList:
                        case AttributeControlType.Checkboxes:
                            {
                                if (!string.IsNullOrEmpty(selectedUserAttributes))
                                {
                                    //clear default selection
                                    foreach (var item in attributeModel.Values)
                                        item.IsPreSelected = false;

                                    //select new values
                                    var selectedValues = _userAttributeParser.ParseUserAttributeValues(selectedUserAttributes);
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
                                if (!string.IsNullOrEmpty(selectedUserAttributes))
                                {
                                    var enteredText = _userAttributeParser.ParseValues(selectedUserAttributes, attribute.Id);
                                    if (enteredText.Any())
                                        attributeModel.DefaultValue = enteredText[0];
                                }
                            }
                            break;
                        case AttributeControlType.Datepicker:
                        case AttributeControlType.ColorSquares:
                        case AttributeControlType.ImageSquares:
                        case AttributeControlType.FileUpload:
                        default:
                            //not supported attribute control types
                            break;
                    }
                }

                models.Add(attributeModel);
            }
        }

        /// <summary>
        /// Prepare address model
        /// </summary>
        /// <param name="model">Address model</param>
        /// <param name="address">Address</param>
        protected virtual void PrepareAddressModel(AddressModel model, Address address)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            //set some of address fields as enabled and required
            model.FirstNameEnabled = true;
            model.FirstNameRequired = true;
            model.LastNameEnabled = true;
            model.LastNameRequired = true;
            model.EmailEnabled = true;
            model.EmailRequired = true;
            model.CompanyEnabled = _addressSettings.CompanyEnabled;
            model.CompanyRequired = _addressSettings.CompanyRequired;
            model.CountryEnabled = _addressSettings.CountryEnabled;
            model.CountryRequired = _addressSettings.CountryEnabled; //country is required when enabled
            model.StateProvinceEnabled = _addressSettings.StateProvinceEnabled;
            model.CityEnabled = _addressSettings.CityEnabled;
            model.CityRequired = _addressSettings.CityRequired;
            model.CountyEnabled = _addressSettings.CountyEnabled;
            model.CountyRequired = _addressSettings.CountyRequired;
            model.StreetAddressEnabled = _addressSettings.StreetAddressEnabled;
            model.StreetAddressRequired = _addressSettings.StreetAddressRequired;
            model.StreetAddress2Enabled = _addressSettings.StreetAddress2Enabled;
            model.StreetAddress2Required = _addressSettings.StreetAddress2Required;
            model.ZipPostalCodeEnabled = _addressSettings.ZipPostalCodeEnabled;
            model.ZipPostalCodeRequired = _addressSettings.ZipPostalCodeRequired;
            model.PhoneEnabled = _addressSettings.PhoneEnabled;
            model.PhoneRequired = _addressSettings.PhoneRequired;
            model.FaxEnabled = _addressSettings.FaxEnabled;
            model.FaxRequired = _addressSettings.FaxRequired;

            //prepare available countries
            _baseAdminModelFactory.PrepareCountries(model.AvailableCountries);

            //prepare available states
            _baseAdminModelFactory.PrepareStatesAndProvinces(model.AvailableStates, model.CountryId);

            //prepare custom address attributes
            _addressAttributeModelFactory.PrepareCustomAddressAttributes(model.CustomAddressAttributes, address);
        }

        /// <summary>
        /// Prepare HTML string address
        /// </summary>
        /// <param name="model">Address model</param>
        /// <param name="address">Address</param>
        protected virtual void PrepareModelAddressHtml(AddressModel model, Address address)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var addressHtmlSb = new StringBuilder("<div>");

            if (_addressSettings.CompanyEnabled && !string.IsNullOrEmpty(model.Company))
                addressHtmlSb.AppendFormat("{0}<br />", WebUtility.HtmlEncode(model.Company));

            if (_addressSettings.StreetAddressEnabled && !string.IsNullOrEmpty(model.Address1))
                addressHtmlSb.AppendFormat("{0}<br />", WebUtility.HtmlEncode(model.Address1));

            if (_addressSettings.StreetAddress2Enabled && !string.IsNullOrEmpty(model.Address2))
                addressHtmlSb.AppendFormat("{0}<br />", WebUtility.HtmlEncode(model.Address2));

            if (_addressSettings.CityEnabled && !string.IsNullOrEmpty(model.City))
                addressHtmlSb.AppendFormat("{0},", WebUtility.HtmlEncode(model.City));

            if (_addressSettings.CountyEnabled && !string.IsNullOrEmpty(model.County))
                addressHtmlSb.AppendFormat("{0},", WebUtility.HtmlEncode(model.County));

            if (_addressSettings.StateProvinceEnabled && !string.IsNullOrEmpty(model.StateProvinceName))
                addressHtmlSb.AppendFormat("{0},", WebUtility.HtmlEncode(model.StateProvinceName));

            if (_addressSettings.ZipPostalCodeEnabled && !string.IsNullOrEmpty(model.ZipPostalCode))
                addressHtmlSb.AppendFormat("{0}<br />", WebUtility.HtmlEncode(model.ZipPostalCode));

            if (_addressSettings.CountryEnabled && !string.IsNullOrEmpty(model.CountryName))
                addressHtmlSb.AppendFormat("{0}", WebUtility.HtmlEncode(model.CountryName));

            var customAttributesFormatted = _addressAttributeFormatter.FormatAttributes(address?.CustomAttributes);
            if (!string.IsNullOrEmpty(customAttributesFormatted))
            {
                //already encoded
                addressHtmlSb.AppendFormat("<br />{0}", customAttributesFormatted);
            }

            addressHtmlSb.Append("</div>");

            model.AddressHtml = addressHtmlSb.ToString();
        }

        /// <summary>
        /// Prepare reward points search model
        /// </summary>
        /// <param name="searchModel">Reward points search model</param>
        /// <param name="user">User</param>
        /// <returns>Reward points search model</returns>
        protected virtual UserRewardPointsSearchModel PrepareRewardPointsSearchModel(UserRewardPointsSearchModel searchModel, User user)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            searchModel.UserId = user.Id;

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        /// <summary>
        /// Prepare user address search model
        /// </summary>
        /// <param name="searchModel">User address search model</param>
        /// <param name="user">User</param>
        /// <returns>User address search model</returns>
        protected virtual UserAddressSearchModel PrepareUserAddressSearchModel(UserAddressSearchModel searchModel, User user)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            searchModel.UserId = user.Id;

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        /// <summary>
        /// Prepare user order search model
        /// </summary>
        /// <param name="searchModel">User order search model</param>
        /// <param name="user">User</param>
        /// <returns>User order search model</returns>
        protected virtual UserOrderSearchModel PrepareUserOrderSearchModel(UserOrderSearchModel searchModel, User user)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            searchModel.UserId = user.Id;

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        /// <summary>
        /// Prepare user shopping cart search model
        /// </summary>
        /// <param name="searchModel">User shopping cart search model</param>
        /// <param name="user">User</param>
        /// <returns>User shopping cart search model</returns>
        protected virtual UserShoppingCartSearchModel PrepareUserShoppingCartSearchModel(UserShoppingCartSearchModel searchModel,
            User user)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            searchModel.UserId = user.Id;

            //prepare available shopping cart types (search shopping cart by default)
            searchModel.ShoppingCartTypeId = (int)ShoppingCartType.ShoppingCart;
            _baseAdminModelFactory.PrepareShoppingCartTypes(searchModel.AvailableShoppingCartTypes, false);

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        /// <summary>
        /// Prepare user activity log search model
        /// </summary>
        /// <param name="searchModel">User activity log search model</param>
        /// <param name="user">User</param>
        /// <returns>User activity log search model</returns>
        protected virtual UserActivityLogSearchModel PrepareUserActivityLogSearchModel(UserActivityLogSearchModel searchModel, User user)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            searchModel.UserId = user.Id;

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        /// <summary>
        /// Prepare user back in stock subscriptions search model
        /// </summary>
        /// <param name="searchModel">User back in stock subscriptions search model</param>
        /// <param name="user">User</param>
        /// <returns>User back in stock subscriptions search model</returns>
        protected virtual UserBackInStockSubscriptionSearchModel PrepareUserBackInStockSubscriptionSearchModel(
            UserBackInStockSubscriptionSearchModel searchModel, User user)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            searchModel.UserId = user.Id;

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare user search model
        /// </summary>
        /// <param name="searchModel">User search model</param>
        /// <returns>User search model</returns>
        public virtual UserSearchModel PrepareUserSearchModel(UserSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            searchModel.UsernamesEnabled = _userSettings.UsernamesEnabled;
            searchModel.AvatarEnabled = _userSettings.AllowUsersToUploadAvatars;
            searchModel.DateOfBirthEnabled = _userSettings.DateOfBirthEnabled;
            searchModel.CompanyEnabled = _userSettings.CompanyEnabled;
            searchModel.PhoneEnabled = _userSettings.PhoneEnabled;
            searchModel.ZipPostalCodeEnabled = _userSettings.ZipPostalCodeEnabled;

            //search registered users by default
            var registeredRole = _userService.GetUserRoleBySystemName(AppUserDefaults.RegisteredRoleName);
            if (registeredRole != null)
                searchModel.SelectedUserRoleIds.Add(registeredRole.Id);

            //prepare available user roles
            _aclSupportedModelFactory.PrepareModelUserRoles(searchModel);

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        /// <summary>
        /// Prepare paged user list model
        /// </summary>
        /// <param name="searchModel">User search model</param>
        /// <returns>User list model</returns>
        public virtual UserListModel PrepareUserListModel(UserSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get parameters to filter users
            int.TryParse(searchModel.SearchDayOfBirth, out var dayOfBirth);
            int.TryParse(searchModel.SearchMonthOfBirth, out var monthOfBirth);

            //get users
            var users = _userService.GetAllUsers(loadOnlyWithShoppingCart: false,
                userRoleIds: searchModel.SelectedUserRoleIds.ToArray(),
                email: searchModel.SearchEmail,
                username: searchModel.SearchUsername,
                firstName: searchModel.SearchFirstName,
                lastName: searchModel.SearchLastName,
                dayOfBirth: dayOfBirth,
                monthOfBirth: monthOfBirth,
                company: searchModel.SearchCompany,
                phone: searchModel.SearchPhone,
                zipPostalCode: searchModel.SearchZipPostalCode,
                ipAddress: searchModel.SearchIpAddress,
                pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize);

            //prepare list model
            var model = new UserListModel
            {
                Data = users.Select(user =>
                {
                    //fill in model values from the entity
                    var userModel = new UserModel
                    {
                        Id = user.Id,
                        Email = user.IsRegistered() ? user.Email : _localizationService.GetResource("Admin.Users.Guest"),
                        Username = user.Username,
                        FullName = _userService.GetUserFullName(user),
                        Company = _genericAttributeService.GetAttribute<string>(user, AppUserDefaults.CompanyAttribute),
                        Phone = _genericAttributeService.GetAttribute<string>(user, AppUserDefaults.PhoneAttribute),
                        ZipPostalCode = _genericAttributeService.GetAttribute<string>(user, AppUserDefaults.ZipPostalCodeAttribute),
                        Active = user.Active
                    };

                    //convert dates to the user time
                    userModel.CreatedOn = _dateTimeHelper.ConvertToUserTime(user.CreatedOnUtc, DateTimeKind.Utc);
                    userModel.LastActivityDate = _dateTimeHelper.ConvertToUserTime(user.LastActivityDateUtc, DateTimeKind.Utc);

                    //fill in additional values (not existing in the entity)
                    userModel.UserRoleNames = string.Join(", ", user.UserRoles.Select(role => role.Name));
                    if (_userSettings.AllowUsersToUploadAvatars)
                    {
                        var avatarPictureId = _genericAttributeService.GetAttribute<int>(user, AppUserDefaults.AvatarPictureIdAttribute);
                        userModel.AvatarUrl = _pictureService.GetPictureUrl(avatarPictureId, _mediaSettings.AvatarPictureSize,
                            _userSettings.DefaultAvatarEnabled, defaultPictureType: PictureType.Avatar);
                    }

                    return userModel;
                }),
                Total = users.TotalCount
            };

            return model;
        }

        /// <summary>
        /// Prepare user model
        /// </summary>
        /// <param name="model">User model</param>
        /// <param name="user">User</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>User model</returns>
        public virtual UserModel PrepareUserModel(UserModel model, User user, bool excludeProperties = false)
        {
            if (user != null)
            {
                //fill in model values from the entity
                model = model ?? new UserModel();

                model.Id = user.Id;
                model.DisplayVatNumber = _taxSettings.EuVatEnabled;
                model.AllowSendingOfWelcomeMessage = user.IsRegistered() &&
                    _userSettings.UserRegistrationType == UserRegistrationType.AdminApproval;
                model.AllowReSendingOfActivationMessage = user.IsRegistered() && !user.Active &&
                    _userSettings.UserRegistrationType == UserRegistrationType.EmailValidation;
                model.GdprEnabled = _gdprSettings.GdprEnabled;

                //whether to fill in some of properties
                if (!excludeProperties)
                {
                    model.Email = user.Email;
                    model.Username = user.Username;
                    model.VendorId = user.VendorId;
                    model.AdminComment = user.AdminComment;
                    model.IsTaxExempt = user.IsTaxExempt;
                    model.Active = user.Active;
                    model.FirstName = _genericAttributeService.GetAttribute<string>(user, AppUserDefaults.FirstNameAttribute);
                    model.LastName = _genericAttributeService.GetAttribute<string>(user, AppUserDefaults.LastNameAttribute);
                    model.Gender = _genericAttributeService.GetAttribute<string>(user, AppUserDefaults.GenderAttribute);
                    model.DateOfBirth = _genericAttributeService.GetAttribute<DateTime?>(user, AppUserDefaults.DateOfBirthAttribute);
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
                    model.TimeZoneId = _genericAttributeService.GetAttribute<string>(user, AppUserDefaults.TimeZoneIdAttribute);
                    model.VatNumber = _genericAttributeService.GetAttribute<string>(user, AppUserDefaults.VatNumberAttribute);
                    model.VatNumberStatusNote = _localizationService.GetLocalizedEnum((VatNumberStatus)_genericAttributeService
                        .GetAttribute<int>(user, AppUserDefaults.VatNumberStatusIdAttribute));
                    model.CreatedOn = _dateTimeHelper.ConvertToUserTime(user.CreatedOnUtc, DateTimeKind.Utc);
                    model.LastActivityDate = _dateTimeHelper.ConvertToUserTime(user.LastActivityDateUtc, DateTimeKind.Utc);
                    model.LastIpAddress = user.LastIpAddress;
                    model.LastVisitedPage = _genericAttributeService.GetAttribute<string>(user, AppUserDefaults.LastVisitedPageAttribute);
                    model.SelectedUserRoleIds = user.UserUserRoleMappings.Select(mapping => mapping.UserRoleId).ToList();
                    model.RegisteredInStore = _storeService.GetAllStores()
                        .FirstOrDefault(store => store.Id == user.RegisteredInStoreId)?.Name ?? string.Empty;

                    //prepare model affiliate
                    var affiliate = _affiliateService.GetAffiliateById(user.AffiliateId);
                    if (affiliate != null)
                    {
                        model.AffiliateId = affiliate.Id;
                        model.AffiliateName = _affiliateService.GetAffiliateFullName(affiliate);
                    }

                    //prepare model newsletter subscriptions
                    if (!string.IsNullOrEmpty(user.Email))
                    {
                        model.SelectedNewsletterSubscriptionStoreIds = _storeService.GetAllStores()
                            .Where(store => _newsLetterSubscriptionService.GetNewsLetterSubscriptionByEmailAndStoreId(user.Email, store.Id) != null)
                            .Select(store => store.Id).ToList();
                    }
                }
                //prepare reward points model
                model.DisplayRewardPointsHistory = _rewardPointsSettings.Enabled;
                if (model.DisplayRewardPointsHistory)
                    PrepareAddRewardPointsToUserModel(model.AddRewardPoints);

                //prepare external authentication records
                PrepareAssociatedExternalAuthModels(model.AssociatedExternalAuthRecords, user);

                //prepare nested search models
                PrepareRewardPointsSearchModel(model.UserRewardPointsSearchModel, user);
                PrepareUserAddressSearchModel(model.UserAddressSearchModel, user);
                PrepareUserOrderSearchModel(model.UserOrderSearchModel, user);
                PrepareUserShoppingCartSearchModel(model.UserShoppingCartSearchModel, user);
                PrepareUserActivityLogSearchModel(model.UserActivityLogSearchModel, user);
                PrepareUserBackInStockSubscriptionSearchModel(model.UserBackInStockSubscriptionSearchModel, user);
            }
            else
            {
                //whether to fill in some of properties
                if (!excludeProperties)
                {
                    //precheck Registered Role as a default role while creating a new user through admin
                    var registeredRole = _userService.GetUserRoleBySystemName(AppUserDefaults.RegisteredRoleName);
                    if (registeredRole != null)
                        model.SelectedUserRoleIds.Add(registeredRole.Id);
                }
            }

            model.UsernamesEnabled = _userSettings.UsernamesEnabled;
            model.AllowUsersToSetTimeZone = _dateTimeSettings.AllowUsersToSetTimeZone;
            model.GenderEnabled = _userSettings.GenderEnabled;
            model.DateOfBirthEnabled = _userSettings.DateOfBirthEnabled;
            model.CompanyEnabled = _userSettings.CompanyEnabled;
            model.StreetAddressEnabled = _userSettings.StreetAddressEnabled;
            model.StreetAddress2Enabled = _userSettings.StreetAddress2Enabled;
            model.ZipPostalCodeEnabled = _userSettings.ZipPostalCodeEnabled;
            model.CityEnabled = _userSettings.CityEnabled;
            model.CountyEnabled = _userSettings.CountyEnabled;
            model.CountryEnabled = _userSettings.CountryEnabled;
            model.StateProvinceEnabled = _userSettings.StateProvinceEnabled;
            model.PhoneEnabled = _userSettings.PhoneEnabled;
            model.FaxEnabled = _userSettings.FaxEnabled;

            //set default values for the new model
            if (user == null)
            {
                model.Active = true;
                model.DisplayVatNumber = false;
            }

            //prepare available vendors
            _baseAdminModelFactory.PrepareVendors(model.AvailableVendors,
                defaultItemText: _localizationService.GetResource("Admin.Users.Users.Fields.Vendor.None"));

            //prepare model user attributes
            PrepareUserAttributeModels(model.UserAttributes, user);

            //prepare model stores for newsletter subscriptions
            model.AvailableNewsletterSubscriptionStores = _storeService.GetAllStores().Select(store => new SelectListItem
            {
                Value = store.Id.ToString(),
                Text = store.Name,
                Selected = model.SelectedNewsletterSubscriptionStoreIds.Contains(store.Id)
            }).ToList();

            //prepare model user roles
            _aclSupportedModelFactory.PrepareModelUserRoles(model);

            //prepare available time zones
            _baseAdminModelFactory.PrepareTimeZones(model.AvailableTimeZones, false);

            //prepare available countries and states
            if (_userSettings.CountryEnabled)
            {
                _baseAdminModelFactory.PrepareCountries(model.AvailableCountries);
                if (_userSettings.StateProvinceEnabled)
                    _baseAdminModelFactory.PrepareStatesAndProvinces(model.AvailableStates, model.CountryId == 0 ? null : (int?)model.CountryId);
            }

            return model;
        }

        /// <summary>
        /// Prepare paged reward points list model
        /// </summary>
        /// <param name="searchModel">Reward points search model</param>
        /// <param name="user">User</param>
        /// <returns>Reward points list model</returns>
        public virtual UserRewardPointsListModel PrepareRewardPointsListModel(UserRewardPointsSearchModel searchModel, User user)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            //get reward points history
            var rewardPoints = _rewardPointService.GetRewardPointsHistory(user.Id,
                showNotActivated: true,
                pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize);

            //prepare list model
            var model = new UserRewardPointsListModel
            {
                Data = rewardPoints.Select(historyEntry =>
                {
                    //fill in model values from the entity        
                    var rewardPointsHistoryModel = new UserRewardPointsModel
                    {
                        Points = historyEntry.Points,
                        Message = historyEntry.Message
                    };

                    //convert dates to the user time
                    var activatingDate = _dateTimeHelper.ConvertToUserTime(historyEntry.CreatedOnUtc, DateTimeKind.Utc);
                    rewardPointsHistoryModel.CreatedOn = activatingDate;
                    rewardPointsHistoryModel.PointsBalance = historyEntry.PointsBalance.HasValue ? historyEntry.PointsBalance.ToString() :
                        string.Format(_localizationService.GetResource("Admin.Users.Users.RewardPoints.ActivatedLater"), activatingDate);
                    rewardPointsHistoryModel.EndDate = !historyEntry.EndDateUtc.HasValue ? null :
                        (DateTime?)_dateTimeHelper.ConvertToUserTime(historyEntry.EndDateUtc.Value, DateTimeKind.Utc);

                    //fill in additional values (not existing in the entity)
                    rewardPointsHistoryModel.StoreName = _storeService.GetStoreById(historyEntry.StoreId)?.Name ?? "Unknown";

                    return rewardPointsHistoryModel;
                }),
                Total = rewardPoints.TotalCount
            };

            return model;
        }

        /// <summary>
        /// Prepare paged user address list model
        /// </summary>
        /// <param name="searchModel">User address search model</param>
        /// <param name="user">User</param>
        /// <returns>User address list model</returns>
        public virtual UserAddressListModel PrepareUserAddressListModel(UserAddressSearchModel searchModel, User user)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            //get user addresses
            var addresses = user.Addresses
                .OrderByDescending(address => address.CreatedOnUtc).ThenByDescending(address => address.Id).ToList();

            //prepare list model
            var model = new UserAddressListModel
            {
                Data = addresses.PaginationByRequestModel(searchModel).Select(address =>
                {
                    //fill in model values from the entity        
                    var addressModel = address.ToModel<AddressModel>();

                    //fill in additional values (not existing in the entity)
                    PrepareModelAddressHtml(addressModel, address);

                    return addressModel;
                }),
                Total = addresses.Count
            };

            return model;
        }

        /// <summary>
        /// Prepare user address model
        /// </summary>
        /// <param name="model">User address model</param>
        /// <param name="user">User</param>
        /// <param name="address">Address</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>User address model</returns>
        public virtual UserAddressModel PrepareUserAddressModel(UserAddressModel model,
            User user, Address address, bool excludeProperties = false)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (address != null)
            {
                //fill in model values from the entity
                model = model ?? new UserAddressModel();

                //whether to fill in some of properties
                if (!excludeProperties)
                    model.Address = address.ToModel(model.Address);
            }

            model.UserId = user.Id;

            //prepare address model
            PrepareAddressModel(model.Address, address);

            return model;
        }

        /// <summary>
        /// Prepare paged user order list model
        /// </summary>
        /// <param name="searchModel">User order search model</param>
        /// <param name="user">User</param>
        /// <returns>User order list model</returns>
        public virtual UserOrderListModel PrepareUserOrderListModel(UserOrderSearchModel searchModel, User user)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            //get user orders
            var orders = _orderService.SearchOrders(userId: user.Id,
                pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize);

            //prepare list model
            var model = new UserOrderListModel
            {
                Data = orders.Select(order =>
                {
                    //fill in model values from the entity
                    var orderModel = new UserOrderModel
                    {
                        Id = order.Id,
                        OrderStatus = _localizationService.GetLocalizedEnum(order.OrderStatus),
                        OrderStatusId = order.OrderStatusId,
                        PaymentStatus = _localizationService.GetLocalizedEnum(order.PaymentStatus),
                        ShippingStatus = _localizationService.GetLocalizedEnum(order.ShippingStatus),
                        OrderTotal = _priceFormatter.FormatPrice(order.OrderTotal, true, false),
                        CustomOrderNumber = order.CustomOrderNumber
                    };

                    //convert dates to the user time
                    orderModel.CreatedOn = _dateTimeHelper.ConvertToUserTime(order.CreatedOnUtc, DateTimeKind.Utc);

                    //fill in additional values (not existing in the entity)
                    orderModel.StoreName = _storeService.GetStoreById(order.StoreId)?.Name ?? "Unknown";

                    return orderModel;
                }),
                Total = orders.TotalCount
            };

            return model;
        }

        /// <summary>
        /// Prepare paged user shopping cart list model
        /// </summary>
        /// <param name="searchModel">User shopping cart search model</param>
        /// <param name="user">User</param>
        /// <returns>User shopping cart list model</returns>
        public virtual UserShoppingCartListModel PrepareUserShoppingCartListModel(UserShoppingCartSearchModel searchModel,
            User user)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            //get user shopping cart
            var shoppingCart = user.ShoppingCartItems.Where(item => item.ShoppingCartTypeId == searchModel.ShoppingCartTypeId).ToList();

            //prepare list model
            var model = new UserShoppingCartListModel
            {
                Data = shoppingCart.PaginationByRequestModel(searchModel).Select(item =>
                {
                    //fill in model values from the entity
                    var shoppingCartItemModel = new ShoppingCartItemModel
                    {
                        Id = item.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        ProductName = item.Product.Name,
                        AttributeInfo = _productAttributeFormatter.FormatAttributes(item.Product, item.AttributesXml),
                        UnitPrice = _priceFormatter
                            .FormatPrice(_taxService.GetProductPrice(item.Product, _priceCalculationService.GetUnitPrice(item), out var _)),
                        Total = _priceFormatter
                            .FormatPrice(_taxService.GetProductPrice(item.Product, _priceCalculationService.GetSubTotal(item), out _))
                    };

                    //convert dates to the user time
                    shoppingCartItemModel.UpdatedOn = _dateTimeHelper.ConvertToUserTime(item.UpdatedOnUtc, DateTimeKind.Utc);

                    //fill in additional values (not existing in the entity)
                    shoppingCartItemModel.Store = _storeService.GetStoreById(item.StoreId)?.Name ?? "Unknown";

                    return shoppingCartItemModel;
                }),
                Total = shoppingCart.Count
            };

            return model;
        }

        /// <summary>
        /// Prepare paged user activity log list model
        /// </summary>
        /// <param name="searchModel">User activity log search model</param>
        /// <param name="user">User</param>
        /// <returns>User activity log list model</returns>
        public virtual UserActivityLogListModel PrepareUserActivityLogListModel(UserActivityLogSearchModel searchModel, User user)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            //get user activity log
            var activityLog = _userActivityService.GetAllActivities(userId: user.Id,
                pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize);

            //prepare list model
            var model = new UserActivityLogListModel
            {
                Data = activityLog.Select(logItem =>
                {
                    //fill in model values from the entity
                    var userActivityLogModel = new UserActivityLogModel
                    {
                        Id = logItem.Id,
                        ActivityLogTypeName = logItem.ActivityLogType.Name,
                        Comment = logItem.Comment,
                        IpAddress = logItem.IpAddress
                    };

                    //convert dates to the user time
                    userActivityLogModel.CreatedOn = _dateTimeHelper.ConvertToUserTime(logItem.CreatedOnUtc, DateTimeKind.Utc);

                    return userActivityLogModel;
                }),
                Total = activityLog.TotalCount
            };

            return model;
        }

        /// <summary>
        /// Prepare paged user back in stock subscriptions list model
        /// </summary>
        /// <param name="searchModel">User back in stock subscriptions search model</param>
        /// <param name="user">User</param>
        /// <returns>User back in stock subscriptions list model</returns>
        public virtual UserBackInStockSubscriptionListModel PrepareUserBackInStockSubscriptionListModel(
            UserBackInStockSubscriptionSearchModel searchModel, User user)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            //get user back in stock subscriptions
            var subscriptions = _backInStockSubscriptionService.GetAllSubscriptionsByUserId(user.Id,
                pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize);

            //prepare list model
            var model = new UserBackInStockSubscriptionListModel
            {
                Data = subscriptions.Select(subscription =>
                {
                    //fill in model values from the entity
                    var subscriptionModel = new UserBackInStockSubscriptionModel
                    {
                        Id = subscription.Id,
                        ProductId = subscription.ProductId
                    };

                    //convert dates to the user time
                    subscriptionModel.CreatedOn = _dateTimeHelper.ConvertToUserTime(subscription.CreatedOnUtc, DateTimeKind.Utc);

                    //fill in additional values (not existing in the entity)
                    subscriptionModel.StoreName = _storeService.GetStoreById(subscription.StoreId)?.Name ?? "Unknown";
                    subscriptionModel.ProductName = subscription.Product?.Name ?? "Unknown";

                    return subscriptionModel;
                }),
                Total = subscriptions.TotalCount
            };

            return model;
        }

        /// <summary>
        /// Prepare online user search model
        /// </summary>
        /// <param name="searchModel">Online user search model</param>
        /// <returns>Online user search model</returns>
        public virtual OnlineUserSearchModel PrepareOnlineUserSearchModel(OnlineUserSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        /// <summary>
        /// Prepare paged online user list model
        /// </summary>
        /// <param name="searchModel">Online user search model</param>
        /// <returns>Online user list model</returns>
        public virtual OnlineUserListModel PrepareOnlineUserListModel(OnlineUserSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get parameters to filter users
            var lastActivityFrom = DateTime.UtcNow.AddMinutes(-_userSettings.OnlineUserMinutes);

            //get online users
            var users = _userService.GetOnlineUsers(userRoleIds: null,
                 lastActivityFromUtc: lastActivityFrom,
                 pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize);

            //prepare list model
            var model = new OnlineUserListModel
            {
                Data = users.Select(user =>
                {
                    //fill in model values from the entity
                    var userModel = new OnlineUserModel
                    {
                        Id = user.Id
                    };

                    //convert dates to the user time
                    userModel.LastActivityDate = _dateTimeHelper.ConvertToUserTime(user.LastActivityDateUtc, DateTimeKind.Utc);

                    //fill in additional values (not existing in the entity)
                    userModel.UserInfo = user.IsRegistered()
                        ? user.Email : _localizationService.GetResource("Admin.Users.Guest");
                    userModel.LastIpAddress = _userSettings.StoreIpAddresses
                        ? user.LastIpAddress : _localizationService.GetResource("Admin.Users.OnlineUsers.Fields.IPAddress.Disabled");
                    userModel.Location = _geoLookupService.LookupCountryName(user.LastIpAddress);
                    userModel.LastVisitedPage = _userSettings.StoreLastVisitedPage
                        ? _genericAttributeService.GetAttribute<string>(user, AppUserDefaults.LastVisitedPageAttribute)
                        : _localizationService.GetResource("Admin.Users.OnlineUsers.Fields.LastVisitedPage.Disabled");

                    return userModel;
                }),
                Total = users.TotalCount
            };

            return model;
        }

        /// <summary>
        /// Prepare GDPR request (log) search model
        /// </summary>
        /// <param name="searchModel">GDPR request search model</param>
        /// <returns>GDPR request search model</returns>
        public virtual GdprLogSearchModel PrepareGdprLogSearchModel(GdprLogSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare request types
            _baseAdminModelFactory.PrepareGdprRequestTypes(searchModel.AvailableRequestTypes);

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        /// <summary>
        /// Prepare paged GDPR request list model
        /// </summary>
        /// <param name="searchModel">GDPR request search model</param>
        /// <returns>GDPR request list model</returns>
        public virtual GdprLogListModel PrepareGdprLogListModel(GdprLogSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            var userId = 0;
            var userInfo = "";
            if (!String.IsNullOrEmpty(searchModel.SearchEmail))
            {
                var user = _userService.GetUserByEmail(searchModel.SearchEmail);
                if (user != null)
                    userId = user.Id;
                else
                {
                    userInfo = searchModel.SearchEmail;
                }
            }
            //get requests
            var gdprLog = _gdprService.GetAllLog(
                userId: userId,
                userInfo: userInfo,
                requestType: searchModel.SearchRequestTypeId > 0 ? (GdprRequestType?)searchModel.SearchRequestTypeId : null,
                pageIndex: searchModel.Page - 1,
                pageSize: searchModel.PageSize);

            //prepare list model
            var model = new GdprLogListModel
            {
                Data = gdprLog.Select(log =>
                {
                    //fill in model values from the entity
                    var user = _userService.GetUserById(log.UserId);

                    var requestModel = new GdprLogModel
                    {
                        Id = log.Id,
                        UserInfo = user != null && !user.Deleted && !String.IsNullOrEmpty(user.Email) ?
                            user.Email :
                            log.UserInfo,
                        RequestType = _localizationService.GetLocalizedEnum(log.RequestType),
                        RequestDetails = log.RequestDetails,
                        CreatedOn = _dateTimeHelper.ConvertToUserTime(log.CreatedOnUtc, DateTimeKind.Utc)
                    };
                    return requestModel;
                }),
                Total = gdprLog.TotalCount
            };

            return model;
        }

        #endregion
    }
}