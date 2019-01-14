using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Appulu.Core;
using Appulu.Core.Domain.Users;
using Appulu.Core.Domain.Forums;
using Appulu.Services.Forums;
using Appulu.Services.Localization;
using Appulu.Web.Factories;
using Appulu.Web.Framework.Mvc;
using Appulu.Web.Framework.Mvc.Filters;
using Appulu.Web.Framework.Mvc.Rss;
using Appulu.Web.Framework.Security;
using Appulu.Web.Models.Boards;

namespace Appulu.Web.Controllers
{
    [HttpsRequirement(SslRequirement.No)]
    public partial class BoardsController : BasePublicController
    {
        #region Fields

        private readonly ForumSettings _forumSettings;
        private readonly IForumModelFactory _forumModelFactory;
        private readonly IForumService _forumService;
        private readonly ILocalizationService _localizationService;
        private readonly IStoreContext _storeContext;
        private readonly IWebHelper _webHelper;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public BoardsController(ForumSettings forumSettings,
            IForumModelFactory forumModelFactory,
            IForumService forumService,
            ILocalizationService localizationService,
            IStoreContext storeContext,
            IWebHelper webHelper,
            IWorkContext workContext)
        {
            this._forumSettings = forumSettings;
            this._forumModelFactory = forumModelFactory;
            this._forumService = forumService;
            this._localizationService = localizationService;
            this._storeContext = storeContext;
            this._webHelper = webHelper;
            this._workContext = workContext;
        }

        #endregion

        #region Methods

        public virtual IActionResult Index()
        {
            if (!_forumSettings.ForumsEnabled)
            {
                return RedirectToRoute("HomePage");
            }

            var model = _forumModelFactory.PrepareBoardsIndexModel();
            return View(model);
        }

        public virtual IActionResult ActiveDiscussions(int forumId = 0, int pageNumber = 1)
        {
            if (!_forumSettings.ForumsEnabled)
            {
                return RedirectToRoute("HomePage");
            }

            var model = _forumModelFactory.PrepareActiveDiscussionsModel(forumId, pageNumber);
            return View(model);
        }

        public virtual IActionResult ActiveDiscussionsRss(int forumId = 0)
        {
            if (!_forumSettings.ForumsEnabled)
            {
                return RedirectToRoute("HomePage");
            }

            if (!_forumSettings.ActiveDiscussionsFeedEnabled)
            {
                return RedirectToRoute("Boards");
            }

            var topics = _forumService.GetActiveTopics(forumId, 0, _forumSettings.ActiveDiscussionsFeedCount);
            var url = Url.RouteUrl("ActiveDiscussionsRSS", null, _webHelper.CurrentRequestProtocol);

            var feedTitle = _localizationService.GetResource("Forum.ActiveDiscussionsFeedTitle");
            var feedDescription = _localizationService.GetResource("Forum.ActiveDiscussionsFeedDescription");

            var feed = new RssFeed(
                string.Format(feedTitle, _localizationService.GetLocalized(_storeContext.CurrentStore, x => x.Name)),
                feedDescription,
                new Uri(url),
                DateTime.UtcNow);

            var items = new List<RssItem>();

            var viewsText = _localizationService.GetResource("Forum.Views");
            var repliesText = _localizationService.GetResource("Forum.Replies");

            foreach (var topic in topics)
            {
                var topicUrl = Url.RouteUrl("TopicSlug", new { id = topic.Id, slug = _forumService.GetTopicSeName(topic) }, _webHelper.CurrentRequestProtocol);
                var content = $"{repliesText}: {topic.NumReplies.ToString()}, {viewsText}: {topic.Views.ToString()}";

                items.Add(new RssItem(topic.Subject, content, new Uri(topicUrl),
                    $"urn:store:{_storeContext.CurrentStore.Id}:activeDiscussions:topic:{topic.Id}", topic.LastPostTime ?? topic.UpdatedOnUtc));
            }
            feed.Items = items;

            return new RssActionResult(feed, _webHelper.GetThisPageUrl(false));
        }

