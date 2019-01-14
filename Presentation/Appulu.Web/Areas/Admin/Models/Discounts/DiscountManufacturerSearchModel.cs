using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Discounts
{
    /// <summary>
    /// Represents a discount manufacturer search model
    /// </summary>
    public partial class DiscountManufacturerSearchModel : BaseSearchModel
    {
        #region Properties

        public int DiscountId { get; set; }

        #endregion
    }
}