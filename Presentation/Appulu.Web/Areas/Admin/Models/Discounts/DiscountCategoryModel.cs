using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Discounts
{
    /// <summary>
    /// Represents a discount category model
    /// </summary>
    public partial class DiscountCategoryModel : BaseAppEntityModel
    {
        #region Properties

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        #endregion
    }
}