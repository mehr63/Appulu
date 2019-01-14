using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Appulu.Core;
using Appulu.Core.Domain.Users;
using Appulu.Core.Domain.Stores;
using Appulu.Core.Infrastructure;
using Appulu.Services.Common;
using Appulu.Services.Stores;

namespace Appulu.Web.Framework
{
    /// <summary>
    /// Store context for web application
    /// </summary>
    public partial class WebStoreContext : IStoreContext
    {
        #region Fields

        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IStoreService _storeService;

        private Store _cachedStore;
        private int? _cachedActiveStoreScopeConfiguration;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="genericAttributeService">Generic attribute service</param>
        /// <param name="httpContextAccessor">HTTP context accessor</param>
        /// <param name="storeService">Store service</param>
        public WebStoreContext(IGenericAttributeService genericAttributeService,
            IHttpContextAccessor httpContextAccessor,
            IStoreService storeService)
        {
            this._genericAttributeService = genericAttributeService;
            this._httpContextAccessor = httpContextAccessor;
            this._storeService = storeService;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the current store
        /// </summary>
        public virtual Store CurrentStore
        {
            get
            {
                if (_cachedStore != null)
                    return _cachedStore;

                //try to determine the current store by HOST header
                string host = _httpContextAccessor.HttpContext?.Request?.Headers[HeaderNames.Host];

                var allStores = _storeService.GetAllStores();
                var store = allStores.FirstOrDefault(s => _storeService.ContainsHostValue(s, host));

                if (store == null)
                {
                    //load the first found store
                    store = allStores.FirstOrDefault();
                }

                _cachedStore = store ?? throw new Exception("No store could be loaded");

                return _cachedStore;
            }
        }

        /// <summary>
        /// Gets active store scope configuration
        /// </summary>
        public virtual int ActiveStoreScopeConfiguration
        {
            get
            {
                if (_cachedActiveStoreScopeConfiguration.HasValue)
                    return _cachedActiveStoreScopeConfiguration.Value;

                //ensure that we have 2 (or more) stores
                if (_storeService.GetAllStores().Count > 1)
                {
                    //do not inject IWorkContext via constructor because it'll cause circular references
                    var currentUser = EngineContext.Current.Resolve<IWorkContext>().CurrentUser;

                    //try to get store identifier from attributes
                    var storeId = _genericAttributeService
                        .GetAttribute<int>(currentUser, AppUserDefaults.AdminAreaStoreScopeConfigurationAttribute);

                    _cachedActiveStoreScopeConfiguration = _storeService.GetStoreById(storeId)?.Id ?? 0;
                }
                else
                    _cachedActiveStoreScopeConfiguration = 0;

                return _cachedActiveStoreScopeConfiguration ?? 0;
            }
        }

        #endregion
    }
}