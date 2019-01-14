using System.Collections.Generic;
using Appulu.Web.Framework.Models;
using Appulu.Web.Models.Common;

namespace Appulu.Web.Models.Catalog
{
    public class UserProductReviewModel : BaseAppModel
    {
        public UserProductReviewModel()
        {
            AdditionalProductReviewList = new List<ProductReviewReviewTypeMappingModel>();
        }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSeName { get; set; }
        public string Title { get; set; }
        public string ReviewText { get; set; }
        public string ReplyText { get; set; }
        public int Rating { get; set; }
        public string WrittenOnStr { get; set; }
        public string ApprovalStatus { get; set; }
        public IList<ProductReviewReviewTypeMappingModel> AdditionalProductReviewList { get; set; }
    }

    public class UserProductReviewsModel : BaseAppModel
    {
        public UserProductReviewsModel()
        {
            ProductReviews = new List<UserProductReviewModel>();
        }

        public IList<UserProductReviewModel> ProductReviews { get; set; }
        public PagerModel PagerModel { get; set; }

        #region Nested class

        /// <summary>
        /// Class that has only page for route value. Used for (My Account) My Product Reviews pagination
        /// </summary>
        public partial class UserProductReviewsRouteValues : IRouteValues
        {
            public int pageNumber { get; set; }
        }

        #endregion
    }
}