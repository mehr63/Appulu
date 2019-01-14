using Appulu.Core.Caching;
using Appulu.Core.Domain.Configuration;
using Appulu.Core.Events;
using Appulu.Services.Events;

namespace Appulu.Plugin.Widgets.NivoSlider.Infrastructure.Cache
{
    /// <summary>
    /// Model cache event consumer (used for caching of presentation layer models)
    /// </summary>
    public partial class ModelCacheEventConsumer :
        IConsumer<EntityInsertedEvent<Setting>>,
        IConsumer<EntityUpdatedEvent<Setting>>,
        IConsumer<EntityDeletedEvent<Setting>>
    {
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : picture id
        /// </remarks>
        public const string PICTURE_URL_MODEL_KEY = "App.plugins.widgets.nivoslider.pictureurl-{0}";
        public const string PICTURE_URL_PATTERN_KEY = "App.plugins.widgets.nivoslider";

        private readonly IStaticCacheManager _cacheManager;

        public ModelCacheEventConsumer(IStaticCacheManager cacheManager)
        {
            this._cacheManager = cacheManager;
        }

        public void HandleEvent(EntityInsertedEvent<Setting> eventMessage)
        {
            _cacheManager.RemoveByPattern(PICTURE_URL_PATTERN_KEY);
        }
        public void HandleEvent(EntityUpdatedEvent<Setting> eventMessage)
        {
            _cacheManager.RemoveByPattern(PICTURE_URL_PATTERN_KEY);
        }
        public void HandleEvent(EntityDeletedEvent<Setting> eventMessage)
        {
            _cacheManager.RemoveByPattern(PICTURE_URL_PATTERN_KEY);
        }
    }
}