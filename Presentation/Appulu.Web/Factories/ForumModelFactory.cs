using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Appulu.Core;
using Appulu.Core.Domain.Users;
using Appulu.Core.Domain.Forums;
using Appulu.Core.Domain.Media;
using Appulu.Core.Html;
using Appulu.Services.Common;
using Appulu.Services.Users;
using Appulu.Services.Directory;
using Appulu.Services.Forums;
using Appulu.Services.Helpers;
using Appulu.Services.Localization;
using Appulu.Services.Media;
using Appulu.Web.Framework.Extensions;
using Appulu.Web.Models.Boards;
using Appulu.Web.Models.Common;

namespace Appulu.Web.Factories
{
    /// <summary>
    /// Represents the forum model factory
    /// </summary>
    public partial class ForumModelFactory : IForumModelFactory
    {
        #region Fields

        private readonly UserSettings _userSettings;
        private readonly ForumSettings _forumSettings;
        private readonly ICountryService _countryService;
        private readonly IUserService _userService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IForumService _forumService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ILocalizationService _localizationService;
        private readonly IPictureService _pictureService;
        private readonly IWorkContext _workContext;
        private readonly MediaSettings _mediaSettings;

        #endregion

        #region Ctor

        public ForumModelFactory(UserSettings userSettings,
            ForumSettings forumSettings,
            ICountryService countryService,
            IUserService userService,
            IDateTimeHelper dateTimeHelper,
            IForumService forumService,
            IGenericAttributeService genericAttributeService,
            ILocalizationService localizationService,
            IPictureService pictureService,
            IWorkContext workContext,
            MediaSettings mediaSettings)
        {
            this._userSettings = userSettings;
            this._forumSettings = forumSettings;
            this._countryService = countryService;
            this._userService = userService;
            this._dateTimeHelper = dateTimeHelper;
            this._forumService = forumService;
            this._genericAttributeService = genericAttributeService;
            this._localizationService = localizationService;
            this._pictureService = pictureService;
            this._workContext = workContext;
            this._mediaSettings = mediaSettings;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Get the list of forum topic types
        /// </summary>
        /// <returns>Collection of the select list item</returns>
        protected virtual IEnumerable<SelectListItem> ForumTopicTypesList()
        {
            var list = new List<SelectListItem>();

            list.Add(new SelectListItem
            {
                Text = _localizationService.GetResource("Forum.Normal"),
                Value = ((int)ForumTopicType.Normal).ToString()
            });

            list.Add(new SelectListItem
            {
                Text = _localizationService.GetResource("Forum.Sticky"),
                Value = ((int)ForumTopicType.Sticky).ToString()
            });

            list.Add(new SelectListItem
            {
                Text = _localizationService.GetResource("Forum.Announcement"),
                Value = ((int)ForumTopicType.Announcement).ToString()
            });

            return list;
        }

        /// <summary>
        /// Get the list of forum groups
        /// </summary>
        /// <returns>Collection of the select list item</returns>
        protected virtual IEnumerable<SelectListItem> ForumGroupsForumsList()
        {
            var forumsList = new List<SelectListItem>();
            var separator = "--";
            var forumGroups = _forumService.GetAllForumGroups();

            foreach (var fg in forumGroups)
            {
                // Add the forum group with Value of 0 so it won't be used as a target forum
                forumsList.Add(new SelectListItem { Text = fg.Name, Value = "0" });

                var forums = _forumService.GetAllForumsByGroupId(fg.Id);
                foreach (var f in forums)
                {
                    forumsList.Add(new SelectListItem { Text = $"{separator}{f.Name}", Value = f.Id.ToString() });
                }
            }

            return forumsList;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare the forum topic row model
        /// </summary>
        /// <param name="topic">Forum topic</param>
        /// <returns>Forum topic row model</returns>
        public virtual ForumTopicRowModel PrepareForumTopicRowModel(ForumTopic topic)
        {
            if (topic == null)
                throw new ArgumentNullException(nameof(topic));

            var topicModel = new ForumTopicRowModel
            {
                Id = topic.Id,
                Subject = topic.Subject,
                SeName = _forumService.GetTopicSeName(topic),
                LastPostId = topic.LastPostId,
                NumPosts = topic.NumPosts,
                Views = topic.Views,
                NumReplies = topic.NumReplies,
                ForumTopicType = topic.ForumTopicType,
                UserId = topic.UserId,
                AllowViewingProfiles = _userSettings.AllowViewingProfiles && !topic.User.IsGuest(),
                UserName = _userService.FormatUserName(topic.User)
            };

            var forumPosts = _forumService.GetAllPosts(topic.Id, 0, string.Empty, 1, _forumSettings.PostsPageSize);
            topicModel.TotalPostPages = forumPosts.TotalPages;

            var firstPost = _forumService.GetFirstPost(topic);
            topicModel.Votes = firstPost != null ? firstPost.VoteCount : 0;
            return topicModel;
        }

        /// <summary>
        /// Prepare the forum row model
        /// </summary>
        /// <param name="forum">Forum</param>
        /// <returns>Forum row model</returns>
        public virtual ForumRowModel PrepareForumRowModel(Forum forum)
        {
            if (forum == null)
                throw new ArgumentNullException(nameof(forum));

            var forumModel = new ForumRowModel
            {
                Id = forum.Id,
                Name = forum.Name,
                SeName = _forumService.GetForumSeName(forum),
                Description = forum.Description,
                NumTopics = forum.NumTopics,
                NumPosts = forum.NumPosts,
                LastPostId = forum.LastPostId,
            };
            return forumModel;
        }

        /// <summary>
        /// Prepare the forum group model
        /// </summary>
        /// <param name="forumGroup">Forum group</param>
        /// <returns>Forum group model</returns>
        public virtual ForumGroupModel PrepareForumGroupModel(ForumGroup forumGroup)
        {
            if (forumGroup == null)
                throw new ArgumentNullException(nameof(forumGroup));

            var forumGroupModel = new ForumGroupModel
            {
                Id = forumGroup.Id,
                Name = forumGroup.Name,
                SeName = _forumService.GetForumGroupSeName(forumGroup),
            };
            var forums = _forumService.GetAllForumsByGroupId(forumGroup.Id);
            foreach (var forum in forums)
            {
                var forumModel = PrepareForumRowModel(forum);
                forumGroupModel.Forums.Add(forumModel);
            }
            return forumGroupModel;
        }

        /// <summary>
        /// Prepare the boards index model
        /// </summary>
        /// <returns>Boards index model</returns>
        public virtual BoardsIndexModel PrepareBoardsIndexModel()
        {
            var model = new BoardsIndexModel();

            var forumGroups = _forumService.GetAllForumGroups();
            foreach (var forumGroup in forumGroups)
            {
                var forumGroupModel = PrepareForumGroupModel(forumGroup);
                model.ForumGroups.Add(forumGroupModel);
            }
            return model;
        }

        /// <summary>
        /// Prepare the active discussions model
        /// </summary>
        /// <returns>Active discussions model</returns>
        public virtual ActiveDiscussionsModel PrepareActiveDiscussionsModel()
        {
            var model = new ActiveDiscussionsModel()
            {
                ViewAllLinkEnabled = true,
                ActiveDiscussionsFeedEnabled = _forumSettings.ActiveDiscussionsFeedEnabled,
                PostsPageSize = _forumSettings.PostsPageSize,
                AllowPostVoting = _forumSettings.AllowPostVoting
            };

            var topics = _forumService.GetActiveTopics(0, 0, _forumSettings.HomePageActiveDiscussionsTopicCount);
            foreach (var topic in topics)
            {
                var topicModel = PrepareForumTopicRowModel(topic);
                model.ForumTopics.Add(topicModel);
            }

            return model;
        }

        /// <summary>
        /// Prepare the active discussions model
        /// </summary>
        /// <param name="forumId">Forum identifier</param>
        /// <param name="page">Number of forum topics page</param>
        /// <returns>Active discussions model</returns>
        public virtual ActiveDiscussionsModel PrepareActiveDiscussionsModel(int forumId, int page)
        {
            var model = new ActiveDiscussionsModel
            {
                ViewAllLinkEnabled = false,
                ActiveDiscussionsFeedEnabled = _forumSettings.ActiveDiscussionsFeedEnabled,
                PostsPageSize = _forumSettings.PostsPageSize,
                AllowPostVoting = _forumSettings.AllowPostVoting
            };

            var pageSize = _forumSettings.ActiveDiscussionsPageSize > 0 ? _forumSettings.ActiveDiscussionsPageSize : 50;

            var topics = _forumService.GetActiveTopics(forumId, (page - 1), pageSize);
            model.TopicPageSize = topics.PageSize;
            model.TopicTotalRecords = topics.TotalCount;
            model.TopicPageIndex = topics.PageIndex;
            foreach (var topic in topics)
            {
                var topicModel = PrepareForumTopicRowModel(topic);
                model.ForumTopics.Add(topicModel);
            }

            return model;
        }

        /// <summary>
        /// Prepare the forum page model
        /// </summary>
        /// <param name="forum">Forum</param>
        /// <param name="page">Number of forum topics page</param>
        /// <returns>Forum page model</returns>
        public virtual ForumPageModel PrepareForumPageModel(Forum forum, int page)
        {
            if (forum == null)
                throw new ArgumentNullException(nameof(forum));

            var model = new ForumPageModel
            {
                Id = forum.Id,
                Name = forum.Name,
                SeName = _forumService.GetForumSeName(forum),
                Description = forum.Description
            };

            var pageSize = _forumSettings.TopicsPageSize > 0 ? _forumSettings.TopicsPageSize : 10;

            model.AllowPostVoting = _forumSettings.AllowPostVoting;

            //subscription                
            if (_forumService.IsUserAllowedToSubscribe(_workContext.CurrentUser))
            {
                model.WatchForumText = _localizationService.GetResource("Forum.WatchForum");

                var forumSubscription = _forumService.GetAllSubscriptions(_workContext.CurrentUser.Id, forum.Id, 0, 0, 1).FirstOrDefault();
                if (forumSubscription != null)
                {
                    model.WatchForumText = _localizationService.GetResource("Forum.UnwatchForum");
                }
            }

            var topics = _forumService.GetAllTopics(forum.Id, 0, string.Empty, ForumSearchType.All, 0, (page - 1), pageSize);
            model.TopicPageSize = topics.PageSize;
            model.TopicTotalRecords = topics.TotalCount;
            model.TopicPageIndex = topics.PageIndex;
            foreach (var topic in topics)
            {
                var topicModel = PrepareForumTopicRowModel(topic);
                model.ForumTopics.Add(topicModel);
            }
            model.IsUserAllowedToSubscribe = _forumService.IsUserAllowedToSubscribe(_workContext.CurrentUser);
            model.ForumFeedsEnabled = _forumSettings.ForumFeedsEnabled;
            model.PostsPageSize = _forumSettings.PostsPageSize;
            return model;
        }

        /// <summary>
        /// Prepare the forum topic page model
        /// </summary>
        /// <param name="forumTopic">Forum topic</param>
        /// <param name="page">Number of forum posts page</param>
        /// <returns>Forum topic page model</returns>
        public virtual ForumTopicPageModel PrepareForumTopicPageModel(ForumTopic forumTopic, int page)
        {
            if (forumTopic == null)
                throw new ArgumentNullException(nameof(forumTopic));

            //load posts
            var posts = _forumService.GetAllPosts(forumTopic.Id, 0, string.Empty,
                page - 1, _forumSettings.PostsPageSize);

            //prepare model
            var model = new ForumTopicPageModel
            {
                Id = forumTopic.Id,
                Subject = forumTopic.Subject,
                SeName = _forumService.GetTopicSeName(forumTopic),

                IsUserAllowedToEditTopic = _forumService.IsUserAllowedToEditTopic(_workContext.CurrentUser, forumTopic),
                IsUserAllowedToDeleteTopic = _forumService.IsUserAllowedToDeleteTopic(_workContext.CurrentUser, forumTopic),
                IsUserAllowedToMoveTopic = _forumService.IsUserAllowedToMoveTopic(_workContext.CurrentUser, forumTopic),
                IsUserAllowedToSubscribe = _forumService.IsUserAllowedToSubscribe(_workContext.CurrentUser)
            };

            if (model.IsUserAllowedToSubscribe)
            {
                model.WatchTopicText = _localizationService.GetResource("Forum.WatchTopic");

                var forumTopicSubscription = _forumService.GetAllSubscriptions(_workContext.CurrentUser.Id, 0, forumTopic.Id, 0, 1).FirstOrDefault();
                if (forumTopicSubscription != null)
                {
                    model.WatchTopicText = _localizationService.GetResource("Forum.UnwatchTopic");
                }
            }
            model.PostsPageIndex = posts.PageIndex;
            model.PostsPageSize = posts.PageSize;
            model.PostsTotalRecords = posts.TotalCount;
            foreach (var post in posts)
            {
                var forumPostModel = new ForumPostModel
                {
                    Id = post.Id,
                    ForumTopicId = post.TopicId,
                    ForumTopicSeName = _forumService.GetTopicSeName(forumTopic),
                    FormattedText = _forumService.FormatPostText(post),
                    IsCurrentUserAllowedToEditPost = _forumService.IsUserAllowedToEditPost(_workContext.CurrentUser, post),
                    IsCurrentUserAllowedToDeletePost = _forumService.IsUserAllowedToDeletePost(_workContext.CurrentUser, post),
                    UserId = post.UserId,
                    AllowViewingProfiles = _userSettings.AllowViewingProfiles && !post.User.IsGuest(),
                    UserName = _userService.FormatUserName(post.User),
                    IsUserForumModerator = post.User.IsForumModerator(),
                    ShowUsersPostCount = _forumSettings.ShowUsersPostCount,
                    ForumPostCount = _genericAttributeService.GetAttribute<int>(post.User, AppUserDefaults.ForumPostCountAttribute),
                    ShowUsersJoinDate = _userSettings.ShowUsersJoinDate && !post.User.IsGuest(),
                    UserJoinDate = post.User.CreatedOnUtc,
                    AllowPrivateMessages = _forumSettings.AllowPrivateMessages && !post.User.IsGuest(),
                    SignaturesEnabled = _forumSettings.SignaturesEnabled,
                    FormattedSignature = _forumService.FormatForumSignatureText(_genericAttributeService.GetAttribute<string>(post.User, AppUserDefaults.SignatureAttribute)),
                };
                //created on string
                if (_forumSettings.RelativeDateTimeFormattingEnabled)
                    forumPostModel.PostCreatedOnStr = post.CreatedOnUtc.RelativeFormat(true, "f");
                else
                    forumPostModel.PostCreatedOnStr =
                        _dateTimeHelper.ConvertToUserTime(post.CreatedOnUtc, DateTimeKind.Utc).ToString("f");
                //avatar
                if (_userSettings.AllowUsersToUploadAvatars)
                {
                    forumPostModel.UserAvatarUrl = _pictureService.GetPictureUrl(
                        _genericAttributeService.GetAttribute<int>(post.User, AppUserDefaults.AvatarPictureIdAttribute),
                        _mediaSettings.AvatarPictureSize,
                        _userSettings.DefaultAvatarEnabled,
                        defaultPictureType: PictureType.Avatar);
                }
                //location
                forumPostModel.ShowUsersLocation = _userSettings.ShowUsersLocation && !post.User.IsGuest();
                if (_userSettings.ShowUsersLocation)
                {
                    var countryId = _genericAttributeService.GetAttribute<int>(post.User, AppUserDefaults.CountryIdAttribute);
                    var country = _countryService.GetCountryById(countryId);
                    forumPostModel.UserLocation = country != null ? _localizationService.GetLocalized(country, x => x.Name) : string.Empty;
                }

                //votes
                if (_forumSettings.AllowPostVoting)
                {
                    forumPostModel.AllowPostVoting = true;
                    forumPostModel.VoteCount = post.VoteCount;
                    var postVote = _forumService.GetPostVote(post.Id, _workContext.CurrentUser);
                    if (postVote != null)
                        forumPostModel.VoteIsUp = postVote.IsUp;
                }

                // page number is needed for creating post link in _ForumPost partial view
                forumPostModel.CurrentTopicPage = page;
                model.ForumPostModels.Add(forumPostModel);
            }

            return model;
        }

        /// <summary>
        /// Prepare the topic move model
        /// </summary>
        /// <param name="forumTopic">Forum topic</param>
        /// <returns>Topic move model</returns>
        public virtual TopicMoveModel PrepareTopicMove(ForumTopic forumTopic)
        {
            if (forumTopic == null)
                throw new ArgumentNullException(nameof(forumTopic));

            var model = new TopicMoveModel
            {
                ForumList = ForumGroupsForumsList(),
                Id = forumTopic.Id,
                TopicSeName = _forumService.GetTopicSeName(forumTopic),
                ForumSelected = forumTopic.ForumId
            };

            return model;
        }

        /// <summary>
        /// Prepare the forum topic create model
        /// </summary>
        /// <param name="forum">Forum</param>
        /// <param name="model">Edit forum topic model</param>
        public virtual void PrepareTopicCreateModel(Forum forum, EditForumTopicModel model)
        {
            if (forum == null)
                throw new ArgumentNullException(nameof(forum));

            if (model == null)
                throw new ArgumentNullException(nameof(model));

            model.IsEdit = false;
            model.ForumId = forum.Id;
            model.ForumName = forum.Name;
            model.ForumSeName = _forumService.GetForumSeName(forum);
            model.ForumEditor = _forumSettings.ForumEditor;
            model.IsUserAllowedToSetTopicPriority = _forumService.IsUserAllowedToSetTopicPriority(_workContext.CurrentUser);
            model.TopicPriorities = ForumTopicTypesList();
            model.IsUserAllowedToSubscribe = _forumService.IsUserAllowedToSubscribe(_workContext.CurrentUser);
        }

        /// <summary>
        /// Prepare the forum topic edit model
        /// </summary>
        /// <param name="forumTopic">Forum topic</param>
        /// <param name="model">Edit forum topic model</param>
        /// <param name="excludeProperties">Whether to exclude populating of model properties from the entity</param>
        public virtual void PrepareTopicEditModel(ForumTopic forumTopic, EditForumTopicModel model, bool excludeProperties)
        {
            if (forumTopic == null)
                throw new ArgumentNullException(nameof(forumTopic));

            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var forum = forumTopic.Forum;
            if (forum == null)
                throw new ArgumentException("forum cannot be loaded");

            model.IsEdit = true;
            model.Id = forumTopic.Id;
            model.TopicPriorities = ForumTopicTypesList();
            model.ForumName = forum.Name;
            model.ForumSeName = _forumService.GetForumSeName(forum);
            model.ForumId = forum.Id;
            model.ForumEditor = _forumSettings.ForumEditor;

            model.IsUserAllowedToSetTopicPriority = _forumService.IsUserAllowedToSetTopicPriority(_workContext.CurrentUser);
            model.IsUserAllowedToSubscribe = _forumService.IsUserAllowedToSubscribe(_workContext.CurrentUser);

            if (!excludeProperties)
            {
                var firstPost = _forumService.GetFirstPost(forumTopic);
                model.Text = firstPost.Text;
                model.Subject = forumTopic.Subject;
                model.TopicTypeId = forumTopic.TopicTypeId;
                //subscription            
                if (model.IsUserAllowedToSubscribe)
                {
                    var forumSubscription = _forumService.GetAllSubscriptions(_workContext.CurrentUser.Id, 0, forumTopic.Id, 0, 1).FirstOrDefault();
                    model.Subscribed = forumSubscription != null;
                }
            }
        }

        /// <summary>
        /// Prepare the forum post create model
        /// </summary>
        /// <param name="forumTopic">Forum topic</param>
        /// <param name="quote">Identifier of the quoted post; pass null to load the empty text</param>
        /// <param name="excludeProperties">Whether to exclude populating of model properties from the entity</param>
        /// <returns>Edit forum post model</returns>
        public virtual EditForumPostModel PreparePostCreateModel(ForumTopic forumTopic, int? quote, bool excludeProperties)
        {
            if (forumTopic == null)
                throw new ArgumentNullException(nameof(forumTopic));

            var forum = forumTopic.Forum;
            if (forum == null)
                throw new ArgumentException("forum cannot be loaded");

            var model = new EditForumPostModel
            {
                ForumTopicId = forumTopic.Id,
                IsEdit = false,
                ForumEditor = _forumSettings.ForumEditor,
                ForumName = forum.Name,
                ForumTopicSubject = forumTopic.Subject,
                ForumTopicSeName = _forumService.GetTopicSeName(forumTopic),
                IsUserAllowedToSubscribe = _forumService.IsUserAllowedToSubscribe(_workContext.CurrentUser)
            };

            if (!excludeProperties)
            {
                //subscription            
                if (model.IsUserAllowedToSubscribe)
                {
                    var forumSubscription = _forumService.GetAllSubscriptions(_workContext.CurrentUser.Id,
                        0, forumTopic.Id, 0, 1).FirstOrDefault();
                    model.Subscribed = forumSubscription != null;
                }

                // Insert the quoted text
                var text = string.Empty;
                if (quote.HasValue)
                {
                    var quotePost = _forumService.GetPostById(quote.Value);
                    if (quotePost != null && quotePost.TopicId == forumTopic.Id)
                    {
                        var quotePostText = quotePost.Text;

                        switch (_forumSettings.ForumEditor)
                        {
                            case EditorType.SimpleTextBox:
                                text = $"{_userService.FormatUserName(quotePost.User)}:\n{quotePostText}\n";
                                break;
                            case EditorType.BBCodeEditor:
                                text = $"[quote={_userService.FormatUserName(quotePost.User)}]{BBCodeHelper.RemoveQuotes(quotePostText)}[/quote]";
                                break;
                        }
                        model.Text = text;
                    }
                }
            }

            return model;
        }

        /// <summary>
        /// Prepare the forum post edit model
        /// </summary>
        /// <param name="forumPost">Forum post</param>
        /// <param name="excludeProperties">Whether to exclude populating of model properties from the entity</param>
        /// <returns>Edit forum post model</returns>
        public virtual EditForumPostModel PreparePostEditModel(ForumPost forumPost, bool excludeProperties)
        {
            if (forumPost == null)
                throw new ArgumentNullException(nameof(forumPost));

            var forumTopic = forumPost.ForumTopic;
            if (forumTopic == null)
                throw new ArgumentException("forum topic cannot be loaded");

            var forum = forumTopic.Forum;
            if (forum == null)
                throw new ArgumentException("forum cannot be loaded");

            var model = new EditForumPostModel
            {
                Id = forumPost.Id,
                ForumTopicId = forumTopic.Id,
                IsEdit = true,
                ForumEditor = _forumSettings.ForumEditor,
                ForumName = forum.Name,
                ForumTopicSubject = forumTopic.Subject,
                ForumTopicSeName = _forumService.GetTopicSeName(forumTopic),
                IsUserAllowedToSubscribe = _forumService.IsUserAllowedToSubscribe(_workContext.CurrentUser)
            };

            if (!excludeProperties)
            {
                model.Text = forumPost.Text;
                //subscription
                if (model.IsUserAllowedToSubscribe)
                {
                    var forumSubscription = _forumService.GetAllSubscriptions(_workContext.CurrentUser.Id, 0, forumTopic.Id, 0, 1).FirstOrDefault();
                    model.Subscribed = forumSubscription != null;
                }
            }

            return model;
        }

        /// <summary>
        /// Prepare the search model
        /// </summary>
        /// <param name="searchterms">Search terms</param>
        /// <param name="adv">Whether to use the advanced search</param>
        /// <param name="forumId">Forum identifier</param>
        /// <param name="within">String representation of int value of ForumSearchType</param>
        /// <param name="limitDays">Limit by the last number days; 0 to load all topics</param>
        /// <param name="page">Number of items page</param>
        /// <returns>Search model</returns>
        public virtual SearchModel PrepareSearchModel(string searchterms, bool? adv, string forumId,
            string within, string limitDays, int page)
        {
            var model = new SearchModel();

            var pageSize = 10;

            // Create the values for the "Limit results to previous" select list
            var limitList = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = _localizationService.GetResource("Forum.Search.LimitResultsToPrevious.AllResults"),
                    Value = "0"
                },
                new SelectListItem
                {
                    Text = _localizationService.GetResource("Forum.Search.LimitResultsToPrevious.1day"),
                    Value = "1"
                },
                new SelectListItem
                {
                    Text = _localizationService.GetResource("Forum.Search.LimitResultsToPrevious.7days"),
                    Value = "7"
                },
                new SelectListItem
                {
                    Text = _localizationService.GetResource("Forum.Search.LimitResultsToPrevious.2weeks"),
                    Value = "14"
                },
                new SelectListItem
                {
                    Text = _localizationService.GetResource("Forum.Search.LimitResultsToPrevious.1month"),
                    Value = "30"
                },
                new SelectListItem
                {
                    Text = _localizationService.GetResource("Forum.Search.LimitResultsToPrevious.3months"),
                    Value = "92"
                },
                new SelectListItem
                {
                    Text = _localizationService.GetResource("Forum.Search.LimitResultsToPrevious.6months"),
                    Value = "183"
                },
                new SelectListItem
                {
                    Text = _localizationService.GetResource("Forum.Search.LimitResultsToPrevious.1year"),
                    Value = "365"
                }
            };
            model.LimitList = limitList;

            // Create the values for the "Search in forum" select list
            var forumsSelectList = new List<SelectListItem>();
            forumsSelectList.Add(
                new SelectListItem
                {
                    Text = _localizationService.GetResource("Forum.Search.SearchInForum.All"),
                    Value = "0",
                    Selected = true,
                });
            var separator = "--";
            var forumGroups = _forumService.GetAllForumGroups();
            foreach (var fg in forumGroups)
            {
                // Add the forum group with value as '-' so it can't be used as a target forum id
                forumsSelectList.Add(new SelectListItem { Text = fg.Name, Value = "-" });

                var forums = _forumService.GetAllForumsByGroupId(fg.Id);
                foreach (var f in forums)
                {
                    forumsSelectList.Add(
                        new SelectListItem
                        {
                            Text = $"{separator}{f.Name}",
                            Value = f.Id.ToString()
                        });
                }
            }
            model.ForumList = forumsSelectList;

            // Create the values for "Search within" select list            
            var withinList = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = ((int) ForumSearchType.All).ToString(),
                    Text = _localizationService.GetResource("Forum.Search.SearchWithin.All")
                },
                new SelectListItem
                {
                    Value = ((int) ForumSearchType.TopicTitlesOnly).ToString(),
                    Text = _localizationService.GetResource("Forum.Search.SearchWithin.TopicTitlesOnly")
                },
                new SelectListItem
                {
                    Value = ((int) ForumSearchType.PostTextOnly).ToString(),
                    Text = _localizationService.GetResource("Forum.Search.SearchWithin.PostTextOnly")
                }
            };
            model.WithinList = withinList;

