using System;
using System.Collections.Generic;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Models.News
{
    public partial class HomePageNewsItemsModel : BaseAppModel, ICloneable
    {
        public HomePageNewsItemsModel()
        {
            NewsItems = new List<NewsItemModel>();
        }

        public int WorkingLanguageId { get; set; }
        public IList<NewsItemModel> NewsItems { get; set; }

        public object Clone()
        {
            //we use a shallow copy (deep clone is not required here)
            return MemberwiseClone();
        }
    }
}