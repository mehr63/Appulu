using Appulu.Core.Caching;
using Appulu.Core.Domain.Users;
using Appulu.Services.Events;

namespace Appulu.Services.Users.Cache
{
    /// <summary>
    /// User cache event consumer (used for caching of current user password)
    /// </summary>
    public partial class UserCacheEventConsumer : IConsumer<UserPasswordChangedEvent>
    {
        #region Fields

        private readonly IStaticCacheManager _cacheManager;

        #endregion

        #region Ctor

        public UserCacheEventConsumer(IStaticCacheManager cacheManager)
        {
            this._cacheManager = cacheManager;
        }

        #endregion

        #region Methods

        //password changed
        public void HandleEvent(UserPasswordChangedEvent eventMessage)
        {
            _cacheManager.Remove(string.Format(AppUserServiceDefaults.UserPasswordLifetimeCacheKey, eventMessage.Password.UserId));
        }

        #endregion
    }
}