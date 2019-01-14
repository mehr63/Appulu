using System.Collections.Generic;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Models.News
{
    public partial class NewsItemListModel : BaseAppModel
    {
        public NewsItemListModel()
        {
            PagingFilteringContext = new NewsPagingFilteringModel();
            NewsItems = new List<NewsItemModel>();
        }

        public int WorkingLanguageId { get; set; }
        public NewsPagingFilteringModel PagingFilteringContext { get; set; }
        public IList<NewsItemModel> NewsItems { get; set; }
    }
}