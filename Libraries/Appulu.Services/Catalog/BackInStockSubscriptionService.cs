using System;
using System.Linq;
using Appulu.Core;
using Appulu.Core.Data;
using Appulu.Core.Domain.Catalog;
using Appulu.Core.Domain.Users;
using Appulu.Services.Common;
using Appulu.Services.Events;
using Appulu.Services.Messages;

namespace Appulu.Services.Catalog
{
    /// <summary>
    /// Back in stock subscription service
    /// </summary>
    public partial class BackInStockSubscriptionService : IBackInStockSubscriptionService
    {
        #region Fields

        private readonly IEventPublisher _eventPublisher;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IRepository<BackInStockSubscription> _backInStockSubscriptionRepository;
        private readonly IWorkflowMessageService _workflowMessageService;

        #endregion

        #region Ctor

        public BackInStockSubscriptionService(IEventPublisher eventPublisher,
            IGenericAttributeService genericAttributeService,
            IRepository<BackInStockSubscription> backInStockSubscriptionRepository,
            IWorkflowMessageService workflowMessageService)
        {
            this._eventPublisher = eventPublisher;
            this._genericAttributeService = genericAttributeService;
            this._backInStockSubscriptionRepository = backInStockSubscriptionRepository;
            this._workflowMessageService = workflowMessageService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete a back in stock subscription
        /// </summary>
        /// <param name="subscription">Subscription</param>
        public virtual void DeleteSubscription(BackInStockSubscription subscription)
        {
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));

            _backInStockSubscriptionRepository.Delete(subscription);

            //event notification
            _eventPublisher.EntityDeleted(subscription);
        }

        /// <summary>
        /// Gets all subscriptions
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <param name="storeId">Store identifier; pass 0 to load all records</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Subscriptions</returns>
        public virtual IPagedList<BackInStockSubscription> GetAllSubscriptionsByUserId(int userId,
            int storeId = 0, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _backInStockSubscriptionRepository.Table;
            //user
            query = query.Where(biss => biss.UserId == userId);
            //store
            if (storeId > 0)
                query = query.Where(biss => biss.StoreId == storeId);
            //product
            query = query.Where(biss => !biss.Product.Deleted);
            query = query.OrderByDescending(biss => biss.CreatedOnUtc);

            return new PagedList<BackInStockSubscription>(query, pageIndex, pageSize);
        }

        /// <summary>
        /// Gets all subscriptions
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="storeId">Store identifier; pass 0 to load all records</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Subscriptions</returns>
        public virtual IPagedList<BackInStockSubscription> GetAllSubscriptionsByProductId(int productId,
            int storeId = 0, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _backInStockSubscriptionRepository.Table;
            //product
            query = query.Where(biss => biss.ProductId == productId);
            //store
            if (storeId > 0)
                query = query.Where(biss => biss.StoreId == storeId);
            //user
            query = query.Where(biss => !biss.User.Deleted && biss.User.Active);
            query = query.OrderByDescending(biss => biss.CreatedOnUtc);
            return new PagedList<BackInStockSubscription>(query, pageIndex, pageSize);
        }

        /// <summary>
        /// Gets all subscriptions
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="productId">Product identifier</param>
        /// <param name="storeId">Store identifier</param>
        /// <returns>Subscriptions</returns>
        public virtual BackInStockSubscription FindSubscription(int userId, int productId, int storeId)
        {
            var query = from biss in _backInStockSubscriptionRepository.Table
                        orderby biss.CreatedOnUtc descending
                        where biss.UserId == userId &&
                              biss.ProductId == productId &&
                              biss.StoreId == storeId
                        select biss;

            var subscription = query.FirstOrDefault();
            return subscription;
        }

        /// <summary>
        /// Gets a subscription
        /// </summary>
        /// <param name="subscriptionId">Subscription identifier</param>
        /// <returns>Subscription</returns>
        public virtual BackInStockSubscription GetSubscriptionById(int subscriptionId)
        {
            if (subscriptionId == 0)
                return null;

            var subscription = _backInStockSubscriptionRepository.GetById(subscriptionId);
            return subscription;
        }

        /// <summary>
        /// Inserts subscription
        /// </summary>
        /// <param name="subscription">Subscription</param>
        public virtual void InsertSubscription(BackInStockSubscription subscription)
        {
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));

            _backInStockSubscriptionRepository.Insert(subscription);

            //event notification
            _eventPublisher.EntityInserted(subscription);
        }

        /// <summary>
        /// Updates subscription
        /// </summary>
        /// <param name="subscription">Subscription</param>
        public virtual void UpdateSubscription(BackInStockSubscription subscription)
        {
            if (subscription == null)
                throw new ArgumentNullException(nameof(subscription));

            _backInStockSubscriptionRepository.Update(subscription);

            //event notification
            _eventPublisher.EntityUpdated(subscription);
        }

        /// <summary>
        /// Send notification to subscribers
        /// </summary>
        /// <param name="product">Product</param>
        /// <returns>Number of sent email</returns>
        public virtual int SendNotificationsToSubscribers(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            var result = 0;
            var subscriptions = GetAllSubscriptionsByProductId(product.Id);
            foreach (var subscription in subscriptions)
            {
                //ensure that user is registered (simple and fast way)
                if (!CommonHelper.IsValidEmail(subscription.User.Email)) 
                    continue;

                var user = subscription.User;
                var userLanguageId = _genericAttributeService.GetAttribute<int>(user, AppUserDefaults.LanguageIdAttribute, subscription.StoreId);
                _workflowMessageService.SendBackInStockNotification(subscription, userLanguageId);
                result++;
            }

            for (var i = 0; i <= subscriptions.Count - 1; i++)
                DeleteSubscription(subscriptions[i]);

            return result;
        }

        #endregion
    }
}