        public virtual IActionResult ForumGroup(int id)
        {
            if (!_forumSettings.ForumsEnabled)
            {
                return RedirectToRoute("HomePage");
            }

            var forumGroup = _forumService.GetForumGroupById(id);
            if (forumGroup == null)
            {
                return RedirectToRoute("Boards");
            }

            var model = _forumModelFactory.PrepareForumGroupModel(forumGroup);
            return View(model);
        }

        public virtual IActionResult Forum(int id, int pageNumber = 1)
        {
            if (!_forumSettings.ForumsEnabled)
            {
                return RedirectToRoute("HomePage");
            }

            var forum = _forumService.GetForumById(id);
            if (forum == null)
                return RedirectToRoute("Boards");

            var model = _forumModelFactory.PrepareForumPageModel(forum, pageNumber);
            return View(model);
        }

        public virtual IActionResult ForumRss(int id)
        {
            if (!_forumSettings.ForumsEnabled)
            {
                return RedirectToRoute("HomePage");
            }

            if (!_forumSettings.ForumFeedsEnabled)
            {
                return RedirectToRoute("Boards");
            }

            var topicLimit = _forumSettings.ForumFeedCount;
            var forum = _forumService.GetForumById(id);

            if (forum != null)
            {
                //Order by newest topic posts & limit the number of topics to return
                var topics = _forumService.GetAllTopics(forum.Id, 0, string.Empty,
                     ForumSearchType.All, 0, 0, topicLimit);

                var url = Url.RouteUrl("ForumRSS", new { id = forum.Id }, _webHelper.CurrentRequestProtocol);

                var feedTitle = _localizationService.GetResource("Forum.ForumFeedTitle");
                var feedDescription = _localizationService.GetResource("Forum.ForumFeedDescription");

                var feed = new RssFeed(
                    string.Format(feedTitle, _localizationService.GetLocalized(_storeContext.CurrentStore, x => x.Name), forum.Name),
                    feedDescription,
                    new Uri(url),
                    DateTime.UtcNow);

                var items = new List<RssItem>();

                var viewsText = _localizationService.GetResource("Forum.Views");
                var repliesText = _localizationService.GetResource("Forum.Replies");

                foreach (var topic in topics)
                {
                    var topicUrl = Url.RouteUrl("TopicSlug", new { id = topic.Id, slug = _forumService.GetTopicSeName(topic) }, _webHelper.CurrentRequestProtocol);
                    var content = $"{repliesText}: {topic.NumReplies}, {viewsText}: {topic.Views}";

                    items.Add(new RssItem(topic.Subject, content, new Uri(topicUrl), $"urn:store:{_storeContext.CurrentStore.Id}:forum:topic:{topic.Id}", topic.LastPostTime ?? topic.UpdatedOnUtc));
                }

                feed.Items = items;

                return new RssActionResult(feed, _webHelper.GetThisPageUrl(false));
            }

            return new RssActionResult(new RssFeed(new Uri(_webHelper.GetStoreLocation())), _webHelper.GetThisPageUrl(false));
        }

        [HttpPost]
        public virtual IActionResult ForumWatch(int id)
        {
            var watchTopic = _localizationService.GetResource("Forum.WatchForum");
            var unwatchTopic = _localizationService.GetResource("Forum.UnwatchForum");
            var returnText = watchTopic;

            var forum = _forumService.GetForumById(id);
            if (forum == null)
            {
                return Json(new { Subscribed = false, Text = returnText, Error = true });
            }

            if (!_forumService.IsUserAllowedToSubscribe(_workContext.CurrentUser))
            {
                return Json(new { Subscribed = false, Text = returnText, Error = true });
            }

            var forumSubscription = _forumService.GetAllSubscriptions(_workContext.CurrentUser.Id,
                forum.Id, 0, 0, 1).FirstOrDefault();

            bool subscribed;
            if (forumSubscription == null)
            {
                forumSubscription = new ForumSubscription
                {
                    SubscriptionGuid = Guid.NewGuid(),
                    UserId = _workContext.CurrentUser.Id,
                    ForumId = forum.Id,
                    CreatedOnUtc = DateTime.UtcNow
                };
                _forumService.InsertSubscription(forumSubscription);
                subscribed = true;
                returnText = unwatchTopic;
            }
            else
            {
                _forumService.DeleteSubscription(forumSubscription);
                subscribed = false;
            }

            return Json(new { Subscribed = subscribed, Text = returnText, Error = false });
        }