            int.TryParse(forumId, out int forumIdSelected);
            model.ForumIdSelected = forumIdSelected;

            int.TryParse(within, out int withinSelected);
            model.WithinSelected = withinSelected;

            int.TryParse(limitDays, out int limitDaysSelected);
            model.LimitDaysSelected = limitDaysSelected;

            var searchTermMinimumLength = _forumSettings.ForumSearchTermMinimumLength;

            model.ShowAdvancedSearch = adv.GetValueOrDefault();
            model.SearchResultsVisible = false;
            model.NoResultsVisisble = false;
            model.PostsPageSize = _forumSettings.PostsPageSize;

            model.AllowPostVoting = _forumSettings.AllowPostVoting;

            try
            {
                if (!string.IsNullOrWhiteSpace(searchterms))
                {
                    searchterms = searchterms.Trim();
                    model.SearchTerms = searchterms;

                    if (searchterms.Length < searchTermMinimumLength)
                    {
                        throw new AppException(string.Format(_localizationService.GetResource("Forum.SearchTermMinimumLengthIsNCharacters"),
                            searchTermMinimumLength));
                    }

                    ForumSearchType searchWithin = 0;
                    var limitResultsToPrevious = 0;
                    if (adv.GetValueOrDefault())
                    {
                        searchWithin = (ForumSearchType)withinSelected;
                        limitResultsToPrevious = limitDaysSelected;
                    }

                    if (_forumSettings.SearchResultsPageSize > 0)
                    {
                        pageSize = _forumSettings.SearchResultsPageSize;
                    }

                    var topics = _forumService.GetAllTopics(forumIdSelected, 0, searchterms, searchWithin,
                        limitResultsToPrevious, page - 1, pageSize);
                    model.TopicPageSize = topics.PageSize;
                    model.TopicTotalRecords = topics.TotalCount;
                    model.TopicPageIndex = topics.PageIndex;
                    foreach (var topic in topics)
                    {
                        var topicModel = PrepareForumTopicRowModel(topic);
                        model.ForumTopics.Add(topicModel);
                    }

                    model.SearchResultsVisible = (topics.Any());
                    model.NoResultsVisisble = !(model.SearchResultsVisible);

                    return model;
                }
                model.SearchResultsVisible = false;
            }
            catch (Exception ex)
            {
                model.Error = ex.Message;
            }

