using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Catalog
{
    /// <summary>
    /// Represents a product warehouse inventory model
    /// </summary>
    public partial class ProductWarehouseInventoryModel : BaseAppModel
    {
        #region Properties

        [AppResourceDisplayName("Admin.Catalog.Products.ProductWarehouseInventory.Fields.Warehouse")]
        public int WarehouseId { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Products.ProductWarehouseInventory.Fields.Warehouse")]
        public string WarehouseName { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Products.ProductWarehouseInventory.Fields.WarehouseUsed")]
        public bool WarehouseUsed { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Products.ProductWarehouseInventory.Fields.StockQuantity")]
        public int StockQuantity { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Products.ProductWarehouseInventory.Fields.ReservedQuantity")]
        public int ReservedQuantity { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Products.ProductWarehouseInventory.Fields.PlannedQuantity")]
        public int PlannedQuantity { get; set; }

        #endregion
    }
}