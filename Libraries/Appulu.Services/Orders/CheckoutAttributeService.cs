using System;
using System.Collections.Generic;
using System.Linq;
using Appulu.Core.Caching;
using Appulu.Core.Data;
using Appulu.Core.Domain.Orders;
using Appulu.Services.Events;
using Appulu.Services.Stores;

namespace Appulu.Services.Orders
{
    /// <summary>
    /// Checkout attribute service
    /// </summary>
    public partial class CheckoutAttributeService : ICheckoutAttributeService
    {
        #region Fields

        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly IRepository<CheckoutAttribute> _checkoutAttributeRepository;
        private readonly IRepository<CheckoutAttributeValue> _checkoutAttributeValueRepository;
        private readonly IStoreMappingService _storeMappingService;

        #endregion

        #region Ctor

        public CheckoutAttributeService(ICacheManager cacheManager,
            IEventPublisher eventPublisher,
            IRepository<CheckoutAttribute> checkoutAttributeRepository,
            IRepository<CheckoutAttributeValue> checkoutAttributeValueRepository,
            IStoreMappingService storeMappingService)
        {
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
            this._checkoutAttributeRepository = checkoutAttributeRepository;
            this._checkoutAttributeValueRepository = checkoutAttributeValueRepository;
            this._storeMappingService = storeMappingService;
        }

        #endregion

        #region Methods

        #region Checkout attributes

        /// <summary>
        /// Deletes a checkout attribute
        /// </summary>
        /// <param name="checkoutAttribute">Checkout attribute</param>
        public virtual void DeleteCheckoutAttribute(CheckoutAttribute checkoutAttribute)
        {
            if (checkoutAttribute == null)
                throw new ArgumentNullException(nameof(checkoutAttribute));

            _checkoutAttributeRepository.Delete(checkoutAttribute);

            _cacheManager.RemoveByPattern(AppOrderDefaults.CheckoutAttributesPatternCacheKey);
            _cacheManager.RemoveByPattern(AppOrderDefaults.CheckoutAttributeValuesPatternCacheKey);

            //event notification
            _eventPublisher.EntityDeleted(checkoutAttribute);
        }

        /// <summary>
        /// Gets all checkout attributes
        /// </summary>
        /// <param name="storeId">Store identifier</param>
        /// <param name="excludeShippableAttributes">A value indicating whether we should exclude shippable attributes</param>
        /// <returns>Checkout attributes</returns>
        public virtual IList<CheckoutAttribute> GetAllCheckoutAttributes(int storeId = 0, bool excludeShippableAttributes = false)
        {
            var key = string.Format(AppOrderDefaults.CheckoutAttributesAllCacheKey, storeId, excludeShippableAttributes);
            return _cacheManager.Get(key, () =>
            {
                var query = from ca in _checkoutAttributeRepository.Table
                            orderby ca.DisplayOrder, ca.Id
                            select ca;
                var checkoutAttributes = query.ToList();
                if (storeId > 0)
                {
                    //store mapping
                    checkoutAttributes = checkoutAttributes.Where(ca => _storeMappingService.Authorize(ca)).ToList();
                }

                if (excludeShippableAttributes)
                {
                    //remove attributes which require shippable products
                    checkoutAttributes = checkoutAttributes.Where(x => !x.ShippableProductRequired).ToList();
                }

                return checkoutAttributes;
            });
        }

        /// <summary>
        /// Gets a checkout attribute 
        /// </summary>
        /// <param name="checkoutAttributeId">Checkout attribute identifier</param>
        /// <returns>Checkout attribute</returns>
        public virtual CheckoutAttribute GetCheckoutAttributeById(int checkoutAttributeId)
        {
            if (checkoutAttributeId == 0)
                return null;

            var key = string.Format(AppOrderDefaults.CheckoutAttributesByIdCacheKey, checkoutAttributeId);
            return _cacheManager.Get(key, () => _checkoutAttributeRepository.GetById(checkoutAttributeId));
        }

        /// <summary>
        /// Inserts a checkout attribute
        /// </summary>
        /// <param name="checkoutAttribute">Checkout attribute</param>
        public virtual void InsertCheckoutAttribute(CheckoutAttribute checkoutAttribute)
        {
            if (checkoutAttribute == null)
                throw new ArgumentNullException(nameof(checkoutAttribute));

            _checkoutAttributeRepository.Insert(checkoutAttribute);

            _cacheManager.RemoveByPattern(AppOrderDefaults.CheckoutAttributesPatternCacheKey);
            _cacheManager.RemoveByPattern(AppOrderDefaults.CheckoutAttributeValuesPatternCacheKey);

            //event notification
            _eventPublisher.EntityInserted(checkoutAttribute);
        }

