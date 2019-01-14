using System.Collections.Generic;
using FluentValidation.Attributes;
using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Validators.Catalog;

namespace Appulu.Web.Models.Catalog
{
    public partial class ProductReviewOverviewModel : BaseAppModel
    {
        public int ProductId { get; set; }

        public int RatingSum { get; set; }

        public int TotalReviews { get; set; }

        public bool AllowUserReviews { get; set; }
    }

    [Validator(typeof(ProductReviewsValidator))]
    public partial class ProductReviewsModel : BaseAppModel
    {
        public ProductReviewsModel()
        {
            Items = new List<ProductReviewModel>();
            AddProductReview = new AddProductReviewModel();
            ReviewTypeList = new List<ReviewTypeModel>();
            AddAdditionalProductReviewList = new List<AddProductReviewReviewTypeMappingModel>();
        }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductSeName { get; set; }

        public IList<ProductReviewModel> Items { get; set; }

        public AddProductReviewModel AddProductReview { get; set; }

        public IList<ReviewTypeModel> ReviewTypeList { get; set; }

        public IList<AddProductReviewReviewTypeMappingModel> AddAdditionalProductReviewList { get; set; }        
    }

    public partial class ReviewTypeModel : BaseAppEntityModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsRequired { get; set; }

        public bool VisibleToAllUsers { get; set; }

        public double AverageRating { get; set; }
    }

    public partial class ProductReviewModel : BaseAppEntityModel
    {
        public ProductReviewModel()
        {
            AdditionalProductReviewList = new List<ProductReviewReviewTypeMappingModel>();
        }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public bool AllowViewingProfiles { get; set; }
        
        public string Title { get; set; }

        public string ReviewText { get; set; }

        public string ReplyText { get; set; }

        public int Rating { get; set; }

        public string WrittenOnStr { get; set; }

        public ProductReviewHelpfulnessModel Helpfulness { get; set; }

        public IList<ProductReviewReviewTypeMappingModel> AdditionalProductReviewList { get; set; }
    }

    public partial class ProductReviewHelpfulnessModel : BaseAppModel
    {
        public int ProductReviewId { get; set; }

        public int HelpfulYesTotal { get; set; }

        public int HelpfulNoTotal { get; set; }
    }

    public partial class AddProductReviewModel : BaseAppModel
    {
        [AppResourceDisplayName("Reviews.Fields.Title")]
        public string Title { get; set; }
        
        [AppResourceDisplayName("Reviews.Fields.ReviewText")]
        public string ReviewText { get; set; }

        [AppResourceDisplayName("Reviews.Fields.Rating")]
        public int Rating { get; set; }

        public bool DisplayCaptcha { get; set; }

        public bool CanCurrentUserLeaveReview { get; set; }

        public bool SuccessfullyAdded { get; set; }

        public string Result { get; set; }
    }

    public partial class AddProductReviewReviewTypeMappingModel : BaseAppEntityModel
    {
        public int ProductReviewId { get; set; }

        public int ReviewTypeId { get; set; }

        public int Rating { get; set; }
        
        public string Name { get; set; }

        public string Description { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsRequired { get; set; }
    }

    public partial class ProductReviewReviewTypeMappingModel : BaseAppEntityModel
    {
        public int ProductReviewId { get; set; }

        public int ReviewTypeId { get; set; }

        public int Rating { get; set; }

        public string Name { get; set; }

        public bool VisibleToAllUsers { get; set; }
    }
}