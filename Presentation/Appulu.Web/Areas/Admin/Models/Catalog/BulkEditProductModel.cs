using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Catalog
{
    /// <summary>
    /// Represents a bulk edit product model
    /// </summary>
    public partial class BulkEditProductModel : BaseAppEntityModel
    {
        #region Properties

        [AppResourceDisplayName("Admin.Catalog.BulkEdit.Fields.Name")]
        public string Name { get; set; }

        [AppResourceDisplayName("Admin.Catalog.BulkEdit.Fields.SKU")]
        public string Sku { get; set; }

        [AppResourceDisplayName("Admin.Catalog.BulkEdit.Fields.Price")]
        public decimal Price { get; set; }

        [AppResourceDisplayName("Admin.Catalog.BulkEdit.Fields.OldPrice")]
        public decimal OldPrice { get; set; }

        [AppResourceDisplayName("Admin.Catalog.BulkEdit.Fields.ManageInventoryMethod")]
        public string ManageInventoryMethod { get; set; }

        [AppResourceDisplayName("Admin.Catalog.BulkEdit.Fields.StockQuantity")]
        public int StockQuantity { get; set; }

        [AppResourceDisplayName("Admin.Catalog.BulkEdit.Fields.Published")]
        public bool Published { get; set; }

        #endregion
    }
}