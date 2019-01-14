using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using Appulu.Core;
using Appulu.Core.Caching;
using Appulu.Core.Data;
using Appulu.Core.Data.Extensions;
using Appulu.Core.Domain.Common;
using Appulu.Core.Domain.Users;
using Appulu.Core.Domain.Orders;
using Appulu.Core.Domain.Shipping;
using Appulu.Core.Domain.Tax;
using Appulu.Core.Infrastructure;
using Appulu.Data;
using Appulu.Services.Common;
using Appulu.Services.Events;
using Appulu.Services.Localization;

namespace Appulu.Services.Users
{
    /// <summary>
    /// User service
    /// </summary>
    public partial class UserService : IUserService
    {
        #region Fields

        private readonly UserSettings _userSettings;
        private readonly ICacheManager _cacheManager;
        private readonly IDataProvider _dataProvider;
        private readonly IDbContext _dbContext;
        private readonly IEventPublisher _eventPublisher;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserUserRoleMapping> _userUserRoleMappingRepository;
        private readonly IRepository<UserPassword> _userPasswordRepository;
        private readonly IRepository<UserRole> _userRoleRepository;
        private readonly IRepository<GenericAttribute> _gaRepository;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly string _entityName;

        #endregion

        #region Ctor

        public UserService(UserSettings userSettings,
            ICacheManager cacheManager,
            IDataProvider dataProvider,
            IDbContext dbContext,
            IEventPublisher eventPublisher,
            IGenericAttributeService genericAttributeService,
            IRepository<User> userRepository,
            IRepository<UserUserRoleMapping> userUserRoleMappingRepository,
            IRepository<UserPassword> userPasswordRepository,
            IRepository<UserRole> userRoleRepository,
            IRepository<GenericAttribute> gaRepository,
            IStaticCacheManager staticCacheManager)
        {
            this._userSettings = userSettings;
            this._cacheManager = cacheManager;
            this._dataProvider = dataProvider;
            this._dbContext = dbContext;
            this._eventPublisher = eventPublisher;
            this._genericAttributeService = genericAttributeService;
            this._userRepository = userRepository;
            this._userUserRoleMappingRepository = userUserRoleMappingRepository;
            this._userPasswordRepository = userPasswordRepository;
            this._userRoleRepository = userRoleRepository;
            this._gaRepository = gaRepository;
            this._staticCacheManager = staticCacheManager;
            this._entityName = typeof(User).Name;
        }

        #endregion

        #region Methods

        #region Users

