using Appulu.Web.Framework.Models;

namespace Appulu.Web.Models.Catalog
{
    public partial class ProductTagModel : BaseAppEntityModel
    {
        public string Name { get; set; }

        public string SeName { get; set; }

        public int ProductCount { get; set; }
    }
}