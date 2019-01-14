using System;
using System.Collections.Generic;
using System.Linq;
using Appulu.Core;
using Appulu.Core.Data;
using Appulu.Core.Domain.Users;
using Appulu.Core.Domain.Gdpr;
using Appulu.Data.Extensions;
using Appulu.Services.Authentication.External;
using Appulu.Services.Blogs;
using Appulu.Services.Catalog;
using Appulu.Services.Common;
using Appulu.Services.Users;
using Appulu.Services.Events;
using Appulu.Services.Forums;
using Appulu.Services.Messages;
using Appulu.Services.News;
using Appulu.Services.Orders;
using Appulu.Services.Stores;

namespace Appulu.Services.Gdpr
{
    /// <summary>
    /// Represents the GDPR service
    /// </summary>
    public partial class GdprService : IGdprService
    {
        #region Fields

        private readonly IAddressService _addressService;
        private readonly IBackInStockSubscriptionService _backInStockSubscriptionService;
        private readonly IBlogService _blogService;
        private readonly IUserService _userService;
        private readonly IExternalAuthenticationService _externalAuthenticationService;
        private readonly IEventPublisher _eventPublisher;
        private readonly IForumService _forumService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly INewsLetterSubscriptionService _newsLetterSubscriptionService;
        private readonly INewsService _newsService;
        private readonly IProductService _productService;
        private readonly IRepository<GdprConsent> _gdprConsentRepository;
        private readonly IRepository<GdprLog> _gdprLogRepository;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IStoreService _storeService;

        #endregion

        #region Ctor

        public GdprService(IAddressService addressService,
            IBackInStockSubscriptionService backInStockSubscriptionService,
            IBlogService blogService,
            IUserService userService,
            IExternalAuthenticationService externalAuthenticationService,
            IEventPublisher eventPublisher,
            IForumService forumService,
            IGenericAttributeService genericAttributeService,
            INewsService newsService,
            INewsLetterSubscriptionService newsLetterSubscriptionService,
            IProductService productService,
            IRepository<GdprConsent> gdprConsentRepository,
            IRepository<GdprLog> gdprLogRepository,
            IShoppingCartService shoppingCartService,
            IStoreService storeService)
        {
            this._addressService = addressService;
            this._backInStockSubscriptionService = backInStockSubscriptionService;
            this._blogService = blogService;
            this._userService = userService;
            this._externalAuthenticationService = externalAuthenticationService;
            this._eventPublisher = eventPublisher;
            this._forumService = forumService;
            this._genericAttributeService = genericAttributeService;
            this._newsService = newsService;
            this._newsLetterSubscriptionService = newsLetterSubscriptionService;
            this._productService = productService;
            this._gdprConsentRepository = gdprConsentRepository;
            this._gdprLogRepository = gdprLogRepository;
            this._shoppingCartService = shoppingCartService;
            this._storeService = storeService;
        }

        #endregion

        #region Methods

        #region GDPR consent

        /// <summary>
        /// Get a GDPR consent
        /// </summary>
        /// <param name="gdprConsentId">The GDPR consent identifier</param>
        /// <returns>GDPR consent</returns>
        public virtual GdprConsent GetConsentById(int gdprConsentId)
        {
            if (gdprConsentId == 0)
                return null;

            return _gdprConsentRepository.GetById(gdprConsentId);
        }

        /// <summary>
        /// Get all GDPR consents
        /// </summary>
        /// <returns>GDPR consent</returns>
        public virtual IList<GdprConsent> GetAllConsents()
        {
            var query = from c in _gdprConsentRepository.Table
                        orderby c.DisplayOrder, c.Id
                        select c;
            var gdprConsents = query.ToList();
            return gdprConsents;
        }

        /// <summary>
        /// Insert a GDPR consent
        /// </summary>
        /// <param name="gdprConsent">GDPR consent</param>
        public virtual void InsertConsent(GdprConsent gdprConsent)
        {
            if (gdprConsent == null)
                throw new ArgumentNullException(nameof(gdprConsent));

            _gdprConsentRepository.Insert(gdprConsent);

            //event notification
            _eventPublisher.EntityInserted(gdprConsent);
        }

        /// <summary>
        /// Update the GDPR consent
        /// </summary>
        /// <param name="gdprConsent">GDPR consent</param>
        public virtual void UpdateConsent(GdprConsent gdprConsent)
        {
            if (gdprConsent == null)
                throw new ArgumentNullException(nameof(gdprConsent));

            _gdprConsentRepository.Update(gdprConsent);

            //event notification
            _eventPublisher.EntityUpdated(gdprConsent);
        }

