using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Settings
{
    /// <summary>
    /// Represents a product editor settings model
    /// </summary>
    public partial class ProductEditorSettingsModel : BaseAppModel, ISettingsModel
    {
        #region Properties

        public int ActiveStoreScopeConfiguration { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.Id")]
        public bool Id { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.ProductType")]
        public bool ProductType { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.VisibleIndividually")]
        public bool VisibleIndividually { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.ProductTemplate")]
        public bool ProductTemplate { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.AdminComment")]
        public bool AdminComment { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.Vendor")]
        public bool Vendor { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.Stores")]
        public bool Stores { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.ACL")]
        public bool ACL { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.ShowOnHomePage")]
        public bool ShowOnHomePage { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.DisplayOrder")]
        public bool DisplayOrder { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.AllowUserReviews")]
        public bool AllowUserReviews { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.ProductTags")]
        public bool ProductTags { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.ManufacturerPartNumber")]
        public bool ManufacturerPartNumber { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.GTIN")]
        public bool GTIN { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.ProductCost")]
        public bool ProductCost { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.TierPrices")]
        public bool TierPrices { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.Discounts")]
        public bool Discounts { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.DisableBuyButton")]
        public bool DisableBuyButton { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.DisableWishlistButton")]
        public bool DisableWishlistButton { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.AvailableForPreOrder")]
        public bool AvailableForPreOrder { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.CallForPrice")]
        public bool CallForPrice { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.OldPrice")]
        public bool OldPrice { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.UserEntersPrice")]
        public bool UserEntersPrice { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.PAngV")]
        public bool PAngV { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.RequireOtherProductsAddedToTheCart")]
        public bool RequireOtherProductsAddedToTheCart { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.IsGiftCard")]
        public bool IsGiftCard { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.DownloadableProduct")]
        public bool DownloadableProduct { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.RecurringProduct")]
        public bool RecurringProduct { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.IsRental")]
        public bool IsRental { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.FreeShipping")]
        public bool FreeShipping { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.ShipSeparately")]
        public bool ShipSeparately { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.AdditionalShippingCharge")]
        public bool AdditionalShippingCharge { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.DeliveryDate")]
        public bool DeliveryDate { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.TelecommunicationsBroadcastingElectronicServices")]
        public bool TelecommunicationsBroadcastingElectronicServices { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.ProductAvailabilityRange")]
        public bool ProductAvailabilityRange { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.UseMultipleWarehouses")]
        public bool UseMultipleWarehouses { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.Warehouse")]
        public bool Warehouse { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.DisplayStockAvailability")]
        public bool DisplayStockAvailability { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.DisplayStockQuantity")]
        public bool DisplayStockQuantity { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.MinimumStockQuantity")]
        public bool MinimumStockQuantity { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.LowStockActivity")]
        public bool LowStockActivity { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.NotifyAdminForQuantityBelow")]
        public bool NotifyAdminForQuantityBelow { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.Backorders")]
        public bool Backorders { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.AllowBackInStockSubscriptions")]
        public bool AllowBackInStockSubscriptions { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.MinimumCartQuantity")]
        public bool MinimumCartQuantity { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.MaximumCartQuantity")]
        public bool MaximumCartQuantity { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.AllowedQuantities")]
        public bool AllowedQuantities { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.AllowAddingOnlyExistingAttributeCombinations")]
        public bool AllowAddingOnlyExistingAttributeCombinations { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.NotReturnable")]
        public bool NotReturnable { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.Weight")]
        public bool Weight { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.Dimensions")]
        public bool Dimensions { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.AvailableStartDate")]
        public bool AvailableStartDate { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.AvailableEndDate")]
        public bool AvailableEndDate { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.MarkAsNew")]
        public bool MarkAsNew { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.MarkAsNewStartDate")]
        public bool MarkAsNewStartDate { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.MarkAsNewEndDate")]
        public bool MarkAsNewEndDate { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.Published")]
        public bool Published { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.CreatedOn")]
        public bool CreatedOn { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.UpdatedOn")]
        public bool UpdatedOn { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.RelatedProducts")]
        public bool RelatedProducts { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.CrossSellsProducts")]
        public bool CrossSellsProducts { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.Seo")]
        public bool Seo { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.PurchasedWithOrders")]
        public bool PurchasedWithOrders { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.OneColumnProductPage")]
        public bool OneColumnProductPage { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.ProductAttributes")]
        public bool ProductAttributes { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.SpecificationAttributes")]
        public bool SpecificationAttributes { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.Manufacturers")]
        public bool Manufacturers { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.ProductEditor.StockQuantityHistory")]
        public bool StockQuantityHistory { get; set; }

        #endregion
    }
}