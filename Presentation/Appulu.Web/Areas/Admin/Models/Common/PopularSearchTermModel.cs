using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Common
{
    /// <summary>
    /// Represents a popular search term model
    /// </summary>
    public partial class PopularSearchTermModel : BaseAppModel
    {
        #region Properties

        [AppResourceDisplayName("Admin.SearchTermReport.Keyword")]
        public string Keyword { get; set; }

        [AppResourceDisplayName("Admin.SearchTermReport.Count")]
        public int Count { get; set; }

        #endregion
    }
}