        /// <summary>
        /// Delete a GDPR consent
        /// </summary>
        /// <param name="gdprConsent">GDPR consent</param>
        public virtual void DeleteConsent(GdprConsent gdprConsent)
        {
            if (gdprConsent == null)
                throw new ArgumentNullException(nameof(gdprConsent));

            _gdprConsentRepository.Delete(gdprConsent);

            //event notification
            _eventPublisher.EntityDeleted(gdprConsent);
        }

        /// <summary>
        /// Gets the latest selected value (a consent is accepted or not by a user)
        /// </summary>
        /// <param name="consentId">Consent identifier</param>
        /// <param name="userId">User identifier</param>
        /// <returns>Result; null if previous a user hasn't been asked</returns>
        public virtual bool? IsConsentAccepted(int consentId, int userId)
        {
            //get latest record
            var log = GetAllLog(userId: userId, consentId: consentId, pageIndex: 0, pageSize: 1).FirstOrDefault();
            if (log == null)
                return null;

            switch (log.RequestType)
            {
                case GdprRequestType.ConsentAgree:
                    return true;
                case GdprRequestType.ConsentDisagree:
                    return false;
                default:
                    return null;
            }
        }
        #endregion

        #region GDPR log

        /// <summary>
        /// Get a GDPR log
        /// </summary>
        /// <param name="gdprLogId">The GDPR log identifier</param>
        /// <returns>GDPR log</returns>
        public virtual GdprLog GetLogById(int gdprLogId)
        {
            if (gdprLogId == 0)
                return null;

            return _gdprLogRepository.GetById(gdprLogId);
        }

        /// <summary>
        /// Get all GDPR log records
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <param name="consentId">Consent identifier</param>
        /// <param name="userInfo">User info (Exact match)</param>
        /// <param name="requestType">GDPR request type</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>GDPR log records</returns>
        public virtual IPagedList<GdprLog> GetAllLog(int userId = 0, int consentId = 0,
            string userInfo = "", GdprRequestType? requestType = null,
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _gdprLogRepository.Table;
            if (userId > 0)
            {
                query = query.Where(log => log.UserId == userId);
            }

            if (consentId > 0)
            {
                query = query.Where(log => log.ConsentId == consentId);
            }

            if (!string.IsNullOrEmpty(userInfo))
            {
                query = query.Where(log => log.UserInfo == userInfo);
            }

            if (requestType != null)
            {
                var requestTypeId = (int)requestType;
                query = query.Where(log => log.RequestTypeId == requestTypeId);
            }

            query = query.OrderByDescending(log => log.CreatedOnUtc).ThenByDescending(log => log.Id);
            return new PagedList<GdprLog>(query, pageIndex, pageSize);
        }

        /// <summary>
        /// Insert a GDPR log
        /// </summary>
        /// <param name="gdprLog">GDPR log</param>
        public virtual void InsertLog(GdprLog gdprLog)
        {
            if (gdprLog == null)
                throw new ArgumentNullException(nameof(gdprLog));

            _gdprLogRepository.Insert(gdprLog);

            //event notification
            _eventPublisher.EntityInserted(gdprLog);
        }

        /// <summary>
        /// Insert a GDPR log
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="consentId">Consent identifier</param>
        /// <param name="requestType">Request type</param>
        /// <param name="requestDetails">Request details</param>
        public virtual void InsertLog(User user, int consentId, GdprRequestType requestType, string requestDetails)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var gdprLog = new GdprLog
            {
                UserId = user.Id,
                ConsentId = consentId,
                UserInfo = user.Email,
                RequestType = requestType,
                RequestDetails = requestDetails,
                CreatedOnUtc = DateTime.UtcNow
            };
            InsertLog(gdprLog);
        }

        /// <summary>
        /// Update the GDPR log
        /// </summary>
        /// <param name="gdprLog">GDPR log</param>
        public virtual void UpdateLog(GdprLog gdprLog)
        {
            if (gdprLog == null)
                throw new ArgumentNullException(nameof(gdprLog));

            _gdprLogRepository.Update(gdprLog);

            //event notification
            _eventPublisher.EntityUpdated(gdprLog);
        }

        /// <summary>
        /// Delete a GDPR log
        /// </summary>
        /// <param name="gdprLog">GDPR log</param>
        public virtual void DeleteLog(GdprLog gdprLog)
        {
            if (gdprLog == null)
                throw new ArgumentNullException(nameof(gdprLog));

            _gdprLogRepository.Delete(gdprLog);

            //event notification
            _eventPublisher.EntityDeleted(gdprLog);
        }

