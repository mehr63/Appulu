using Appulu.Web.Framework.Models;

namespace Appulu.Web.Models.Catalog
{
    public partial class SearchBoxModel : BaseAppModel
    {
        public bool AutoCompleteEnabled { get; set; }
        public bool ShowProductImagesInSearchAutoComplete { get; set; }
        public int SearchTermMinimumLength { get; set; }
    }
}