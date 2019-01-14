using System;
using System.Linq;
using System.Net;
using System.Xml;
using Microsoft.AspNetCore.Http;
using Appulu.Core;
using Appulu.Core.Caching;
using Appulu.Core.Domain.Common;
using Appulu.Services.Configuration;
using Appulu.Web.Areas.Admin.Infrastructure.Cache;
using Appulu.Web.Areas.Admin.Models.Home;
using Appulu.Web.Framework.Mvc.Rss;

namespace Appulu.Web.Areas.Admin.Factories
{
    /// <summary>
    /// Represents the home models factory implementation
    /// </summary>
    public partial class HomeModelFactory : IHomeModelFactory
    {
        #region Constants

        /// <summary>
        /// Appulu news URL
        /// </summary>
        /// <remarks>
        /// {0} : Appulu version
        /// {1} : whether the store based is on the localhost
        /// {2} : whether advertisements are hidden
        /// {3} : store URL
        /// </remarks>
        private const string APPULU_NEWS_URL = "https://www.Appulu.com/NewsRSS.aspx?Version={0}&Localhost={1}&HideAdvertisements={2}&StoreURL={3}";

        #endregion

        #region Fields

        private readonly AdminAreaSettings _adminAreaSettings;
        private readonly ICommonModelFactory _commonModelFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOrderModelFactory _orderModelFactory;
        private readonly ISettingService _settingService;
        private readonly IStaticCacheManager _cacheManager;
        private readonly IWebHelper _webHelper;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public HomeModelFactory(AdminAreaSettings adminAreaSettings,
            ICommonModelFactory commonModelFactory,
            IHttpContextAccessor httpContextAccessor,
            IOrderModelFactory orderModelFactory,
            ISettingService settingService,
            IStaticCacheManager cacheManager,
            IWebHelper webHelper,
            IWorkContext workContext)
        {
            this._adminAreaSettings = adminAreaSettings;
            this._commonModelFactory = commonModelFactory;
            this._httpContextAccessor = httpContextAccessor;
            this._orderModelFactory = orderModelFactory;
            this._settingService = settingService;
            this._cacheManager = cacheManager;
            this._webHelper = webHelper;
            this._workContext = workContext;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare dashboard model
        /// </summary>
        /// <param name="model">Dashboard model</param>
        /// <returns>Dashboard model</returns>
        public virtual DashboardModel PrepareDashboardModel(DashboardModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            model.IsLoggedInAsVendor = _workContext.CurrentVendor != null;

            //prepare nested search models
            _commonModelFactory.PreparePopularSearchTermSearchModel(model.PopularSearchTerms);
            _orderModelFactory.PrepareBestsellerBriefSearchModel(model.BestsellersByAmount);
            _orderModelFactory.PrepareBestsellerBriefSearchModel(model.BestsellersByQuantity);

            return model;
        }

        /// <summary>
        /// Prepare Appulu news model
        /// </summary>
        /// <returns>Appulu news model</returns>
        public virtual AppuluNewsModel PrepareAppuluNewsModel()
        {
            var model = new AppuluNewsModel
            {
                HideAdvertisements = _adminAreaSettings.HideAdvertisementsOnAdminArea
            };

            var rssData = _cacheManager.Get(ModelCacheEventConsumer.OFFICIAL_NEWS_MODEL_KEY, () =>
            {
                //compose Appulu news RSS feed URL
                var appuluNewsUrl = string.Format(APPULU_NEWS_URL,
                    AppVersion.CurrentVersion,
                    _webHelper.IsLocalRequest(_httpContextAccessor.HttpContext.Request),
                    _adminAreaSettings.HideAdvertisementsOnAdminArea,
                    _webHelper.GetStoreLocation()).ToLowerInvariant();

                //create request
                var request = WebRequest.Create(appuluNewsUrl);

                //specify request timeout
                request.Timeout = 3000;

                //try to get Appulu news RSS feed
                using (var response = request.GetResponse())
                    using (var reader = XmlReader.Create(response.GetResponseStream()))
                        return RssFeed.Load(reader);
            });

            for (var i = 0; i < rssData.Items.Count; i++)
            {
                var item = rssData.Items.ElementAt(i);
                var newsItem = new AppuluNewsDetailsModel
                {
                    Title = item.TitleText,
                    Summary = item.ContentText,
                    Url = item.Url.OriginalString,
                    PublishDate = item.PublishDate
                };
                model.Items.Add(newsItem);

                //has new items?
                if (i == 0)
                {
                    var firstRequest = string.IsNullOrEmpty(_adminAreaSettings.LastNewsTitleAdminArea);
                    if (_adminAreaSettings.LastNewsTitleAdminArea != newsItem.Title)
                    {
                        _adminAreaSettings.LastNewsTitleAdminArea = newsItem.Title;
                        _settingService.SaveSetting(_adminAreaSettings);

                        if (!firstRequest)
                        {
                            //new item
                            model.HasNewItems = true;
                        }
                    }
                }
            }

            return model;
        }

        #endregion
    }
}