        #endregion

        #region User

        /// <summary>
        /// Permanent delete of user
        /// </summary>
        /// <param name="user">User</param>
        public virtual void PermanentDeleteUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            //blog comments
            var blogComments = _blogService.GetAllComments(userId: user.Id);
            _blogService.DeleteBlogComments(blogComments);

            //news comments
            var newsComments = _newsService.GetAllComments(userId: user.Id);
            _newsService.DeleteNewsComments(newsComments);

            //back in stock subscriptions
            var backInStockSubscriptions = _backInStockSubscriptionService.GetAllSubscriptionsByUserId(user.Id);
            foreach (var backInStockSubscription in backInStockSubscriptions)
                _backInStockSubscriptionService.DeleteSubscription(backInStockSubscription);

            //product review
            var productReviews = _productService.GetAllProductReviews(userId: user.Id, approved: null);
            var reviewedProducts = _productService.GetProductsByIds(productReviews.Select(p => p.ProductId).Distinct().ToArray());
            _productService.DeleteProductReviews(productReviews);
            //update product totals
            foreach (var product in reviewedProducts)
            {
                _productService.UpdateProductReviewTotals(product);
            }

            //external authentication record
            foreach (var ear in user.ExternalAuthenticationRecords)
                _externalAuthenticationService.DeleteExternalAuthenticationRecord(ear);

            //forum subscriptions
            var forumSubscriptions = _forumService.GetAllSubscriptions(user.Id);
            foreach (var forumSubscription in forumSubscriptions)
                _forumService.DeleteSubscription(forumSubscription);

            //shopping cart items
            foreach (var sci in user.ShoppingCartItems)
                _shoppingCartService.DeleteShoppingCartItem(sci);

            //private messages (sent)
            foreach (var pm in _forumService.GetAllPrivateMessages(0, user.Id, 0, null, null, null, null))
                _forumService.DeletePrivateMessage(pm);

            //private messages (received)
            foreach (var pm in _forumService.GetAllPrivateMessages(0, 0, user.Id, null, null, null, null))
                _forumService.DeletePrivateMessage(pm);

            //newsletter
            var allStores = _storeService.GetAllStores();
            foreach (var store in allStores)
            {
                var newsletter = _newsLetterSubscriptionService.GetNewsLetterSubscriptionByEmailAndStoreId(user.Email, store.Id);
                if (newsletter != null)
                    _newsLetterSubscriptionService.DeleteNewsLetterSubscription(newsletter);
            }

            //addresses
            foreach (var address in user.Addresses)
            {
                _userService.RemoveUserAddress(user, address);
                _userService.UpdateUser(user);
                //now delete the address record
                _addressService.DeleteAddress(address);
            }

            //generic attributes
            var keyGroup = user.GetUnproxiedEntityType().Name;
            var genericAttributes = _genericAttributeService.GetAttributesForEntity(user.Id, keyGroup);
            _genericAttributeService.DeleteAttributes(genericAttributes);

            //ignore ActivityLog
            //ignore ForumPost, ForumTopic, ignore ForumPostVote
            //ignore Log
            //ignore PollVotingRecord
            //ignore ProductReviewHelpfulness
            //ignore RecurringPayment 
            //ignore ReturnRequest
            //ignore RewardPointsHistory
            //and we do not delete orders

            //remove from Registered role, add to Guest one
            if (user.IsRegistered())
            {
                var registeredRole = _userService.GetUserRoleBySystemName(AppUserDefaults.RegisteredRoleName);
                user.UserUserRoleMappings
                    .Remove(user.UserUserRoleMappings.FirstOrDefault(mapping => mapping.UserRoleId == registeredRole.Id));
            }

            if (!user.IsGuest())
            {
                var guestRole = _userService.GetUserRoleBySystemName(AppUserDefaults.GuestsRoleName);
                user.UserUserRoleMappings.Add(new UserUserRoleMapping { UserRole = guestRole });
            }

            var email = user.Email;

            //clear other information
            user.Email = string.Empty;
            user.EmailToRevalidate = string.Empty;
            user.Username = string.Empty;
            user.Active = false;
            user.Deleted = true;
            _userService.UpdateUser(user);

            //raise event
            _eventPublisher.Publish(new UserPermanentlyDeleted(user.Id, email));
        }

        #endregion

        #endregion
    }
}