﻿using System;
using FluentValidation.Attributes;
using Appulu.Web.Areas.Admin.Validators.Catalog;
using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Catalog
{
    /// <summary>
    /// Represents a product review model
    /// </summary>
    [Validator(typeof(ProductReviewValidator))]
    public partial class ProductReviewModel : BaseAppEntityModel
    {
        #region Ctor

        public ProductReviewModel()
        {
            ProductReviewReviewTypeMappingSearchModel = new ProductReviewReviewTypeMappingSearchModel();            
        }

        #endregion

        #region Properties

        [AppResourceDisplayName("Admin.Catalog.ProductReviews.Fields.Store")]
        public string StoreName { get; set; }

        [AppResourceDisplayName("Admin.Catalog.ProductReviews.Fields.Product")]
        public int ProductId { get; set; }

        [AppResourceDisplayName("Admin.Catalog.ProductReviews.Fields.Product")]
        public string ProductName { get; set; }

        [AppResourceDisplayName("Admin.Catalog.ProductReviews.Fields.User")]
        public int UserId { get; set; }

        [AppResourceDisplayName("Admin.Catalog.ProductReviews.Fields.User")]
        public string UserInfo { get; set; }
        
        [AppResourceDisplayName("Admin.Catalog.ProductReviews.Fields.Title")]
        public string Title { get; set; }
        
        [AppResourceDisplayName("Admin.Catalog.ProductReviews.Fields.ReviewText")]
        public string ReviewText { get; set; }
        
        [AppResourceDisplayName("Admin.Catalog.ProductReviews.Fields.ReplyText")]
        public string ReplyText { get; set; }

        [AppResourceDisplayName("Admin.Catalog.ProductReviews.Fields.Rating")]
        public int Rating { get; set; }

        [AppResourceDisplayName("Admin.Catalog.ProductReviews.Fields.IsApproved")]
        public bool IsApproved { get; set; }

        [AppResourceDisplayName("Admin.Catalog.ProductReviews.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        //vendor
        public bool IsLoggedInAsVendor { get; set; }

        public ProductReviewReviewTypeMappingSearchModel ProductReviewReviewTypeMappingSearchModel { get; set; }

        #endregion
    }
}