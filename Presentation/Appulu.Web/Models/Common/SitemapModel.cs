﻿using System.Collections.Generic;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Models.Common
{
    public partial class SitemapModel : BaseAppModel
    {
        #region Ctor

        public SitemapModel()
        {
            Items = new List<SitemapItemModel>();
            PageModel = new SitemapPageModel();
        }

        #endregion

        #region Properties

        public List<SitemapItemModel> Items { get; set; }

        public SitemapPageModel PageModel { get; set; }

        #endregion

        #region Nested classes

        public class SitemapItemModel
        {
            public string GroupTitle { get; set; }
            public string Url { get; set; }
            public string Name { get; set; }
        }

        #endregion
    }
}