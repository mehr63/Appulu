﻿using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Settings
{
    /// <summary>
    /// Represents a blog settings model
    /// </summary>
    public partial class BlogSettingsModel : BaseAppModel, ISettingsModel
    {
        #region Properties

        public int ActiveStoreScopeConfiguration { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.Blog.Enabled")]
        public bool Enabled { get; set; }
        public bool Enabled_OverrideForStore { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.Blog.PostsPageSize")]
        public int PostsPageSize { get; set; }
        public bool PostsPageSize_OverrideForStore { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.Blog.AllowNotRegisteredUsersToLeaveComments")]
        public bool AllowNotRegisteredUsersToLeaveComments { get; set; }
        public bool AllowNotRegisteredUsersToLeaveComments_OverrideForStore { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.Blog.NotifyAboutNewBlogComments")]
        public bool NotifyAboutNewBlogComments { get; set; }
        public bool NotifyAboutNewBlogComments_OverrideForStore { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.Blog.NumberOfTags")]
        public int NumberOfTags { get; set; }
        public bool NumberOfTags_OverrideForStore { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.Blog.ShowHeaderRSSUrl")]
        public bool ShowHeaderRssUrl { get; set; }
        public bool ShowHeaderRssUrl_OverrideForStore { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.Blog.BlogCommentsMustBeApproved")]
        public bool BlogCommentsMustBeApproved { get; set; }
        public bool BlogCommentsMustBeApproved_OverrideForStore { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.Blog.ShowBlogCommentsPerStore")]
        public bool ShowBlogCommentsPerStore { get; set; }

        #endregion
    }
}