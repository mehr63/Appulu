using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Discounts
{
    /// <summary>
    /// Represents a category search model to add to the discount
    /// </summary>
    public partial class AddCategoryToDiscountSearchModel : BaseSearchModel
    {
        #region Properties

        [AppResourceDisplayName("Admin.Catalog.Categories.List.SearchCategoryName")]
        public string SearchCategoryName { get; set; }

        #endregion
    }
}