using Appulu.Core.Caching;
using Appulu.Core.Domain.Catalog;
using Appulu.Core.Domain.Configuration;
using Appulu.Core.Domain.Discounts;
using Appulu.Core.Events;
using Appulu.Services.Events;

namespace Appulu.Services.Discounts.Cache
{
    /// <summary>
    /// Cache event consumer (used for caching of discounts)
    /// </summary>
    public partial class DiscountEventConsumer :
        //discounts
        IConsumer<EntityInsertedEvent<Discount>>,
        IConsumer<EntityUpdatedEvent<Discount>>,
        IConsumer<EntityDeletedEvent<Discount>>,
        //discount requirements
        IConsumer<EntityInsertedEvent<DiscountRequirement>>,
        IConsumer<EntityUpdatedEvent<DiscountRequirement>>,
        IConsumer<EntityDeletedEvent<DiscountRequirement>>,
        //settings
        IConsumer<EntityUpdatedEvent<Setting>>,
        //categories
        IConsumer<EntityInsertedEvent<Category>>,
        IConsumer<EntityUpdatedEvent<Category>>,
        IConsumer<EntityDeletedEvent<Category>>,
        //manufacturers
        IConsumer<EntityInsertedEvent<Manufacturer>>,
        IConsumer<EntityUpdatedEvent<Manufacturer>>,
        IConsumer<EntityDeletedEvent<Manufacturer>>
    {
        #region Fields

        private readonly IStaticCacheManager _cacheManager;

        #endregion

        #region Ctor

        public DiscountEventConsumer(IStaticCacheManager cacheManager)
        {
            this._cacheManager = cacheManager;
        }

        #endregion

        #region Methods

        #region Discounts

        public void HandleEvent(EntityInsertedEvent<Discount> eventMessage)
        {
            _cacheManager.RemoveByPattern(AppDiscountDefaults.DiscountAllPatternCacheKey);
            _cacheManager.RemoveByPattern(AppDiscountDefaults.DiscountRequirementPatternCacheKey);
            _cacheManager.RemoveByPattern(AppDiscountDefaults.DiscountCategoryIdsPatternCacheKey);
            _cacheManager.RemoveByPattern(AppDiscountDefaults.DiscountManufacturerIdsPatternCacheKey);
        }

        public void HandleEvent(EntityUpdatedEvent<Discount> eventMessage)
        {
            _cacheManager.RemoveByPattern(AppDiscountDefaults.DiscountAllPatternCacheKey);
            _cacheManager.RemoveByPattern(AppDiscountDefaults.DiscountRequirementPatternCacheKey);
            _cacheManager.RemoveByPattern(AppDiscountDefaults.DiscountCategoryIdsPatternCacheKey);
            _cacheManager.RemoveByPattern(AppDiscountDefaults.DiscountManufacturerIdsPatternCacheKey);
        }

        public void HandleEvent(EntityDeletedEvent<Discount> eventMessage)
        {
            _cacheManager.RemoveByPattern(AppDiscountDefaults.DiscountAllPatternCacheKey);
            _cacheManager.RemoveByPattern(AppDiscountDefaults.DiscountRequirementPatternCacheKey);
            _cacheManager.RemoveByPattern(AppDiscountDefaults.DiscountCategoryIdsPatternCacheKey);
            _cacheManager.RemoveByPattern(AppDiscountDefaults.DiscountManufacturerIdsPatternCacheKey);
        }

        #endregion

        #region Discount requirements

        public void HandleEvent(EntityInsertedEvent<DiscountRequirement> eventMessage)
        {
            _cacheManager.RemoveByPattern(AppDiscountDefaults.DiscountRequirementPatternCacheKey);
        }

        public void HandleEvent(EntityUpdatedEvent<DiscountRequirement> eventMessage)
        {
            _cacheManager.RemoveByPattern(AppDiscountDefaults.DiscountRequirementPatternCacheKey);
        }

        public void HandleEvent(EntityDeletedEvent<DiscountRequirement> eventMessage)
        {
            _cacheManager.RemoveByPattern(AppDiscountDefaults.DiscountRequirementPatternCacheKey);
        }

        #endregion

        #region Settings

        public void HandleEvent(EntityUpdatedEvent<Setting> eventMessage)
        {
            _cacheManager.RemoveByPattern(AppDiscountDefaults.DiscountCategoryIdsPatternCacheKey);
            _cacheManager.RemoveByPattern(AppDiscountDefaults.DiscountManufacturerIdsPatternCacheKey);
        }

        #endregion

        #region Categories

        public void HandleEvent(EntityInsertedEvent<Category> eventMessage)
        {
            _cacheManager.RemoveByPattern(AppDiscountDefaults.DiscountCategoryIdsPatternCacheKey);
        }

        public void HandleEvent(EntityUpdatedEvent<Category> eventMessage)
        {
            _cacheManager.RemoveByPattern(AppDiscountDefaults.DiscountCategoryIdsPatternCacheKey);
        }

        public void HandleEvent(EntityDeletedEvent<Category> eventMessage)
        {
            _cacheManager.RemoveByPattern(AppDiscountDefaults.DiscountCategoryIdsPatternCacheKey);
        }

        #endregion

        #region Manufacturers

        public void HandleEvent(EntityInsertedEvent<Manufacturer> eventMessage)
        {
            _cacheManager.RemoveByPattern(AppDiscountDefaults.DiscountManufacturerIdsPatternCacheKey);
        }

        public void HandleEvent(EntityUpdatedEvent<Manufacturer> eventMessage)
        {
            _cacheManager.RemoveByPattern(AppDiscountDefaults.DiscountManufacturerIdsPatternCacheKey);
        }

        public void HandleEvent(EntityDeletedEvent<Manufacturer> eventMessage)
        {
            _cacheManager.RemoveByPattern(AppDiscountDefaults.DiscountManufacturerIdsPatternCacheKey);
        }

        #endregion

        #endregion
    }
}