        /// <summary>
        /// Updates the checkout attribute
        /// </summary>
        /// <param name="checkoutAttribute">Checkout attribute</param>
        public virtual void UpdateCheckoutAttribute(CheckoutAttribute checkoutAttribute)
        {
            if (checkoutAttribute == null)
                throw new ArgumentNullException(nameof(checkoutAttribute));

            _checkoutAttributeRepository.Update(checkoutAttribute);

            _cacheManager.RemoveByPattern(AppOrderDefaults.CheckoutAttributesPatternCacheKey);
            _cacheManager.RemoveByPattern(AppOrderDefaults.CheckoutAttributeValuesPatternCacheKey);

            //event notification
            _eventPublisher.EntityUpdated(checkoutAttribute);
        }

        #endregion

        #region Checkout attribute values

        /// <summary>
        /// Deletes a checkout attribute value
        /// </summary>
        /// <param name="checkoutAttributeValue">Checkout attribute value</param>
        public virtual void DeleteCheckoutAttributeValue(CheckoutAttributeValue checkoutAttributeValue)
        {
            if (checkoutAttributeValue == null)
                throw new ArgumentNullException(nameof(checkoutAttributeValue));

            _checkoutAttributeValueRepository.Delete(checkoutAttributeValue);

            _cacheManager.RemoveByPattern(AppOrderDefaults.CheckoutAttributesPatternCacheKey);
            _cacheManager.RemoveByPattern(AppOrderDefaults.CheckoutAttributeValuesPatternCacheKey);

            //event notification
            _eventPublisher.EntityDeleted(checkoutAttributeValue);
        }

        /// <summary>
        /// Gets checkout attribute values by checkout attribute identifier
        /// </summary>
        /// <param name="checkoutAttributeId">The checkout attribute identifier</param>
        /// <returns>Checkout attribute values</returns>
        public virtual IList<CheckoutAttributeValue> GetCheckoutAttributeValues(int checkoutAttributeId)
        {
            var key = string.Format(AppOrderDefaults.CheckoutAttributeValuesAllCacheKey, checkoutAttributeId);
            return _cacheManager.Get(key, () =>
            {
                var query = from cav in _checkoutAttributeValueRepository.Table
                            orderby cav.DisplayOrder, cav.Id
                            where cav.CheckoutAttributeId == checkoutAttributeId
                            select cav;
                var checkoutAttributeValues = query.ToList();
                return checkoutAttributeValues;
            });
        }

        /// <summary>
        /// Gets a checkout attribute value
        /// </summary>
        /// <param name="checkoutAttributeValueId">Checkout attribute value identifier</param>
        /// <returns>Checkout attribute value</returns>
        public virtual CheckoutAttributeValue GetCheckoutAttributeValueById(int checkoutAttributeValueId)
        {
            if (checkoutAttributeValueId == 0)
                return null;

            var key = string.Format(AppOrderDefaults.CheckoutAttributeValuesByIdCacheKey, checkoutAttributeValueId);
            return _cacheManager.Get(key, () => _checkoutAttributeValueRepository.GetById(checkoutAttributeValueId));
        }

        /// <summary>
        /// Inserts a checkout attribute value
        /// </summary>
        /// <param name="checkoutAttributeValue">Checkout attribute value</param>
        public virtual void InsertCheckoutAttributeValue(CheckoutAttributeValue checkoutAttributeValue)
        {
            if (checkoutAttributeValue == null)
                throw new ArgumentNullException(nameof(checkoutAttributeValue));

            _checkoutAttributeValueRepository.Insert(checkoutAttributeValue);

            _cacheManager.RemoveByPattern(AppOrderDefaults.CheckoutAttributesPatternCacheKey);
            _cacheManager.RemoveByPattern(AppOrderDefaults.CheckoutAttributeValuesPatternCacheKey);

            //event notification
            _eventPublisher.EntityInserted(checkoutAttributeValue);
        }

        /// <summary>
        /// Updates the checkout attribute value
        /// </summary>
        /// <param name="checkoutAttributeValue">Checkout attribute value</param>
        public virtual void UpdateCheckoutAttributeValue(CheckoutAttributeValue checkoutAttributeValue)
        {
            if (checkoutAttributeValue == null)
                throw new ArgumentNullException(nameof(checkoutAttributeValue));

            _checkoutAttributeValueRepository.Update(checkoutAttributeValue);

            _cacheManager.RemoveByPattern(AppOrderDefaults.CheckoutAttributesPatternCacheKey);
            _cacheManager.RemoveByPattern(AppOrderDefaults.CheckoutAttributeValuesPatternCacheKey);

            //event notification
            _eventPublisher.EntityUpdated(checkoutAttributeValue);
        }

        #endregion

        #endregion
    }
}