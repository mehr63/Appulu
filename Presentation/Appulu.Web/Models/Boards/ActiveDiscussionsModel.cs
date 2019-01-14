﻿using System.Collections.Generic;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Models.Boards
{
    public partial class ActiveDiscussionsModel : BaseAppModel
    {
        public ActiveDiscussionsModel()
        {
            ForumTopics = new List<ForumTopicRowModel>();
        }

        public IList<ForumTopicRowModel> ForumTopics { get; private set; }

        public bool ViewAllLinkEnabled { get; set; }

        public bool ActiveDiscussionsFeedEnabled { get; set; }

        public int TopicPageSize { get; set; }
        public int TopicTotalRecords { get; set; }
        public int TopicPageIndex { get; set; }

        public int PostsPageSize { get; set; }

        public bool AllowPostVoting { get; set; }
    }
}