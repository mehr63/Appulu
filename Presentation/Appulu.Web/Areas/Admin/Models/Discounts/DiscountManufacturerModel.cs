using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Discounts
{
    /// <summary>
    /// Represents a discount manufacturer model
    /// </summary>
    public partial class DiscountManufacturerModel : BaseAppEntityModel
    {
        #region Properties

        public int ManufacturerId { get; set; }

        public string ManufacturerName { get; set; }

        #endregion
    }
}