        /// <summary>
        /// Gets all users
        /// </summary>
        /// <param name="createdFromUtc">Created date from (UTC); null to load all records</param>
        /// <param name="createdToUtc">Created date to (UTC); null to load all records</param>
        /// <param name="affiliateId">Affiliate identifier</param>
        /// <param name="vendorId">Vendor identifier</param>
        /// <param name="userRoleIds">A list of user role identifiers to filter by (at least one match); pass null or empty list in order to load all users; </param>
        /// <param name="email">Email; null to load all users</param>
        /// <param name="username">Username; null to load all users</param>
        /// <param name="firstName">First name; null to load all users</param>
        /// <param name="lastName">Last name; null to load all users</param>
        /// <param name="dayOfBirth">Day of birth; 0 to load all users</param>
        /// <param name="monthOfBirth">Month of birth; 0 to load all users</param>
        /// <param name="company">Company; null to load all users</param>
        /// <param name="phone">Phone; null to load all users</param>
        /// <param name="zipPostalCode">Phone; null to load all users</param>
        /// <param name="ipAddress">IP address; null to load all users</param>
        /// <param name="loadOnlyWithShoppingCart">Value indicating whether to load users only with shopping cart</param>
        /// <param name="sct">Value indicating what shopping cart type to filter; used when 'loadOnlyWithShoppingCart' parameter is 'true'</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="getOnlyTotalCount">A value in indicating whether you want to load only total number of records. Set to "true" if you don't want to load data from database</param>
        /// <returns>Users</returns>
        public virtual IPagedList<User> GetAllUsers(DateTime? createdFromUtc = null,
            DateTime? createdToUtc = null, int affiliateId = 0, int vendorId = 0,
            int[] userRoleIds = null, string email = null, string username = null,
            string firstName = null, string lastName = null,
            int dayOfBirth = 0, int monthOfBirth = 0,
            string company = null, string phone = null, string zipPostalCode = null,
            string ipAddress = null, bool loadOnlyWithShoppingCart = false, ShoppingCartType? sct = null,
            int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false)
        {
            var query = _userRepository.Table;
            if (createdFromUtc.HasValue)
                query = query.Where(c => createdFromUtc.Value <= c.CreatedOnUtc);
            if (createdToUtc.HasValue)
                query = query.Where(c => createdToUtc.Value >= c.CreatedOnUtc);
            if (affiliateId > 0)
                query = query.Where(c => affiliateId == c.AffiliateId);
            if (vendorId > 0)
                query = query.Where(c => vendorId == c.VendorId);
            query = query.Where(c => !c.Deleted);

            if (userRoleIds != null && userRoleIds.Length > 0)
            {
                query = query.Join(_userUserRoleMappingRepository.Table, x => x.Id, y => y.UserId,
                        (x, y) => new { User = x, Mapping = y })
                    .Where(z => userRoleIds.Contains(z.Mapping.UserRoleId))
                    .Select(z => z.User)
                    .Distinct();
            }

            if (!string.IsNullOrWhiteSpace(email))
                query = query.Where(c => c.Email.Contains(email));
            if (!string.IsNullOrWhiteSpace(username))
                query = query.Where(c => c.Username.Contains(username));
            if (!string.IsNullOrWhiteSpace(firstName))
            {
                query = query
                    .Join(_gaRepository.Table, x => x.Id, y => y.EntityId, (x, y) => new { User = x, Attribute = y })
                    .Where(z => z.Attribute.KeyGroup == _entityName &&
                                z.Attribute.Key == AppUserDefaults.FirstNameAttribute &&
                                z.Attribute.Value.Contains(firstName))
                    .Select(z => z.User);
            }

            if (!string.IsNullOrWhiteSpace(lastName))
            {
                query = query
                    .Join(_gaRepository.Table, x => x.Id, y => y.EntityId, (x, y) => new { User = x, Attribute = y })
                    .Where(z => z.Attribute.KeyGroup == _entityName &&
                                z.Attribute.Key == AppUserDefaults.LastNameAttribute &&
                                z.Attribute.Value.Contains(lastName))
                    .Select(z => z.User);
            }

            //date of birth is stored as a string into database.
            //we also know that date of birth is stored in the following format YYYY-MM-DD (for example, 1983-02-18).
            //so let's search it as a string
            if (dayOfBirth > 0 && monthOfBirth > 0)
            {
                //both are specified
                var dateOfBirthStr = monthOfBirth.ToString("00", CultureInfo.InvariantCulture) + "-" + dayOfBirth.ToString("00", CultureInfo.InvariantCulture);

                //z.Attribute.Value.Length - dateOfBirthStr.Length = 5
                //dateOfBirthStr.Length = 5
                query = query
                    .Join(_gaRepository.Table, x => x.Id, y => y.EntityId, (x, y) => new { User = x, Attribute = y })
                    .Where(z => z.Attribute.KeyGroup == _entityName &&
                                z.Attribute.Key == AppUserDefaults.DateOfBirthAttribute &&
                                z.Attribute.Value.Substring(5, 5) == dateOfBirthStr)
                    .Select(z => z.User);
            }
            else if (dayOfBirth > 0)
            {
                //only day is specified
                var dateOfBirthStr = dayOfBirth.ToString("00", CultureInfo.InvariantCulture);

                //z.Attribute.Value.Length - dateOfBirthStr.Length = 8
                //dateOfBirthStr.Length = 2
                query = query
                    .Join(_gaRepository.Table, x => x.Id, y => y.EntityId, (x, y) => new { User = x, Attribute = y })
                    .Where(z => z.Attribute.KeyGroup == _entityName &&
                                z.Attribute.Key == AppUserDefaults.DateOfBirthAttribute &&
                                z.Attribute.Value.Substring(8, 2) == dateOfBirthStr)
                    .Select(z => z.User);
            }
            else if (monthOfBirth > 0)
            {
                //only month is specified
                var dateOfBirthStr = "-" + monthOfBirth.ToString("00", CultureInfo.InvariantCulture) + "-";
                query = query
                    .Join(_gaRepository.Table, x => x.Id, y => y.EntityId, (x, y) => new { User = x, Attribute = y })
                    .Where(z => z.Attribute.KeyGroup == _entityName &&
                                z.Attribute.Key == AppUserDefaults.DateOfBirthAttribute &&
                                z.Attribute.Value.Contains(dateOfBirthStr))
                    .Select(z => z.User);
            }
            //search by company
            if (!string.IsNullOrWhiteSpace(company))
            {
                query = query
                    .Join(_gaRepository.Table, x => x.Id, y => y.EntityId, (x, y) => new { User = x, Attribute = y })
                    .Where(z => z.Attribute.KeyGroup == _entityName &&
                                z.Attribute.Key == AppUserDefaults.CompanyAttribute &&
                                z.Attribute.Value.Contains(company))
                    .Select(z => z.User);
            }
            //search by phone
            if (!string.IsNullOrWhiteSpace(phone))
            {
                query = query
                    .Join(_gaRepository.Table, x => x.Id, y => y.EntityId, (x, y) => new { User = x, Attribute = y })
                    .Where(z => z.Attribute.KeyGroup == _entityName &&
                                z.Attribute.Key == AppUserDefaults.PhoneAttribute &&
                                z.Attribute.Value.Contains(phone))
                    .Select(z => z.User);
            }
            //search by zip
            if (!string.IsNullOrWhiteSpace(zipPostalCode))
            {
                query = query
                    .Join(_gaRepository.Table, x => x.Id, y => y.EntityId, (x, y) => new { User = x, Attribute = y })
                    .Where(z => z.Attribute.KeyGroup == _entityName &&
                                z.Attribute.Key == AppUserDefaults.ZipPostalCodeAttribute &&
                                z.Attribute.Value.Contains(zipPostalCode))
                    .Select(z => z.User);
            }

            //search by IpAddress
            if (!string.IsNullOrWhiteSpace(ipAddress) && CommonHelper.IsValidIpAddress(ipAddress))
            {
                query = query.Where(w => w.LastIpAddress == ipAddress);
            }

            if (loadOnlyWithShoppingCart)
            {
                int? sctId = null;
                if (sct.HasValue)
                    sctId = (int)sct.Value;

                query = sct.HasValue ?
                    query.Where(c => c.ShoppingCartItems.Any(x => x.ShoppingCartTypeId == sctId)) :
                    query.Where(c => c.ShoppingCartItems.Any());
            }

            query = query.OrderByDescending(c => c.CreatedOnUtc);

            var users = new PagedList<User>(query, pageIndex, pageSize, getOnlyTotalCount);
            return users;
        }

