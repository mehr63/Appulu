using System;
using System.ComponentModel.DataAnnotations;
using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Catalog
{
    /// <summary>
    /// Represents a stock quantity history model
    /// </summary>
    public partial class StockQuantityHistoryModel : BaseAppEntityModel
    {
        #region Properties

        [AppResourceDisplayName("Admin.Catalog.Products.StockQuantityHistory.Fields.Warehouse")]
        public string WarehouseName { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Products.StockQuantityHistory.Fields.Combination")]
        public string AttributeCombination { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Products.StockQuantityHistory.Fields.QuantityAdjustment")]
        public int QuantityAdjustment { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Products.StockQuantityHistory.Fields.StockQuantity")]
        public int StockQuantity { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Products.StockQuantityHistory.Fields.Message")]
        public string Message { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Products.StockQuantityHistory.Fields.CreatedOn")]
        [UIHint("DecimalNullable")]
        public DateTime CreatedOn { get; set; }

        #endregion
    }
}