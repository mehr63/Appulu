using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Appulu.Core;
using Appulu.Core.Domain.Users;
using Appulu.Core.Domain.Localization;
using Appulu.Core.Domain.News;
using Appulu.Core.Domain.Security;
using Appulu.Services.Events;
using Appulu.Services.Localization;
using Appulu.Services.Logging;
using Appulu.Services.Messages;
using Appulu.Services.News;
using Appulu.Services.Security;
using Appulu.Services.Seo;
using Appulu.Web.Factories;
using Appulu.Web.Framework;
using Appulu.Web.Framework.Controllers;
using Appulu.Web.Framework.Mvc;
using Appulu.Web.Framework.Mvc.Filters;
using Appulu.Web.Framework.Mvc.Rss;
using Appulu.Web.Framework.Security;
using Appulu.Web.Framework.Security.Captcha;
using Appulu.Web.Models.News;

namespace Appulu.Web.Controllers
{
    [HttpsRequirement(SslRequirement.No)]
    public partial class NewsController : BasePublicController
    {
        #region Fields

        private readonly CaptchaSettings _captchaSettings;
        private readonly IUserActivityService _userActivityService;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILocalizationService _localizationService;
        private readonly INewsModelFactory _newsModelFactory;
        private readonly INewsService _newsService;
        private readonly IPermissionService _permissionService;
        private readonly IStoreContext _storeContext;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IWebHelper _webHelper;
        private readonly IWorkContext _workContext;
        private readonly IWorkflowMessageService _workflowMessageService;
        private readonly LocalizationSettings _localizationSettings;
        private readonly NewsSettings _newsSettings;

        #endregion

        #region Ctor

        public NewsController(CaptchaSettings captchaSettings,
            IUserActivityService userActivityService,
            IEventPublisher eventPublisher,
            ILocalizationService localizationService,
            INewsModelFactory newsModelFactory,
            INewsService newsService,
            IPermissionService permissionService,
            IStoreContext storeContext,
            IUrlRecordService urlRecordService,
            IWebHelper webHelper,
            IWorkContext workContext,
            IWorkflowMessageService workflowMessageService,
            LocalizationSettings localizationSettings,
            NewsSettings newsSettings)
        {
            this._captchaSettings = captchaSettings;
            this._userActivityService = userActivityService;
            this._eventPublisher = eventPublisher;
            this._localizationService = localizationService;
            this._newsModelFactory = newsModelFactory;
            this._newsService = newsService;
            this._permissionService = permissionService;
            this._storeContext = storeContext;
            this._urlRecordService = urlRecordService;
            this._webHelper = webHelper;
            this._workContext = workContext;
            this._workflowMessageService = workflowMessageService;
            this._localizationSettings = localizationSettings;
            this._newsSettings = newsSettings;
        }

        #endregion

        #region Methods

        public virtual IActionResult List(NewsPagingFilteringModel command)
        {
            if (!_newsSettings.Enabled)
                return RedirectToRoute("HomePage");

            var model = _newsModelFactory.PrepareNewsItemListModel(command);
            return View(model);
        }

        public virtual IActionResult ListRss(int languageId)
        {
            var feed = new RssFeed(
                $"{_localizationService.GetLocalized(_storeContext.CurrentStore, x => x.Name)}: News",
                "News",
                new Uri(_webHelper.GetStoreLocation()),
                DateTime.UtcNow);

            if (!_newsSettings.Enabled)
                return new RssActionResult(feed, _webHelper.GetThisPageUrl(false));

            var items = new List<RssItem>();
            var newsItems = _newsService.GetAllNews(languageId, _storeContext.CurrentStore.Id);
            foreach (var n in newsItems)
            {
                var newsUrl = Url.RouteUrl("NewsItem", new { SeName = _urlRecordService.GetSeName(n, n.LanguageId, ensureTwoPublishedLanguages: false) }, _webHelper.CurrentRequestProtocol);
                items.Add(new RssItem(n.Title, n.Short, new Uri(newsUrl), $"urn:store:{_storeContext.CurrentStore.Id}:news:blog:{n.Id}", n.CreatedOnUtc));
            }
            feed.Items = items;
            return new RssActionResult(feed, _webHelper.GetThisPageUrl(false));
        }

