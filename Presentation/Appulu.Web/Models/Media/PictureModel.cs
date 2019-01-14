﻿using Appulu.Web.Framework.Models;

namespace Appulu.Web.Models.Media
{
    public partial class PictureModel : BaseAppModel
    {
        public string ImageUrl { get; set; }

        public string ThumbImageUrl { get; set; }

        public string FullSizeImageUrl { get; set; }

        public string Title { get; set; }

        public string AlternateText { get; set; }
    }
}