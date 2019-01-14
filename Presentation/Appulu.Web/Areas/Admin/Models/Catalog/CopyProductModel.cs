using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Catalog
{
    /// <summary>
    /// Represents a copy product model
    /// </summary>
    public partial class CopyProductModel : BaseAppEntityModel
    {
        #region Properties

        [AppResourceDisplayName("Admin.Catalog.Products.Copy.Name")]
        public string Name { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Products.Copy.CopyImages")]
        public bool CopyImages { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Products.Copy.Published")]
        public bool Published { get; set; }

        #endregion
    }
}