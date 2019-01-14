using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Discounts
{
    /// <summary>
    /// Represents a manufacturer search model to add to the discount
    /// </summary>
    public partial class AddManufacturerToDiscountSearchModel : BaseSearchModel
    {
        #region Properties

        [AppResourceDisplayName("Admin.Catalog.Manufacturers.List.SearchManufacturerName")]
        public string SearchManufacturerName { get; set; }

        #endregion
    }
}