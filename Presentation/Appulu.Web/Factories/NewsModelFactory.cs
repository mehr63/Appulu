using System;
using System.Linq;
using Appulu.Core;
using Appulu.Core.Caching;
using Appulu.Core.Domain.Users;
using Appulu.Core.Domain.Media;
using Appulu.Core.Domain.News;
using Appulu.Core.Domain.Security;
using Appulu.Services.Common;
using Appulu.Services.Users;
using Appulu.Services.Helpers;
using Appulu.Services.Media;
using Appulu.Services.News;
using Appulu.Services.Seo;
using Appulu.Web.Infrastructure.Cache;
using Appulu.Web.Models.News;

namespace Appulu.Web.Factories
{
    /// <summary>
    /// Represents the news model factory
    /// </summary>
    public partial class NewsModelFactory : INewsModelFactory
    {
        #region Fields

        private readonly CaptchaSettings _captchaSettings;
        private readonly UserSettings _userSettings;
        private readonly IUserService _userService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly INewsService _newsService;
        private readonly IPictureService _pictureService;
        private readonly IStaticCacheManager _cacheManager;
        private readonly IStoreContext _storeContext;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IWorkContext _workContext;
        private readonly MediaSettings _mediaSettings;
        private readonly NewsSettings _newsSettings;

        #endregion

        #region Ctor

        public NewsModelFactory(CaptchaSettings captchaSettings,
            UserSettings userSettings,
            IUserService userService,
            IDateTimeHelper dateTimeHelper,
            IGenericAttributeService genericAttributeService,
            INewsService newsService,
            IPictureService pictureService,
            IStaticCacheManager cacheManager,
            IStoreContext storeContext,
            IUrlRecordService urlRecordService,
            IWorkContext workContext,
            MediaSettings mediaSettings,
            NewsSettings newsSettings)
        {
            this._captchaSettings = captchaSettings;
            this._userSettings = userSettings;
            this._userService = userService;
            this._dateTimeHelper = dateTimeHelper;
            this._genericAttributeService = genericAttributeService;
            this._newsService = newsService;
            this._pictureService = pictureService;
            this._cacheManager = cacheManager;
            this._storeContext = storeContext;
            this._urlRecordService = urlRecordService;
            this._workContext = workContext;
            this._mediaSettings = mediaSettings;
            this._newsSettings = newsSettings;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare the news comment model
        /// </summary>
        /// <param name="newsComment">News comment</param>
        /// <returns>News comment model</returns>
        public virtual NewsCommentModel PrepareNewsCommentModel(NewsComment newsComment)
        {
            if (newsComment == null)
                throw new ArgumentNullException(nameof(newsComment));

            var model = new NewsCommentModel
            {
                Id = newsComment.Id,
                UserId = newsComment.UserId,
                UserName = _userService.FormatUserName(newsComment.User),
                CommentTitle = newsComment.CommentTitle,
                CommentText = newsComment.CommentText,
                CreatedOn = _dateTimeHelper.ConvertToUserTime(newsComment.CreatedOnUtc, DateTimeKind.Utc),
                AllowViewingProfiles = _userSettings.AllowViewingProfiles && newsComment.User != null && !newsComment.User.IsGuest(),
            };
            if (_userSettings.AllowUsersToUploadAvatars)
            {
                model.UserAvatarUrl = _pictureService.GetPictureUrl(
                    _genericAttributeService.GetAttribute<int>(newsComment.User, AppUserDefaults.AvatarPictureIdAttribute),
                    _mediaSettings.AvatarPictureSize, _userSettings.DefaultAvatarEnabled, defaultPictureType: PictureType.Avatar);
            }

            return model;
        }

        /// <summary>
        /// Prepare the news item model
        /// </summary>
        /// <param name="model">News item model</param>
        /// <param name="newsItem">News item</param>
        /// <param name="prepareComments">Whether to prepare news comment models</param>
        /// <returns>News item model</returns>
        public virtual NewsItemModel PrepareNewsItemModel(NewsItemModel model, NewsItem newsItem, bool prepareComments)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (newsItem == null)
                throw new ArgumentNullException(nameof(newsItem));

            model.Id = newsItem.Id;
            model.MetaTitle = newsItem.MetaTitle;
            model.MetaDescription = newsItem.MetaDescription;
            model.MetaKeywords = newsItem.MetaKeywords;
            model.SeName = _urlRecordService.GetSeName(newsItem, newsItem.LanguageId, ensureTwoPublishedLanguages: false);
            model.Title = newsItem.Title;
            model.Short = newsItem.Short;
            model.Full = newsItem.Full;
            model.AllowComments = newsItem.AllowComments;
            model.CreatedOn = _dateTimeHelper.ConvertToUserTime(newsItem.StartDateUtc ?? newsItem.CreatedOnUtc, DateTimeKind.Utc);
            model.AddNewComment.DisplayCaptcha = _captchaSettings.Enabled && _captchaSettings.ShowOnNewsCommentPage;

            //number of news comments
            var storeId = _newsSettings.ShowNewsCommentsPerStore ? _storeContext.CurrentStore.Id : 0;
            var cacheKey = string.Format(ModelCacheEventConsumer.NEWS_COMMENTS_NUMBER_KEY, newsItem.Id, storeId, true);
            model.NumberOfComments = _cacheManager.Get(cacheKey, () => _newsService.GetNewsCommentsCount(newsItem, storeId, true));

            if (prepareComments)
            {
                var newsComments = newsItem.NewsComments.Where(comment => comment.IsApproved);
                if (_newsSettings.ShowNewsCommentsPerStore)
                    newsComments = newsComments.Where(comment => comment.StoreId == _storeContext.CurrentStore.Id);

                foreach (var nc in newsComments.OrderBy(comment => comment.CreatedOnUtc))
                {
                    var commentModel = PrepareNewsCommentModel(nc);
                    model.Comments.Add(commentModel);
                }
            }

            return model;
        }

