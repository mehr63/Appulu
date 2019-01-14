using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Discounts
{
    /// <summary>
    /// Represents a discount product model
    /// </summary>
    public partial class DiscountProductModel : BaseAppEntityModel
    {
        #region Properties

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        #endregion
    }
}