        /// <summary>
        /// Gets online users
        /// </summary>
        /// <param name="lastActivityFromUtc">User last activity date (from)</param>
        /// <param name="userRoleIds">A list of user role identifiers to filter by (at least one match); pass null or empty list in order to load all users; </param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Users</returns>
        public virtual IPagedList<User> GetOnlineUsers(DateTime lastActivityFromUtc,
            int[] userRoleIds, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _userRepository.Table;
            query = query.Where(c => lastActivityFromUtc <= c.LastActivityDateUtc);
            query = query.Where(c => !c.Deleted);
            if (userRoleIds != null && userRoleIds.Length > 0)
                query = query.Where(c => c.UserUserRoleMappings.Select(mapping => mapping.UserRoleId).Intersect(userRoleIds).Any());

            query = query.OrderByDescending(c => c.LastActivityDateUtc);
            var users = new PagedList<User>(query, pageIndex, pageSize);
            return users;
        }

        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="user">User</param>
        public virtual void DeleteUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (user.IsSystemAccount)
                throw new AppException($"System user account ({user.SystemName}) could not be deleted");

            user.Deleted = true;

            if (_userSettings.SuffixDeletedUsers)
            {
                if (!string.IsNullOrEmpty(user.Email))
                    user.Email += "-DELETED";
                if (!string.IsNullOrEmpty(user.Username))
                    user.Username += "-DELETED";
            }

            UpdateUser(user);

            //event notification
            _eventPublisher.EntityDeleted(user);
        }

        /// <summary>
        /// Gets a user
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>A user</returns>
        public virtual User GetUserById(int userId)
        {
            if (userId == 0)
                return null;

            return _userRepository.GetById(userId);
        }

        /// <summary>
        /// Get users by identifiers
        /// </summary>
        /// <param name="userIds">User identifiers</param>
        /// <returns>Users</returns>
        public virtual IList<User> GetUsersByIds(int[] userIds)
        {
            if (userIds == null || userIds.Length == 0)
                return new List<User>();

            var query = from c in _userRepository.Table
                        where userIds.Contains(c.Id) && !c.Deleted
                        select c;
            var users = query.ToList();
            //sort by passed identifiers
            var sortedUsers = new List<User>();
            foreach (var id in userIds)
            {
                var user = users.Find(x => x.Id == id);
                if (user != null)
                    sortedUsers.Add(user);
            }

            return sortedUsers;
        }

        /// <summary>
        /// Gets a user by GUID
        /// </summary>
        /// <param name="userGuid">User GUID</param>
        /// <returns>A user</returns>
        public virtual User GetUserByGuid(Guid userGuid)
        {
            if (userGuid == Guid.Empty)
                return null;

            var query = from c in _userRepository.Table
                        where c.UserGuid == userGuid
                        orderby c.Id
                        select c;
            var user = query.FirstOrDefault();
            return user;
        }