            //some exception raised
            model.TopicPageSize = pageSize;
            model.TopicTotalRecords = 0;
            model.TopicPageIndex = page - 1;

            return model;
        }

        /// <summary>
        /// Prepare the last post model
        /// </summary>
        /// <param name="forumPost">Forum post</param>
        /// <param name="showTopic">Whether to show topic</param>
        /// <returns>Last post model</returns>
        public virtual LastPostModel PrepareLastPostModel(ForumPost forumPost, bool showTopic)
        {
            var model = new LastPostModel
            {
                ShowTopic = showTopic
            };

            //do not throw an exception here
            if (forumPost == null)
                return model;

            model.Id = forumPost.Id;
            model.ForumTopicId = forumPost.TopicId;
            model.ForumTopicSeName = _forumService.GetTopicSeName(forumPost.ForumTopic);
            model.ForumTopicSubject = _forumService.StripTopicSubject(forumPost.ForumTopic);
            model.UserId = forumPost.UserId;
            model.AllowViewingProfiles = _userSettings.AllowViewingProfiles && !forumPost.User.IsGuest();
            model.UserName = _userService.FormatUserName(forumPost.User);
            //created on string
            if (_forumSettings.RelativeDateTimeFormattingEnabled)
                model.PostCreatedOnStr = forumPost.CreatedOnUtc.RelativeFormat(true, "f");
            else
                model.PostCreatedOnStr = _dateTimeHelper.ConvertToUserTime(forumPost.CreatedOnUtc, DateTimeKind.Utc).ToString("f");

            return model;
        }

        /// <summary>
        /// Prepare the forum breadcrumb model
        /// </summary>
        /// <param name="forumGroupId">Forum group identifier; pass null to load nothing</param>
        /// <param name="forumId">Forum identifier; pass null to load breadcrumbs up to forum group</param>
        /// <param name="forumTopicId">Forum topic identifier; pass null to load breadcrumbs up to forum</param>
        /// <returns>Forum breadcrumb model</returns>
        public virtual ForumBreadcrumbModel PrepareForumBreadcrumbModel(int? forumGroupId, int? forumId, int? forumTopicId)
        {
            var model = new ForumBreadcrumbModel();

            ForumTopic forumTopic = null;
            if (forumTopicId.HasValue)
            {
                forumTopic = _forumService.GetTopicById(forumTopicId.Value);
                if (forumTopic != null)
                {
                    model.ForumTopicId = forumTopic.Id;
                    model.ForumTopicSubject = forumTopic.Subject;
                    model.ForumTopicSeName = _forumService.GetTopicSeName(forumTopic);
                }
            }

            var forum = _forumService.GetForumById(forumTopic != null ? forumTopic.ForumId : (forumId.HasValue ? forumId.Value : 0));
            if (forum != null)
            {
                model.ForumId = forum.Id;
                model.ForumName = forum.Name;
                model.ForumSeName = _forumService.GetForumSeName(forum);
            }

            var forumGroup = _forumService.GetForumGroupById(forum != null ? forum.ForumGroupId : (forumGroupId.HasValue ? forumGroupId.Value : 0));
            if (forumGroup != null)
            {
                model.ForumGroupId = forumGroup.Id;
                model.ForumGroupName = forumGroup.Name;
                model.ForumGroupSeName = _forumService.GetForumGroupSeName(forumGroup);
            }

            return model;
        }

        /// <summary>
        /// Prepare the user forum subscriptions model
        /// </summary>
        /// <param name="page">Number of items page; pass null to load the first page</param>
        /// <returns>user forum subscriptions model</returns>
        public virtual UserForumSubscriptionsModel PrepareUserForumSubscriptionsModel(int? page)
        {
            var pageIndex = 0;
            if (page > 0)
            {
                pageIndex = page.Value - 1;
            }

            var user = _workContext.CurrentUser;

            var pageSize = _forumSettings.ForumSubscriptionsPageSize;

            var list = _forumService.GetAllSubscriptions(user.Id, 0, 0, pageIndex, pageSize);

            var model = new UserForumSubscriptionsModel();

            foreach (var forumSubscription in list)
            {
                var forumTopicId = forumSubscription.TopicId;
                var forumId = forumSubscription.ForumId;
                var topicSubscription = false;
                var title = string.Empty;
                var slug = string.Empty;

                if (forumTopicId > 0)
                {
                    topicSubscription = true;
                    var forumTopic = _forumService.GetTopicById(forumTopicId);
                    if (forumTopic != null)
                    {
                        title = forumTopic.Subject;
                        slug = _forumService.GetTopicSeName(forumTopic);
                    }
                }
                else
                {
                    var forum = _forumService.GetForumById(forumId);
                    if (forum != null)
                    {
                        title = forum.Name;
                        slug = _forumService.GetForumSeName(forum);
                    }
                }

                model.ForumSubscriptions.Add(new UserForumSubscriptionsModel.ForumSubscriptionModel
                {
                    Id = forumSubscription.Id,
                    ForumTopicId = forumTopicId,
                    ForumId = forumSubscription.ForumId,
                    TopicSubscription = topicSubscription,
                    Title = title,
                    Slug = slug,
                });
            }

            model.PagerModel = new PagerModel
            {
                PageSize = list.PageSize,
                TotalRecords = list.TotalCount,
                PageIndex = list.PageIndex,
                ShowTotalSummary = false,
                RouteActionName = "UserForumSubscriptionsPaged",
                UseRouteLinks = true,
                RouteValues = new ForumSubscriptionsRouteValues { pageNumber = pageIndex }
            };

            return model;
        }

        #endregion
    }
}