using System;
using System.Collections.Generic;
using Appulu.Core.Domain.Users;
using Appulu.Core.Domain.Forums;
using Appulu.Core.Domain.Media;
using Appulu.Services.Common;
using Appulu.Services.Users;
using Appulu.Services.Directory;
using Appulu.Services.Forums;
using Appulu.Services.Helpers;
using Appulu.Services.Localization;
using Appulu.Services.Media;
using Appulu.Web.Framework.Extensions;
using Appulu.Web.Models.Common;
using Appulu.Web.Models.Profile;

namespace Appulu.Web.Factories
{
    /// <summary>
    /// Represents the profile model factory
    /// </summary>
    public partial class ProfileModelFactory : IProfileModelFactory
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
        private readonly MediaSettings _mediaSettings;

        #endregion

        #region Ctor

        public ProfileModelFactory(UserSettings userSettings,
            ForumSettings forumSettings,
            ICountryService countryService,
            IUserService userService,
            IDateTimeHelper dateTimeHelper,
            IForumService forumService,
            IGenericAttributeService genericAttributeService,
            ILocalizationService localizationService,
            IPictureService pictureService,
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
            this._mediaSettings = mediaSettings;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare the profile index model
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="page">Number of posts page; pass null to disable paging</param>
        /// <returns>Profile index model</returns>
        public virtual ProfileIndexModel PrepareProfileIndexModel(User user, int? page)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var pagingPosts = false;
            var postsPage = 0;

            if (page.HasValue)
            {
                postsPage = page.Value;
                pagingPosts = true;
            }

            var name = _userService.FormatUserName(user);
            var title = string.Format(_localizationService.GetResource("Profile.ProfileOf"), name);

            var model = new ProfileIndexModel
            {
                ProfileTitle = title,
                PostsPage = postsPage,
                PagingPosts = pagingPosts,
                UserProfileId = user.Id,
                ForumsEnabled = _forumSettings.ForumsEnabled
            };
            return model;
        }

        /// <summary>
        /// Prepare the profile info model
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Profile info model</returns>
        public virtual ProfileInfoModel PrepareProfileInfoModel(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            //avatar
            var avatarUrl = "";
            if (_userSettings.AllowUsersToUploadAvatars)
            {
                avatarUrl = _pictureService.GetPictureUrl(
                 _genericAttributeService.GetAttribute<int>(user, AppUserDefaults.AvatarPictureIdAttribute),
                 _mediaSettings.AvatarPictureSize,
                 _userSettings.DefaultAvatarEnabled,
                 defaultPictureType: PictureType.Avatar);
            }

            //location
            var locationEnabled = false;
            var location = string.Empty;
            if (_userSettings.ShowUsersLocation)
            {
                locationEnabled = true;

                var countryId = _genericAttributeService.GetAttribute<int>(user, AppUserDefaults.CountryIdAttribute);
                var country = _countryService.GetCountryById(countryId);
                if (country != null)
                {
                    location = _localizationService.GetLocalized(country, x => x.Name);
                }
                else
                {
                    locationEnabled = false;
                }
            }

            //private message
            var pmEnabled = _forumSettings.AllowPrivateMessages && !user.IsGuest();

            //total forum posts
            var totalPostsEnabled = false;
            var totalPosts = 0;
            if (_forumSettings.ForumsEnabled && _forumSettings.ShowUsersPostCount)
            {
                totalPostsEnabled = true;
                totalPosts = _genericAttributeService.GetAttribute<int>(user, AppUserDefaults.ForumPostCountAttribute);
            }

            //registration date
            var joinDateEnabled = false;
            var joinDate = string.Empty;

            if (_userSettings.ShowUsersJoinDate)
            {
                joinDateEnabled = true;
                joinDate = _dateTimeHelper.ConvertToUserTime(user.CreatedOnUtc, DateTimeKind.Utc).ToString("f");
            }

            //birth date
            var dateOfBirthEnabled = false;
            var dateOfBirth = string.Empty;
            if (_userSettings.DateOfBirthEnabled)
            {
                var dob = _genericAttributeService.GetAttribute<DateTime?>(user, AppUserDefaults.DateOfBirthAttribute);
                if (dob.HasValue)
                {
                    dateOfBirthEnabled = true;
                    dateOfBirth = dob.Value.ToString("D");
                }
            }

            var model = new ProfileInfoModel
            {
                UserProfileId = user.Id,
                AvatarUrl = avatarUrl,
                LocationEnabled = locationEnabled,
                Location = location,
                PMEnabled = pmEnabled,
                TotalPostsEnabled = totalPostsEnabled,
                TotalPosts = totalPosts.ToString(),
                JoinDateEnabled = joinDateEnabled,
                JoinDate = joinDate,
                DateOfBirthEnabled = dateOfBirthEnabled,
                DateOfBirth = dateOfBirth,
            };

            return model;
        }

        /// <summary>
        /// Prepare the profile posts model
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="page">Number of posts page</param>
        /// <returns>Profile posts model</returns>
        public virtual ProfilePostsModel PrepareProfilePostsModel(User user, int page)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (page > 0)
            {
                page -= 1;
            }

            var pageSize = _forumSettings.LatestUserPostsPageSize;

            var list = _forumService.GetAllPosts(0, user.Id, string.Empty, false, page, pageSize);

            var latestPosts = new List<PostsModel>();

            foreach (var forumPost in list)
            {
                var posted = string.Empty;
                if (_forumSettings.RelativeDateTimeFormattingEnabled)
                {
                    posted = forumPost.CreatedOnUtc.RelativeFormat(true, "f");
                }
                else
                {
                    posted = _dateTimeHelper.ConvertToUserTime(forumPost.CreatedOnUtc, DateTimeKind.Utc).ToString("f");
                }

                latestPosts.Add(new PostsModel
                {
                    ForumTopicId = forumPost.TopicId,
                    ForumTopicTitle = forumPost.ForumTopic.Subject,
                    ForumTopicSlug = _forumService.GetTopicSeName(forumPost.ForumTopic),
                    ForumPostText = _forumService.FormatPostText(forumPost),
                    Posted = posted
                });
            }

            var pagerModel = new PagerModel
            {
                PageSize = list.PageSize,
                TotalRecords = list.TotalCount,
                PageIndex = list.PageIndex,
                ShowTotalSummary = false,
                RouteActionName = "UserProfilePaged",
                UseRouteLinks = true,
                RouteValues = new RouteValues { pageNumber = page, id = user.Id }
            };

            var model = new ProfilePostsModel
            {
                PagerModel = pagerModel,
                Posts = latestPosts,
            };

            return model;
        }

        #endregion
    }
}