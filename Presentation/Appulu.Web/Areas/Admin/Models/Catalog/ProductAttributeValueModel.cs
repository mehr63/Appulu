using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using Appulu.Web.Areas.Admin.Validators.Catalog;
using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Catalog
{
    /// <summary>
    /// Represents a product attribute value model
    /// </summary>
    [Validator(typeof(ProductAttributeValueModelValidator))]
    public partial class ProductAttributeValueModel : BaseAppEntityModel, ILocalizedModel<ProductAttributeValueLocalizedModel>
    {
        #region Ctor

        public ProductAttributeValueModel()
        {
            ProductPictureModels = new List<ProductPictureModel>();
            Locales = new List<ProductAttributeValueLocalizedModel>();
        }

        #endregion

        #region Properties

        public int ProductAttributeMappingId { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Products.ProductAttributes.Attributes.Values.Fields.AttributeValueType")]
        public int AttributeValueTypeId { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Products.ProductAttributes.Attributes.Values.Fields.AttributeValueType")]
        public string AttributeValueTypeName { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Products.ProductAttributes.Attributes.Values.Fields.AssociatedProduct")]
        public int AssociatedProductId { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Products.ProductAttributes.Attributes.Values.Fields.AssociatedProduct")]
        public string AssociatedProductName { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Products.ProductAttributes.Attributes.Values.Fields.Name")]
        public string Name { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Products.ProductAttributes.Attributes.Values.Fields.ColorSquaresRgb")]
        public string ColorSquaresRgb { get; set; }

        public bool DisplayColorSquaresRgb { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Products.ProductAttributes.Attributes.Values.Fields.ImageSquaresPicture")]
        [UIHint("Picture")]
        public int ImageSquaresPictureId { get; set; }

        public bool DisplayImageSquaresPicture { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Products.ProductAttributes.Attributes.Values.Fields.PriceAdjustment")]
        public decimal PriceAdjustment { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Products.ProductAttributes.Attributes.Values.Fields.PriceAdjustment")]
        //used only on the values list page
        public string PriceAdjustmentStr { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Products.ProductAttributes.Attributes.Values.Fields.PriceAdjustmentUsePercentage")]
        public bool PriceAdjustmentUsePercentage { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Products.ProductAttributes.Attributes.Values.Fields.WeightAdjustment")]
        public decimal WeightAdjustment { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Products.ProductAttributes.Attributes.Values.Fields.WeightAdjustment")]
        //used only on the values list page
        public string WeightAdjustmentStr { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Products.ProductAttributes.Attributes.Values.Fields.Cost")]
        public decimal Cost { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Products.ProductAttributes.Attributes.Values.Fields.UserEntersQty")]
        public bool UserEntersQty { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Products.ProductAttributes.Attributes.Values.Fields.Quantity")]
        public int Quantity { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Products.ProductAttributes.Attributes.Values.Fields.IsPreSelected")]
        public bool IsPreSelected { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Products.ProductAttributes.Attributes.Values.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Products.ProductAttributes.Attributes.Values.Fields.Picture")]
        public int PictureId { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Products.ProductAttributes.Attributes.Values.Fields.Picture")]
        public string PictureThumbnailUrl { get; set; }

        public IList<ProductPictureModel> ProductPictureModels { get; set; }

        public IList<ProductAttributeValueLocalizedModel> Locales { get; set; }

        #endregion
    }

    public partial class ProductAttributeValueLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Products.ProductAttributes.Attributes.Values.Fields.Name")]
        public string Name { get; set; }
    }
}