        public virtual IActionResult NewsItem(int newsItemId)
        {
            if (!_newsSettings.Enabled)
                return RedirectToRoute("HomePage");

            var newsItem = _newsService.GetNewsById(newsItemId);
            if (newsItem == null)
                return RedirectToRoute("HomePage");

            var hasAdminAccess = _permissionService.Authorize(StandardPermissionProvider.AccessAdminPanel) && _permissionService.Authorize(StandardPermissionProvider.ManageNews);
            //access to News preview
            if ((!newsItem.Published || !_newsService.IsNewsAvailable(newsItem)) && !hasAdminAccess)
                return RedirectToRoute("HomePage");

            var model = new NewsItemModel();
            model = _newsModelFactory.PrepareNewsItemModel(model, newsItem, true);

            //display "edit" (manage) link
            if (hasAdminAccess)
                DisplayEditLink(Url.Action("Edit", "News", new { id = newsItem.Id, area = AreaNames.Admin }));

            return View(model);
        }

        [HttpPost, ActionName("NewsItem")]
        [PublicAntiForgery]
        [FormValueRequired("add-comment")]
        [ValidateCaptcha]
        public virtual IActionResult NewsCommentAdd(int newsItemId, NewsItemModel model, bool captchaValid)
        {
            if (!_newsSettings.Enabled)
                return RedirectToRoute("HomePage");

            var newsItem = _newsService.GetNewsById(newsItemId);
            if (newsItem == null || !newsItem.Published || !newsItem.AllowComments)
                return RedirectToRoute("HomePage");

            //validate CAPTCHA
            if (_captchaSettings.Enabled && _captchaSettings.ShowOnNewsCommentPage && !captchaValid)
            {
                ModelState.AddModelError("", _captchaSettings.GetWrongCaptchaMessage(_localizationService));
            }

            if (_workContext.CurrentUser.IsGuest() && !_newsSettings.AllowNotRegisteredUsersToLeaveComments)
            {
                ModelState.AddModelError("", _localizationService.GetResource("News.Comments.OnlyRegisteredUsersLeaveComments"));
            }

            if (ModelState.IsValid)
            {
                var comment = new NewsComment
                {
                    NewsItemId = newsItem.Id,
                    UserId = _workContext.CurrentUser.Id,
                    CommentTitle = model.AddNewComment.CommentTitle,
                    CommentText = model.AddNewComment.CommentText,
                    IsApproved = !_newsSettings.NewsCommentsMustBeApproved,
                    StoreId = _storeContext.CurrentStore.Id,
                    CreatedOnUtc = DateTime.UtcNow,
                };
                newsItem.NewsComments.Add(comment);
                _newsService.UpdateNews(newsItem);

                //notify a store owner;
                if (_newsSettings.NotifyAboutNewNewsComments)
                    _workflowMessageService.SendNewsCommentNotificationMessage(comment, _localizationSettings.DefaultAdminLanguageId);

                //activity log
                _userActivityService.InsertActivity("PublicStore.AddNewsComment",
                    _localizationService.GetResource("ActivityLog.PublicStore.AddNewsComment"), comment);

                //raise event
                if (comment.IsApproved)
                    _eventPublisher.Publish(new NewsCommentApprovedEvent(comment));

                //The text boxes should be cleared after a comment has been posted
                //That' why we reload the page
                TempData["app.news.addcomment.result"] = comment.IsApproved
                    ? _localizationService.GetResource("News.Comments.SuccessfullyAdded")
                    : _localizationService.GetResource("News.Comments.SeeAfterApproving");

                return RedirectToRoute("NewsItem", new { SeName = _urlRecordService.GetSeName(newsItem, newsItem.LanguageId, ensureTwoPublishedLanguages: false) });
            }

            //If we got this far, something failed, redisplay form
            model = _newsModelFactory.PrepareNewsItemModel(model, newsItem, true);
            return View(model);
        }

        #endregion
    }
}