        /// <summary>
        /// Get user by email
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>User</returns>
        public virtual User GetUserByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            var query = from c in _userRepository.Table
                        orderby c.Id
                        where c.Email == email
                        select c;
            var user = query.FirstOrDefault();
            return user;
        }

        /// <summary>
        /// Get user by system name
        /// </summary>
        /// <param name="systemName">System name</param>
        /// <returns>User</returns>
        public virtual User GetUserBySystemName(string systemName)
        {
            if (string.IsNullOrWhiteSpace(systemName))
                return null;

            var query = from c in _userRepository.Table
                        orderby c.Id
                        where c.SystemName == systemName
                        select c;
            var user = query.FirstOrDefault();
            return user;
        }

        /// <summary>
        /// Get user by username
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>User</returns>
        public virtual User GetUserByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return null;

            var query = from c in _userRepository.Table
                        orderby c.Id
                        where c.Username == username
                        select c;
            var user = query.FirstOrDefault();
            return user;
        }

        /// <summary>
        /// Insert a guest user
        /// </summary>
        /// <returns>User</returns>
        public virtual User InsertGuestUser()
        {
            var user = new User
            {
                UserGuid = Guid.NewGuid(),
                Active = true,
                CreatedOnUtc = DateTime.UtcNow,
                LastActivityDateUtc = DateTime.UtcNow
            };

            //add to 'Guests' role
            var guestRole = GetUserRoleBySystemName(AppUserDefaults.GuestsRoleName);
            if (guestRole == null)
                throw new AppException("'Guests' role could not be loaded");
            //user.UserRoles.Add(guestRole);
            user.UserUserRoleMappings.Add(new UserUserRoleMapping { UserRole = guestRole });

            _userRepository.Insert(user);

            return user;
        }

        /// <summary>
        /// Insert a user
        /// </summary>
        /// <param name="user">User</param>
        public virtual void InsertUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            _userRepository.Insert(user);

            //event notification
            _eventPublisher.EntityInserted(user);
        }

        /// <summary>
        /// Updates the user
        /// </summary>
        /// <param name="user">User</param>
        public virtual void UpdateUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            _userRepository.Update(user);

            //event notification
            _eventPublisher.EntityUpdated(user);
        }

        /// <summary>
        /// Reset data required for checkout
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="storeId">Store identifier</param>
        /// <param name="clearCouponCodes">A value indicating whether to clear coupon code</param>
        /// <param name="clearCheckoutAttributes">A value indicating whether to clear selected checkout attributes</param>
        /// <param name="clearRewardPoints">A value indicating whether to clear "Use reward points" flag</param>
        /// <param name="clearShippingMethod">A value indicating whether to clear selected shipping method</param>
        /// <param name="clearPaymentMethod">A value indicating whether to clear selected payment method</param>
        public virtual void ResetCheckoutData(User user, int storeId,
            bool clearCouponCodes = false, bool clearCheckoutAttributes = false,
            bool clearRewardPoints = true, bool clearShippingMethod = true,
            bool clearPaymentMethod = true)
        {
            if (user == null)
                throw new ArgumentNullException();

            //clear entered coupon codes
            if (clearCouponCodes)
            {
                _genericAttributeService.SaveAttribute<string>(user, AppUserDefaults.DiscountCouponCodeAttribute, null);
                _genericAttributeService.SaveAttribute<string>(user, AppUserDefaults.GiftCardCouponCodesAttribute, null);
            }

            //clear checkout attributes
            if (clearCheckoutAttributes)
            {
                _genericAttributeService.SaveAttribute<string>(user, AppUserDefaults.CheckoutAttributes, null, storeId);
            }

            //clear reward points flag
            if (clearRewardPoints)
            {
                _genericAttributeService.SaveAttribute(user, AppUserDefaults.UseRewardPointsDuringCheckoutAttribute, false, storeId);
            }

            //clear selected shipping method
            if (clearShippingMethod)
            {
                _genericAttributeService.SaveAttribute<ShippingOption>(user, AppUserDefaults.SelectedShippingOptionAttribute, null, storeId);
                _genericAttributeService.SaveAttribute<ShippingOption>(user, AppUserDefaults.OfferedShippingOptionsAttribute, null, storeId);
                _genericAttributeService.SaveAttribute<PickupPoint>(user, AppUserDefaults.SelectedPickupPointAttribute, null, storeId);
            }

            //clear selected payment method
            if (clearPaymentMethod)
            {
                _genericAttributeService.SaveAttribute<string>(user, AppUserDefaults.SelectedPaymentMethodAttribute, null, storeId);
            }

            UpdateUser(user);
        }

        /// <summary>
        /// Delete guest user records
        /// </summary>
        /// <param name="createdFromUtc">Created date from (UTC); null to load all records</param>
        /// <param name="createdToUtc">Created date to (UTC); null to load all records</param>
        /// <param name="onlyWithoutShoppingCart">A value indicating whether to delete users only without shopping cart</param>
        /// <returns>Number of deleted users</returns>
        public virtual int DeleteGuestUsers(DateTime? createdFromUtc, DateTime? createdToUtc, bool onlyWithoutShoppingCart)
        {
            //prepare parameters
            var pOnlyWithoutShoppingCart = _dataProvider.GetBooleanParameter("OnlyWithoutShoppingCart", onlyWithoutShoppingCart);
            var pCreatedFromUtc = _dataProvider.GetDateTimeParameter("CreatedFromUtc", createdFromUtc);
            var pCreatedToUtc = _dataProvider.GetDateTimeParameter("CreatedToUtc", createdToUtc);
            var pTotalRecordsDeleted = _dataProvider.GetOutputInt32Parameter("TotalRecordsDeleted");

            //invoke stored procedure
            _dbContext.ExecuteSqlCommand(
                "EXEC [DeleteGuests] @OnlyWithoutShoppingCart, @CreatedFromUtc, @CreatedToUtc, @TotalRecordsDeleted OUTPUT",
                false, null,
                pOnlyWithoutShoppingCart,
                pCreatedFromUtc,
                pCreatedToUtc,
                pTotalRecordsDeleted);

            var totalRecordsDeleted = pTotalRecordsDeleted.Value != DBNull.Value ? Convert.ToInt32(pTotalRecordsDeleted.Value) : 0;
            return totalRecordsDeleted;
        }

        /// <summary>
        /// Gets a default tax display type (if configured)
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Result</returns>
        public virtual TaxDisplayType? GetUserDefaultTaxDisplayType(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var roleWithOverriddenTaxType = user.UserRoles.FirstOrDefault(cr => cr.Active && cr.OverrideTaxDisplayType);
            if (roleWithOverriddenTaxType == null)
                return null;

            return (TaxDisplayType)roleWithOverriddenTaxType.DefaultTaxDisplayTypeId;
        }

        /// <summary>
        /// Remove address
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="address">Address</param>
        public virtual void RemoveUserAddress(User user, Address address)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (!user.Addresses.Contains(address))
                return;

            if (user.BillingAddress == address) user.BillingAddress = null;
            if (user.ShippingAddress == address) user.ShippingAddress = null;

            //user.Addresses.Remove(address);
            user.UserAddressMappings
                .Remove(user.UserAddressMappings.FirstOrDefault(mapping => mapping.AddressId == address.Id));
        }

        /// <summary>
        /// Get full name
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>User full name</returns>
        public virtual string GetUserFullName(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var firstName = _genericAttributeService.GetAttribute<string>(user, AppUserDefaults.FirstNameAttribute);
            var lastName = _genericAttributeService.GetAttribute<string>(user, AppUserDefaults.LastNameAttribute);

            var fullName = string.Empty;
            if (!string.IsNullOrWhiteSpace(firstName) && !string.IsNullOrWhiteSpace(lastName))
                fullName = $"{firstName} {lastName}";
            else
            {
                if (!string.IsNullOrWhiteSpace(firstName))
                    fullName = firstName;

                if (!string.IsNullOrWhiteSpace(lastName))
                    fullName = lastName;
            }

            return fullName;
        }

        /// <summary>
        /// Formats the user name
        /// </summary>
        /// <param name="user">Source</param>
        /// <param name="stripTooLong">Strip too long user name</param>
        /// <param name="maxLength">Maximum user name length</param>
        /// <returns>Formatted text</returns>
        public virtual string FormatUserName(User user, bool stripTooLong = false, int maxLength = 0)
        {
            if (user == null)
                return string.Empty;

            if (user.IsGuest())
                return EngineContext.Current.Resolve<ILocalizationService>().GetResource("User.Guest");

            var result = string.Empty;
            switch (_userSettings.UserNameFormat)
            {
                case UserNameFormat.ShowEmails:
                    result = user.Email;
                    break;
                case UserNameFormat.ShowUsernames:
                    result = user.Username;
                    break;
                case UserNameFormat.ShowFullNames:
                    result = this.GetUserFullName(user);
                    break;
                case UserNameFormat.ShowFirstName:
                    result = _genericAttributeService.GetAttribute<string>(user, AppUserDefaults.FirstNameAttribute);
                    break;
                default:
                    break;
            }

            if (stripTooLong && maxLength > 0)
                result = CommonHelper.EnsureMaximumLength(result, maxLength);

            return result;
        }

        /// <summary>
        /// Gets coupon codes
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Coupon codes</returns>
        public virtual string[] ParseAppliedDiscountCouponCodes(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var existingCouponCodes = _genericAttributeService.GetAttribute<string>(user, AppUserDefaults.DiscountCouponCodeAttribute);

            var couponCodes = new List<string>();
            if (string.IsNullOrEmpty(existingCouponCodes))
                return couponCodes.ToArray();

            try
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(existingCouponCodes);

                var nodeList1 = xmlDoc.SelectNodes(@"//DiscountCouponCodes/CouponCode");
                foreach (XmlNode node1 in nodeList1)
                {
                    if (node1.Attributes?["Code"] == null) 
                        continue;
                    var code = node1.Attributes["Code"].InnerText.Trim();
                    couponCodes.Add(code);
                }
            }
            catch
            {
                // ignored
            }

            return couponCodes.ToArray();
        }

        /// <summary>
        /// Adds a coupon code
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="couponCode">Coupon code</param>
        /// <returns>New coupon codes document</returns>
        public virtual void ApplyDiscountCouponCode(User user, string couponCode)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var result = string.Empty;
            try
            {
                var existingCouponCodes = _genericAttributeService.GetAttribute<string>(user, AppUserDefaults.DiscountCouponCodeAttribute);

                couponCode = couponCode.Trim().ToLower();

                var xmlDoc = new XmlDocument();
                if (string.IsNullOrEmpty(existingCouponCodes))
                {
                    var element1 = xmlDoc.CreateElement("DiscountCouponCodes");
                    xmlDoc.AppendChild(element1);
                }
                else
                {
                    xmlDoc.LoadXml(existingCouponCodes);
                }

                var rootElement = (XmlElement)xmlDoc.SelectSingleNode(@"//DiscountCouponCodes");

                XmlElement gcElement = null;
                //find existing
                var nodeList1 = xmlDoc.SelectNodes(@"//DiscountCouponCodes/CouponCode");
                foreach (XmlNode node1 in nodeList1)
                {
                    if (node1.Attributes?["Code"] == null) 
                        continue;

                    var couponCodeAttribute = node1.Attributes["Code"].InnerText.Trim();

                    if (couponCodeAttribute.ToLower() != couponCode.ToLower()) 
                        continue;

                    gcElement = (XmlElement)node1;
                    break;
                }

                //create new one if not found
                if (gcElement == null)
                {
                    gcElement = xmlDoc.CreateElement("CouponCode");
                    gcElement.SetAttribute("Code", couponCode);
                    rootElement.AppendChild(gcElement);
                }

                result = xmlDoc.OuterXml;
            }
            catch
            {
                // ignored
            }

            //apply new value
            _genericAttributeService.SaveAttribute(user, AppUserDefaults.DiscountCouponCodeAttribute, result);
        }

        /// <summary>
        /// Removes a coupon code
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="couponCode">Coupon code to remove</param>
        /// <returns>New coupon codes document</returns>
        public virtual void RemoveDiscountCouponCode(User user, string couponCode)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            //get applied coupon codes
            var existingCouponCodes = this.ParseAppliedDiscountCouponCodes(user);

            //clear them
            _genericAttributeService.SaveAttribute<string>(user, AppUserDefaults.DiscountCouponCodeAttribute, null);

            //save again except removed one
            foreach (var existingCouponCode in existingCouponCodes)
                if (!existingCouponCode.Equals(couponCode, StringComparison.InvariantCultureIgnoreCase))
                    this.ApplyDiscountCouponCode(user, existingCouponCode);
        }

        /// <summary>
        /// Gets coupon codes
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Coupon codes</returns>
        public virtual string[] ParseAppliedGiftCardCouponCodes(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var existingCouponCodes = _genericAttributeService.GetAttribute<string>(user, AppUserDefaults.GiftCardCouponCodesAttribute);

            var couponCodes = new List<string>();
            if (string.IsNullOrEmpty(existingCouponCodes))
                return couponCodes.ToArray();

            try
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(existingCouponCodes);

                var nodeList1 = xmlDoc.SelectNodes(@"//GiftCardCouponCodes/CouponCode");
                foreach (XmlNode node1 in nodeList1)
                {
                    if (node1.Attributes?["Code"] == null) 
                        continue;

                    var code = node1.Attributes["Code"].InnerText.Trim();
                    couponCodes.Add(code);
                }
            }
            catch
            {
                // ignored
            }

            return couponCodes.ToArray();
        }

        /// <summary>
        /// Adds a coupon code
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="couponCode">Coupon code</param>
        /// <returns>New coupon codes document</returns>
        public virtual void ApplyGiftCardCouponCode(User user, string couponCode)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var result = string.Empty;
            try
            {
                var existingCouponCodes = _genericAttributeService.GetAttribute<string>(user, AppUserDefaults.GiftCardCouponCodesAttribute);

                couponCode = couponCode.Trim().ToLower();

                var xmlDoc = new XmlDocument();
                if (string.IsNullOrEmpty(existingCouponCodes))
                {
                    var element1 = xmlDoc.CreateElement("GiftCardCouponCodes");
                    xmlDoc.AppendChild(element1);
                }
                else
                {
                    xmlDoc.LoadXml(existingCouponCodes);
                }

                var rootElement = (XmlElement)xmlDoc.SelectSingleNode(@"//GiftCardCouponCodes");

                XmlElement gcElement = null;
                //find existing
                var nodeList1 = xmlDoc.SelectNodes(@"//GiftCardCouponCodes/CouponCode");
                foreach (XmlNode node1 in nodeList1)
                {
                    if (node1.Attributes?["Code"] == null) 
                        continue;

                    var couponCodeAttribute = node1.Attributes["Code"].InnerText.Trim();
                    if (couponCodeAttribute.ToLower() != couponCode.ToLower()) 
                        continue;

                    gcElement = (XmlElement)node1;
                    break;
                }

                //create new one if not found
                if (gcElement == null)
                {
                    gcElement = xmlDoc.CreateElement("CouponCode");
                    gcElement.SetAttribute("Code", couponCode);
                    rootElement.AppendChild(gcElement);
                }

                result = xmlDoc.OuterXml;
            }
            catch
            {
                // ignored
            }

            //apply new value
            _genericAttributeService.SaveAttribute(user, AppUserDefaults.GiftCardCouponCodesAttribute, result);
        }

        /// <summary>
        /// Removes a coupon code
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="couponCode">Coupon code to remove</param>
        /// <returns>New coupon codes document</returns>
        public virtual void RemoveGiftCardCouponCode(User user, string couponCode)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            //get applied coupon codes
            var existingCouponCodes = this.ParseAppliedGiftCardCouponCodes(user);

            //clear them
            _genericAttributeService.SaveAttribute<string>(user, AppUserDefaults.GiftCardCouponCodesAttribute, null);

            //save again except removed one
            foreach (var existingCouponCode in existingCouponCodes)
                if (!existingCouponCode.Equals(couponCode, StringComparison.InvariantCultureIgnoreCase))
                    this.ApplyGiftCardCouponCode(user, existingCouponCode);
        }

        #endregion

        #region User roles

        /// <summary>
        /// Delete a user role
        /// </summary>
        /// <param name="userRole">User role</param>
        public virtual void DeleteUserRole(UserRole userRole)
        {
            if (userRole == null)
                throw new ArgumentNullException(nameof(userRole));

            if (userRole.IsSystemRole)
                throw new AppException("System role could not be deleted");

            _userRoleRepository.Delete(userRole);

            _cacheManager.RemoveByPattern(AppUserServiceDefaults.UserRolesPatternCacheKey);

            //event notification
            _eventPublisher.EntityDeleted(userRole);
        }

        /// <summary>
        /// Gets a user role
        /// </summary>
        /// <param name="userRoleId">User role identifier</param>
        /// <returns>User role</returns>
        public virtual UserRole GetUserRoleById(int userRoleId)
        {
            if (userRoleId == 0)
                return null;

            return _userRoleRepository.GetById(userRoleId);
        }

        /// <summary>
        /// Gets a user role
        /// </summary>
        /// <param name="systemName">User role system name</param>
        /// <returns>User role</returns>
        public virtual UserRole GetUserRoleBySystemName(string systemName)
        {
            if (string.IsNullOrWhiteSpace(systemName))
                return null;

            var key = string.Format(AppUserServiceDefaults.UserRolesBySystemNameCacheKey, systemName);
            return _cacheManager.Get(key, () =>
            {
                var query = from cr in _userRoleRepository.Table
                            orderby cr.Id
                            where cr.SystemName == systemName
                            select cr;
                var userRole = query.FirstOrDefault();
                return userRole;
            });
        }

        /// <summary>
        /// Gets all user roles
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>User roles</returns>
        public virtual IList<UserRole> GetAllUserRoles(bool showHidden = false)
        {
            var key = string.Format(AppUserServiceDefaults.UserRolesAllCacheKey, showHidden);
            return _cacheManager.Get(key, () =>
            {
                var query = from cr in _userRoleRepository.Table
                            orderby cr.Name
                            where showHidden || cr.Active
                            select cr;
                var userRoles = query.ToList();
                return userRoles;
            });
        }

        /// <summary>
        /// Inserts a user role
        /// </summary>
        /// <param name="userRole">User role</param>
        public virtual void InsertUserRole(UserRole userRole)
        {
            if (userRole == null)
                throw new ArgumentNullException(nameof(userRole));

            _userRoleRepository.Insert(userRole);

            _cacheManager.RemoveByPattern(AppUserServiceDefaults.UserRolesPatternCacheKey);

            //event notification
            _eventPublisher.EntityInserted(userRole);
        }

        /// <summary>
        /// Updates the user role
        /// </summary>
        /// <param name="userRole">User role</param>
        public virtual void UpdateUserRole(UserRole userRole)
        {
            if (userRole == null)
                throw new ArgumentNullException(nameof(userRole));

            _userRoleRepository.Update(userRole);

            _cacheManager.RemoveByPattern(AppUserServiceDefaults.UserRolesPatternCacheKey);

            //event notification
            _eventPublisher.EntityUpdated(userRole);
        }

        #endregion

        #region User passwords

        /// <summary>
        /// Gets user passwords
        /// </summary>
        /// <param name="userId">User identifier; pass null to load all records</param>
        /// <param name="passwordFormat">Password format; pass null to load all records</param>
        /// <param name="passwordsToReturn">Number of returning passwords; pass null to load all records</param>
        /// <returns>List of user passwords</returns>
        public virtual IList<UserPassword> GetUserPasswords(int? userId = null,
            PasswordFormat? passwordFormat = null, int? passwordsToReturn = null)
        {
            var query = _userPasswordRepository.Table;

            //filter by user
            if (userId.HasValue)
                query = query.Where(password => password.UserId == userId.Value);

            //filter by password format
            if (passwordFormat.HasValue)
                query = query.Where(password => password.PasswordFormatId == (int)passwordFormat.Value);

            //get the latest passwords
            if (passwordsToReturn.HasValue)
                query = query.OrderByDescending(password => password.CreatedOnUtc).Take(passwordsToReturn.Value);

            return query.ToList();
        }

        /// <summary>
        /// Get current user password
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>User password</returns>
        public virtual UserPassword GetCurrentPassword(int userId)
        {
            if (userId == 0)
                return null;

            //return the latest password
            return GetUserPasswords(userId, passwordsToReturn: 1).FirstOrDefault();
        }

        /// <summary>
        /// Insert a user password
        /// </summary>
        /// <param name="userPassword">User password</param>
        public virtual void InsertUserPassword(UserPassword userPassword)
        {
            if (userPassword == null)
                throw new ArgumentNullException(nameof(userPassword));

            _userPasswordRepository.Insert(userPassword);

            //event notification
            _eventPublisher.EntityInserted(userPassword);
        }

        /// <summary>
        /// Update a user password
        /// </summary>
        /// <param name="userPassword">User password</param>
        public virtual void UpdateUserPassword(UserPassword userPassword)
        {
            if (userPassword == null)
                throw new ArgumentNullException(nameof(userPassword));

            _userPasswordRepository.Update(userPassword);

            //event notification
            _eventPublisher.EntityUpdated(userPassword);
        }

        /// <summary>
        /// Check whether password recovery token is valid
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="token">Token to validate</param>
        /// <returns>Result</returns>
        public virtual bool IsPasswordRecoveryTokenValid(User user, string token)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var cPrt = _genericAttributeService.GetAttribute<string>(user, AppUserDefaults.PasswordRecoveryTokenAttribute);
            if (string.IsNullOrEmpty(cPrt))
                return false;

            if (!cPrt.Equals(token, StringComparison.InvariantCultureIgnoreCase))
                return false;

            return true;
        }

        /// <summary>
        /// Check whether password recovery link is expired
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Result</returns>
        public virtual bool IsPasswordRecoveryLinkExpired(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (_userSettings.PasswordRecoveryLinkDaysValid == 0)
                return false;

            var geneatedDate = _genericAttributeService.GetAttribute<DateTime?>(user, AppUserDefaults.PasswordRecoveryTokenDateGeneratedAttribute);
            if (!geneatedDate.HasValue)
                return false;

            var daysPassed = (DateTime.UtcNow - geneatedDate.Value).TotalDays;
            if (daysPassed > _userSettings.PasswordRecoveryLinkDaysValid)
                return true;

            return false;
        }

        /// <summary>
        /// Check whether user password is expired 
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>True if password is expired; otherwise false</returns>
        public virtual bool PasswordIsExpired(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            //the guests don't have a password
            if (user.IsGuest())
                return false;

            //password lifetime is disabled for user
            if (!user.UserRoles.Any(role => role.Active && role.EnablePasswordLifetime))
                return false;

            //setting disabled for all
            if (_userSettings.PasswordLifetime == 0)
                return false;

            //cache result between HTTP requests
            var cacheKey = string.Format(AppUserServiceDefaults.UserPasswordLifetimeCacheKey, user.Id);

            //get current password usage time
            var currentLifetime = _staticCacheManager.Get(cacheKey, () =>
            {
                var userPassword = this.GetCurrentPassword(user.Id);
                //password is not found, so return max value to force user to change password
                if (userPassword == null)
                    return int.MaxValue;

                return (DateTime.UtcNow - userPassword.CreatedOnUtc).Days;
            });

            return currentLifetime >= _userSettings.PasswordLifetime;
        }

        #endregion

        #endregion
    }
}