        public virtual IActionResult Topic(int id, int pageNumber = 1)
        {
            if (!_forumSettings.ForumsEnabled)
            {
                return RedirectToRoute("HomePage");
            }

            var forumTopic = _forumService.GetTopicById(id);
            if (forumTopic == null)
            {
                return RedirectToRoute("Boards");
            }

            var model = _forumModelFactory.PrepareForumTopicPageModel(forumTopic, pageNumber);
            //if no posts loaded, redirect to the first page
            if (!model.ForumPostModels.Any() && pageNumber > 1)
                return RedirectToRoute("TopicSlug", new { id = forumTopic.Id, slug = _forumService.GetTopicSeName(forumTopic) });

            //update view count
            forumTopic.Views += 1;
            _forumService.UpdateTopic(forumTopic);

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult TopicWatch(int id)
        {
            var watchTopic = _localizationService.GetResource("Forum.WatchTopic");
            var unwatchTopic = _localizationService.GetResource("Forum.UnwatchTopic");
            var returnText = watchTopic;

            var forumTopic = _forumService.GetTopicById(id);
            if (forumTopic == null)
            {
                return Json(new { Subscribed = false, Text = returnText, Error = true });
            }

            if (!_forumService.IsUserAllowedToSubscribe(_workContext.CurrentUser))
            {
                return Json(new { Subscribed = false, Text = returnText, Error = true });
            }

            var forumSubscription = _forumService.GetAllSubscriptions(_workContext.CurrentUser.Id,
                0, forumTopic.Id, 0, 1).FirstOrDefault();

            bool subscribed;
            if (forumSubscription == null)
            {
                forumSubscription = new ForumSubscription
                {
                    SubscriptionGuid = Guid.NewGuid(),
                    UserId = _workContext.CurrentUser.Id,
                    TopicId = forumTopic.Id,
                    CreatedOnUtc = DateTime.UtcNow
                };
                _forumService.InsertSubscription(forumSubscription);
                subscribed = true;
                returnText = unwatchTopic;
            }
            else
            {
                _forumService.DeleteSubscription(forumSubscription);
                subscribed = false;
            }

            return Json(new { Subscribed = subscribed, Text = returnText, Error = false });
        }

        public virtual IActionResult TopicMove(int id)
        {
            if (!_forumSettings.ForumsEnabled)
            {
                return RedirectToRoute("HomePage");
            }

            var forumTopic = _forumService.GetTopicById(id);
            if (forumTopic == null)
            {
                return RedirectToRoute("Boards");
            }

            var model = _forumModelFactory.PrepareTopicMove(forumTopic);
            return View(model);
        }

        [HttpPost]
        [PublicAntiForgery]
        public virtual IActionResult TopicMove(TopicMoveModel model)
        {
            if (!_forumSettings.ForumsEnabled)
            {
                return RedirectToRoute("HomePage");
            }

            var forumTopic = _forumService.GetTopicById(model.Id);

            if (forumTopic == null)
            {
                return RedirectToRoute("Boards");
            }

            var newForumId = model.ForumSelected;
            var forum = _forumService.GetForumById(newForumId);

            if (forum != null && forumTopic.ForumId != newForumId)
            {
                _forumService.MoveTopic(forumTopic.Id, newForumId);
            }

            return RedirectToRoute("TopicSlug", new { id = forumTopic.Id, slug = _forumService.GetTopicSeName(forumTopic) });
        }

        [HttpPost]
        [PublicAntiForgery]
        public virtual IActionResult TopicDelete(int id)
        {
            if (!_forumSettings.ForumsEnabled)
            {
                return Json(new
                {
                    redirect = Url.RouteUrl("HomePage"),
                });
            }

            var forumTopic = _forumService.GetTopicById(id);
            if (forumTopic != null)
            {
                if (!_forumService.IsUserAllowedToDeleteTopic(_workContext.CurrentUser, forumTopic))
                {
                    return Challenge();
                }
                var forum = _forumService.GetForumById(forumTopic.ForumId);

                _forumService.DeleteTopic(forumTopic);

                if (forum != null)
                {
                    return Json(new
                    {
                        redirect = Url.RouteUrl("ForumSlug", new { id = forum.Id, slug = _forumService.GetForumSeName(forum) }),
                    });
                }
            }

            return Json(new
            {
                redirect = Url.RouteUrl("Boards"),
            });
        }

        public virtual IActionResult TopicCreate(int id)
        {
            if (!_forumSettings.ForumsEnabled)
            {
                return RedirectToRoute("HomePage");
            }

            var forum = _forumService.GetForumById(id);
            if (forum == null)
            {
                return RedirectToRoute("Boards");
            }

            if (_forumService.IsUserAllowedToCreateTopic(_workContext.CurrentUser, forum) == false)
            {
                return Challenge();
            }

            var model = new EditForumTopicModel();
            _forumModelFactory.PrepareTopicCreateModel(forum, model);
            return View(model);
        }

        [HttpPost]
        [PublicAntiForgery]
        public virtual IActionResult TopicCreate(EditForumTopicModel model)
        {
            if (!_forumSettings.ForumsEnabled)
            {
                return RedirectToRoute("HomePage");
            }

            var forum = _forumService.GetForumById(model.ForumId);

            if (forum == null)
            {
                return RedirectToRoute("Boards");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (!_forumService.IsUserAllowedToCreateTopic(_workContext.CurrentUser, forum))
                    {
                        return Challenge();
                    }

                    var subject = model.Subject;
                    var maxSubjectLength = _forumSettings.TopicSubjectMaxLength;
                    if (maxSubjectLength > 0 && subject.Length > maxSubjectLength)
                    {
                        subject = subject.Substring(0, maxSubjectLength);
                    }

                    var text = model.Text;
                    var maxPostLength = _forumSettings.PostMaxLength;
                    if (maxPostLength > 0 && text.Length > maxPostLength)
                    {
                        text = text.Substring(0, maxPostLength);
                    }

                    var topicType = ForumTopicType.Normal;

                    var ipAddress = _webHelper.GetCurrentIpAddress();

                    var nowUtc = DateTime.UtcNow;

                    if (_forumService.IsUserAllowedToSetTopicPriority(_workContext.CurrentUser))
                    {
                        topicType = (ForumTopicType)Enum.ToObject(typeof(ForumTopicType), model.TopicTypeId);
                    }

                    //forum topic
                    var forumTopic = new ForumTopic
                    {
                        ForumId = forum.Id,
                        UserId = _workContext.CurrentUser.Id,
                        TopicTypeId = (int)topicType,
                        Subject = subject,
                        CreatedOnUtc = nowUtc,
                        UpdatedOnUtc = nowUtc
                    };
                    _forumService.InsertTopic(forumTopic, true);

                    //forum post
                    var forumPost = new ForumPost
                    {
                        TopicId = forumTopic.Id,
                        UserId = _workContext.CurrentUser.Id,
                        Text = text,
                        IPAddress = ipAddress,
                        CreatedOnUtc = nowUtc,
                        UpdatedOnUtc = nowUtc
                    };
                    _forumService.InsertPost(forumPost, false);

                    //update forum topic
                    forumTopic.NumPosts = 1;
                    forumTopic.LastPostId = forumPost.Id;
                    forumTopic.LastPostUserId = forumPost.UserId;
                    forumTopic.LastPostTime = forumPost.CreatedOnUtc;
                    forumTopic.UpdatedOnUtc = nowUtc;
                    _forumService.UpdateTopic(forumTopic);

                    //subscription                
                    if (_forumService.IsUserAllowedToSubscribe(_workContext.CurrentUser))
                    {
                        if (model.Subscribed)
                        {
                            var forumSubscription = new ForumSubscription
                            {
                                SubscriptionGuid = Guid.NewGuid(),
                                UserId = _workContext.CurrentUser.Id,
                                TopicId = forumTopic.Id,
                                CreatedOnUtc = nowUtc
                            };

                            _forumService.InsertSubscription(forumSubscription);
                        }
                    }

                    return RedirectToRoute("TopicSlug", new { id = forumTopic.Id, slug = _forumService.GetTopicSeName(forumTopic) });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            //redisplay form
            _forumModelFactory.PrepareTopicCreateModel(forum, model);
            return View(model);
        }

        public virtual IActionResult TopicEdit(int id)
        {
            if (!_forumSettings.ForumsEnabled)
            {
                return RedirectToRoute("HomePage");
            }

            var forumTopic = _forumService.GetTopicById(id);
            if (forumTopic == null)
            {
                return RedirectToRoute("Boards");
            }

            if (!_forumService.IsUserAllowedToEditTopic(_workContext.CurrentUser, forumTopic))
            {
                return Challenge();
            }

            var model = new EditForumTopicModel();
            _forumModelFactory.PrepareTopicEditModel(forumTopic, model, false);
            return View(model);
        }

        [HttpPost]
        [PublicAntiForgery]
        public virtual IActionResult TopicEdit(EditForumTopicModel model)
        {
            if (!_forumSettings.ForumsEnabled)
            {
                return RedirectToRoute("HomePage");
            }

            var forumTopic = _forumService.GetTopicById(model.Id);

            if (forumTopic == null)
            {
                return RedirectToRoute("Boards");
            }
            var forum = forumTopic.Forum;
            if (forum == null)
            {
                return RedirectToRoute("Boards");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (!_forumService.IsUserAllowedToEditTopic(_workContext.CurrentUser, forumTopic))
                    {
                        return Challenge();
                    }

                    var subject = model.Subject;
                    var maxSubjectLength = _forumSettings.TopicSubjectMaxLength;
                    if (maxSubjectLength > 0 && subject.Length > maxSubjectLength)
                    {
                        subject = subject.Substring(0, maxSubjectLength);
                    }

                    var text = model.Text;
                    var maxPostLength = _forumSettings.PostMaxLength;
                    if (maxPostLength > 0 && text.Length > maxPostLength)
                    {
                        text = text.Substring(0, maxPostLength);
                    }

                    var topicType = ForumTopicType.Normal;

                    var ipAddress = _webHelper.GetCurrentIpAddress();

                    var nowUtc = DateTime.UtcNow;

                    if (_forumService.IsUserAllowedToSetTopicPriority(_workContext.CurrentUser))
                    {
                        topicType = (ForumTopicType)Enum.ToObject(typeof(ForumTopicType), model.TopicTypeId);
                    }

                    //forum topic
                    forumTopic.TopicTypeId = (int)topicType;
                    forumTopic.Subject = subject;
                    forumTopic.UpdatedOnUtc = nowUtc;
                    _forumService.UpdateTopic(forumTopic);

                    //forum post                
                    var firstPost = _forumService.GetFirstPost(forumTopic);
                    if (firstPost != null)
                    {
                        firstPost.Text = text;
                        firstPost.UpdatedOnUtc = nowUtc;
                        _forumService.UpdatePost(firstPost);
                    }
                    else
                    {
                        //error (not possible)
                        firstPost = new ForumPost
                        {
                            TopicId = forumTopic.Id,
                            UserId = forumTopic.UserId,
                            Text = text,
                            IPAddress = ipAddress,
                            UpdatedOnUtc = nowUtc
                        };

                        _forumService.InsertPost(firstPost, false);
                    }

                    //subscription
                    if (_forumService.IsUserAllowedToSubscribe(_workContext.CurrentUser))
                    {
                        var forumSubscription = _forumService.GetAllSubscriptions(_workContext.CurrentUser.Id,
                            0, forumTopic.Id, 0, 1).FirstOrDefault();
                        if (model.Subscribed)
                        {
                            if (forumSubscription == null)
                            {
                                forumSubscription = new ForumSubscription
                                {
                                    SubscriptionGuid = Guid.NewGuid(),
                                    UserId = _workContext.CurrentUser.Id,
                                    TopicId = forumTopic.Id,
                                    CreatedOnUtc = nowUtc
                                };

                                _forumService.InsertSubscription(forumSubscription);
                            }
                        }
                        else
                        {
                            if (forumSubscription != null)
                            {
                                _forumService.DeleteSubscription(forumSubscription);
                            }
                        }
                    }

                    // redirect to the topic page with the topic slug
                    return RedirectToRoute("TopicSlug", new { id = forumTopic.Id, slug = _forumService.GetTopicSeName(forumTopic) });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            //redisplay form
            _forumModelFactory.PrepareTopicEditModel(forumTopic, model, true);
            return View(model);
        }

        [HttpPost]
        [PublicAntiForgery]
        public virtual IActionResult PostDelete(int id)
        {
            if (!_forumSettings.ForumsEnabled)
            {
                return Json(new
                {
                    redirect = Url.RouteUrl("HomePage"),
                });
            }

            var forumPost = _forumService.GetPostById(id);

            if (forumPost != null)
            {
                if (!_forumService.IsUserAllowedToDeletePost(_workContext.CurrentUser, forumPost))
                {
                    return Challenge();
                }

                var forumTopic = forumPost.ForumTopic;
                var forumId = forumTopic.Forum.Id;
                var forumSlug = _forumService.GetForumSeName(forumTopic.Forum);

                _forumService.DeletePost(forumPost);

                //get topic one more time because it can be deleted (first or only post deleted)
                forumTopic = _forumService.GetTopicById(forumPost.TopicId);
                if (forumTopic == null)
                {
                    return Json(new
                    {
                        redirect = Url.RouteUrl("ForumSlug", new { id = forumId, slug = forumSlug }),
                    });
                }
                return Json(new
                {
                    redirect = Url.RouteUrl("TopicSlug", new { id = forumTopic.Id, slug = _forumService.GetTopicSeName(forumTopic) }),
                });
            }

            return Json(new
            {
                redirect = Url.RouteUrl("Boards"),
            });
        }

        public virtual IActionResult PostCreate(int id, int? quote)
        {
            if (!_forumSettings.ForumsEnabled)
            {
                return RedirectToRoute("HomePage");
            }

            var forumTopic = _forumService.GetTopicById(id);
            if (forumTopic == null)
            {
                return RedirectToRoute("Boards");
            }

            if (!_forumService.IsUserAllowedToCreatePost(_workContext.CurrentUser, forumTopic))
            {
                return Challenge();
            }

            var model = _forumModelFactory.PreparePostCreateModel(forumTopic, quote, false);
            return View(model);
        }

        [HttpPost]
        [PublicAntiForgery]
        public virtual IActionResult PostCreate(EditForumPostModel model)
        {
            if (!_forumSettings.ForumsEnabled)
            {
                return RedirectToRoute("HomePage");
            }

            var forumTopic = _forumService.GetTopicById(model.ForumTopicId);
            if (forumTopic == null)
            {
                return RedirectToRoute("Boards");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (!_forumService.IsUserAllowedToCreatePost(_workContext.CurrentUser, forumTopic))
                        return Challenge();

                    var text = model.Text;
                    var maxPostLength = _forumSettings.PostMaxLength;
                    if (maxPostLength > 0 && text.Length > maxPostLength)
                        text = text.Substring(0, maxPostLength);

                    var ipAddress = _webHelper.GetCurrentIpAddress();

                    var nowUtc = DateTime.UtcNow;

                    var forumPost = new ForumPost
                    {
                        TopicId = forumTopic.Id,
                        UserId = _workContext.CurrentUser.Id,
                        Text = text,
                        IPAddress = ipAddress,
                        CreatedOnUtc = nowUtc,
                        UpdatedOnUtc = nowUtc
                    };
                    _forumService.InsertPost(forumPost, true);

                    //subscription
                    if (_forumService.IsUserAllowedToSubscribe(_workContext.CurrentUser))
                    {
                        var forumSubscription = _forumService.GetAllSubscriptions(_workContext.CurrentUser.Id,
                            0, forumPost.TopicId, 0, 1).FirstOrDefault();
                        if (model.Subscribed)
                        {
                            if (forumSubscription == null)
                            {
                                forumSubscription = new ForumSubscription
                                {
                                    SubscriptionGuid = Guid.NewGuid(),
                                    UserId = _workContext.CurrentUser.Id,
                                    TopicId = forumPost.TopicId,
                                    CreatedOnUtc = nowUtc
                                };

                                _forumService.InsertSubscription(forumSubscription);
                            }
                        }
                        else
                        {
                            if (forumSubscription != null)
                            {
                                _forumService.DeleteSubscription(forumSubscription);
                            }
                        }
                    }

                    var pageSize = _forumSettings.PostsPageSize > 0 ? _forumSettings.PostsPageSize : 10;

                    var pageIndex = (_forumService.CalculateTopicPageIndex(forumPost.TopicId, pageSize, forumPost.Id) + 1);
                    var url = string.Empty;
                    if (pageIndex > 1)
                    {
                        url = Url.RouteUrl("TopicSlugPaged", new { id = forumPost.TopicId, slug = _forumService.GetTopicSeName(forumPost.ForumTopic), pageNumber = pageIndex });
                    }
                    else
                    {
                        url = Url.RouteUrl("TopicSlug", new { id = forumPost.TopicId, slug = _forumService.GetTopicSeName(forumPost.ForumTopic) });
                    }
                    return Redirect($"{url}#{forumPost.Id}");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            //redisplay form
            _forumModelFactory.PreparePostCreateModel(forumTopic, 0, true);
            return View(model);
        }

        public virtual IActionResult PostEdit(int id)
        {
            if (!_forumSettings.ForumsEnabled)
            {
                return RedirectToRoute("HomePage");
            }

            var forumPost = _forumService.GetPostById(id);
            if (forumPost == null)
            {
                return RedirectToRoute("Boards");
            }

            if (!_forumService.IsUserAllowedToEditPost(_workContext.CurrentUser, forumPost))
            {
                return Challenge();
            }

            var model = _forumModelFactory.PreparePostEditModel(forumPost, false);
            return View(model);
        }

        [HttpPost]
        [PublicAntiForgery]
        public virtual IActionResult PostEdit(EditForumPostModel model)
        {
            if (!_forumSettings.ForumsEnabled)
            {
                return RedirectToRoute("HomePage");
            }

            var forumPost = _forumService.GetPostById(model.Id);
            if (forumPost == null)
            {
                return RedirectToRoute("Boards");
            }

            if (!_forumService.IsUserAllowedToEditPost(_workContext.CurrentUser, forumPost))
            {
                return Challenge();
            }

            var forumTopic = forumPost.ForumTopic;
            if (forumTopic == null)
            {
                return RedirectToRoute("Boards");
            }

            var forum = forumTopic.Forum;
            if (forum == null)
            {
                return RedirectToRoute("Boards");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var nowUtc = DateTime.UtcNow;

                    var text = model.Text;
                    var maxPostLength = _forumSettings.PostMaxLength;
                    if (maxPostLength > 0 && text.Length > maxPostLength)
                    {
                        text = text.Substring(0, maxPostLength);
                    }

                    forumPost.UpdatedOnUtc = nowUtc;
                    forumPost.Text = text;
                    _forumService.UpdatePost(forumPost);

                    //subscription
                    if (_forumService.IsUserAllowedToSubscribe(_workContext.CurrentUser))
                    {
                        var forumSubscription = _forumService.GetAllSubscriptions(_workContext.CurrentUser.Id,
                            0, forumPost.TopicId, 0, 1).FirstOrDefault();
                        if (model.Subscribed)
                        {
                            if (forumSubscription == null)
                            {
                                forumSubscription = new ForumSubscription
                                {
                                    SubscriptionGuid = Guid.NewGuid(),
                                    UserId = _workContext.CurrentUser.Id,
                                    TopicId = forumPost.TopicId,
                                    CreatedOnUtc = nowUtc
                                };
                                _forumService.InsertSubscription(forumSubscription);
                            }
                        }
                        else
                        {
                            if (forumSubscription != null)
                            {
                                _forumService.DeleteSubscription(forumSubscription);
                            }
                        }
                    }

                    var pageSize = _forumSettings.PostsPageSize > 0 ? _forumSettings.PostsPageSize : 10;
                    var pageIndex = (_forumService.CalculateTopicPageIndex(forumPost.TopicId, pageSize, forumPost.Id) + 1);
                    var url = string.Empty;
                    if (pageIndex > 1)
                    {
                        url = Url.RouteUrl("TopicSlugPaged", new { id = forumPost.TopicId, slug = _forumService.GetTopicSeName(forumPost.ForumTopic), pageNumber = pageIndex });
                    }
                    else
                    {
                        url = Url.RouteUrl("TopicSlug", new { id = forumPost.TopicId, slug = _forumService.GetTopicSeName(forumPost.ForumTopic) });
                    }
                    return Redirect($"{url}#{forumPost.Id}");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            //redisplay form
            _forumModelFactory.PreparePostEditModel(forumPost, true);
            return View(model);
        }

        public virtual IActionResult Search(string searchterms, bool? adv, string forumId,
            string within, string limitDays, int page = 1)
        {
            if (!_forumSettings.ForumsEnabled)
            {
                return RedirectToRoute("HomePage");
            }

            var model = _forumModelFactory.PrepareSearchModel(searchterms, adv, forumId, within, limitDays, page);
            return View(model);
        }

        public virtual IActionResult UserForumSubscriptions(int? pageNumber)
        {
            if (!_forumSettings.AllowUsersToManageSubscriptions)
            {
                return RedirectToRoute("UserInfo");
            }

            var model = _forumModelFactory.PrepareUserForumSubscriptionsModel(pageNumber);
            return View(model);
        }

        [HttpPost, ActionName("UserForumSubscriptions")]
        public virtual IActionResult UserForumSubscriptionsPOST(IFormCollection formCollection)
        {
            foreach (var key in formCollection.Keys)
            {
                var value = formCollection[key];

                if (value.Equals("on") && key.StartsWith("fs", StringComparison.InvariantCultureIgnoreCase))
                {
                    var id = key.Replace("fs", "").Trim();
                    if (int.TryParse(id, out int forumSubscriptionId))
                    {
                        var forumSubscription = _forumService.GetSubscriptionById(forumSubscriptionId);
                        if (forumSubscription != null && forumSubscription.UserId == _workContext.CurrentUser.Id)
                        {
                            _forumService.DeleteSubscription(forumSubscription);
                        }
                    }
                }
            }

            return RedirectToRoute("UserForumSubscriptions");
        }

        [HttpPost]
        public virtual IActionResult PostVote(int postId, bool isUp)
        {
            if (!_forumSettings.AllowPostVoting)
                return new NullJsonResult();

            var forumPost = _forumService.GetPostById(postId);
            if (forumPost == null)
                return new NullJsonResult();

            if (!_workContext.CurrentUser.IsRegistered())
                return Json(new
                {
                    Error = _localizationService.GetResource("Forum.Votes.Login"),
                    VoteCount = forumPost.VoteCount
                });

            if (_workContext.CurrentUser.Id == forumPost.UserId)
                return Json(new
                {
                    Error = _localizationService.GetResource("Forum.Votes.OwnPost"),
                    VoteCount = forumPost.VoteCount
                });

            var forumPostVote = _forumService.GetPostVote(postId, _workContext.CurrentUser);
            if (forumPostVote != null)
            {
                if ((forumPostVote.IsUp && isUp) || (!forumPostVote.IsUp && !isUp))
                    return Json(new
                    {
                        Error = _localizationService.GetResource("Forum.Votes.AlreadyVoted"),
                        VoteCount = forumPost.VoteCount
                    });

                _forumService.DeletePostVote(forumPostVote);
                return Json(new { VoteCount = forumPost.VoteCount });
            }

            if (_forumService.GetNumberOfPostVotes(_workContext.CurrentUser, DateTime.UtcNow.AddDays(-1)) >= _forumSettings.MaxVotesPerDay)
                return Json(new
                {
                    Error = string.Format(_localizationService.GetResource("Forum.Votes.MaxVotesReached"), _forumSettings.MaxVotesPerDay),
                    VoteCount = forumPost.VoteCount
                });

            _forumService.InsertPostVote(new ForumPostVote
            {
                UserId = _workContext.CurrentUser.Id,
                ForumPostId = postId,
                IsUp = isUp,
                CreatedOnUtc = DateTime.UtcNow
            });
            return Json(new { VoteCount = forumPost.VoteCount, IsUp = isUp });
        }

        #endregion
    }
}