        /// <summary>
        /// Prepare the home page news items model
        /// </summary>
        /// <returns>Home page news items model</returns>
        public virtual HomePageNewsItemsModel PrepareHomePageNewsItemsModel()
        {
            var cacheKey = string.Format(ModelCacheEventConsumer.HOMEPAGE_NEWSMODEL_KEY, _workContext.WorkingLanguage.Id, _storeContext.CurrentStore.Id);
            var cachedModel = _cacheManager.Get(cacheKey, () =>
            {
                var newsItems = _newsService.GetAllNews(_workContext.WorkingLanguage.Id, _storeContext.CurrentStore.Id, 0, _newsSettings.MainPageNewsCount);
                return new HomePageNewsItemsModel
                {
                    WorkingLanguageId = _workContext.WorkingLanguage.Id,
                    NewsItems = newsItems
                        .Select(x =>
                        {
                            var newsModel = new NewsItemModel();
                            PrepareNewsItemModel(newsModel, x, false);
                            return newsModel;
                        })
                        .ToList()
                };
            });

            //"Comments" property of "NewsItemModel" object depends on the current user.
            //Furthermore, we just don't need it for home page news. So let's reset it.
            //But first we need to clone the cached model (the updated one should not be cached)
            var model = (HomePageNewsItemsModel)cachedModel.Clone();
            foreach (var newsItemModel in model.NewsItems)
                newsItemModel.Comments.Clear();
            return model;
        }

        /// <summary>
        /// Prepare the news item list model
        /// </summary>
        /// <param name="command">News paging filtering model</param>
        /// <returns>News item list model</returns>
        public virtual NewsItemListModel PrepareNewsItemListModel(NewsPagingFilteringModel command)
        {
            var model = new NewsItemListModel
            {
                WorkingLanguageId = _workContext.WorkingLanguage.Id
            };

            if (command.PageSize <= 0) command.PageSize = _newsSettings.NewsArchivePageSize;
            if (command.PageNumber <= 0) command.PageNumber = 1;

            var newsItems = _newsService.GetAllNews(_workContext.WorkingLanguage.Id, _storeContext.CurrentStore.Id,
                command.PageNumber - 1, command.PageSize);
            model.PagingFilteringContext.LoadPagedList(newsItems);

            model.NewsItems = newsItems
                .Select(x =>
                {
                    var newsModel = new NewsItemModel();
                    PrepareNewsItemModel(newsModel, x, false);
                    return newsModel;
                })
                .ToList();

            return model;
        }

        #endregion
    }
}