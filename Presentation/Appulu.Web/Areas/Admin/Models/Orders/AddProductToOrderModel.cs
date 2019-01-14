using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Appulu.Core.Domain.Catalog;
using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Orders
{
    /// <summary>
    /// Represents a product model to add to the order
    /// </summary>
    public partial class AddProductToOrderModel : BaseAppModel
    {
        #region Ctor

        public AddProductToOrderModel()
        {
            ProductAttributes = new List<ProductAttributeModel>();
            GiftCard = new GiftCardModel();
            Warnings = new List<string>();
        }

        #endregion

        #region Properties

        public int ProductId { get; set; }

        public int OrderId { get; set; }

        public ProductType ProductType { get; set; }

        public string Name { get; set; }

        [AppResourceDisplayName("Admin.Orders.Products.AddNew.UnitPriceInclTax")]
        public decimal UnitPriceInclTax { get; set; }
        [AppResourceDisplayName("Admin.Orders.Products.AddNew.UnitPriceExclTax")]
        public decimal UnitPriceExclTax { get; set; }

        [AppResourceDisplayName("Admin.Orders.Products.AddNew.Quantity")]
        public int Quantity { get; set; }

        [AppResourceDisplayName("Admin.Orders.Products.AddNew.SubTotalInclTax")]
        public decimal SubTotalInclTax { get; set; }
        [AppResourceDisplayName("Admin.Orders.Products.AddNew.SubTotalExclTax")]
        public decimal SubTotalExclTax { get; set; }

        //product attributes
        public IList<ProductAttributeModel> ProductAttributes { get; set; }
        //gift card info
        public GiftCardModel GiftCard { get; set; }
        //rental
        public bool IsRental { get; set; }

        public List<string> Warnings { get; set; }

        /// <summary>
        /// A value indicating whether this attribute depends on some other attribute
        /// </summary>
        public bool HasCondition { get; set; }

        public bool AutoUpdateOrderTotals { get; set; }

        #endregion

        #region Nested classes
        
        public partial class ProductAttributeModel : BaseAppEntityModel
        {
            public ProductAttributeModel()
            {
                Values = new List<ProductAttributeValueModel>();
            }

            public int ProductAttributeId { get; set; }

            public string Name { get; set; }

            public string TextPrompt { get; set; }

            public bool IsRequired { get; set; }

            public bool HasCondition { get; set; }

            /// <summary>
            /// Allowed file extensions for user uploaded files
            /// </summary>
            public IList<string> AllowedFileExtensions { get; set; }

            public AttributeControlType AttributeControlType { get; set; }

            public IList<ProductAttributeValueModel> Values { get; set; }
        }

        public partial class ProductAttributeValueModel : BaseAppEntityModel
        {
            public string Name { get; set; }

            public bool IsPreSelected { get; set; }

            public string PriceAdjustment { get; set; }

            public decimal PriceAdjustmentValue { get; set; }

            public bool UserEntersQty { get; set; }

            public int Quantity { get; set; }
        }

        public partial class GiftCardModel : BaseAppModel
        {
            public bool IsGiftCard { get; set; }

            [AppResourceDisplayName("Admin.GiftCards.Fields.RecipientName")]
            public string RecipientName { get; set; }
            [DataType(DataType.EmailAddress)]
            [AppResourceDisplayName("Admin.GiftCards.Fields.RecipientEmail")]
            public string RecipientEmail { get; set; }
            [AppResourceDisplayName("Admin.GiftCards.Fields.SenderName")]
            public string SenderName { get; set; }
            [DataType(DataType.EmailAddress)]
            [AppResourceDisplayName("Admin.GiftCards.Fields.SenderEmail")]
            public string SenderEmail { get; set; }
            [AppResourceDisplayName("Admin.GiftCards.Fields.Message")]
            public string Message { get; set; }

            public GiftCardType GiftCardType { get; set; }
        }

        #endregion
    }
}