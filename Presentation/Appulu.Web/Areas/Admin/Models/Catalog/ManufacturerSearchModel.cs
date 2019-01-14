using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Catalog
{
    /// <summary>
    /// Represents a manufacturer search model
    /// </summary>
    public partial class ManufacturerSearchModel : BaseSearchModel
    {
        #region Ctor

        public ManufacturerSearchModel()
        {
            AvailableStores = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [AppResourceDisplayName("Admin.Catalog.Manufacturers.List.SearchManufacturerName")]
        public string SearchManufacturerName { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Manufacturers.List.SearchStore")]
        public int SearchStoreId { get; set; }

        public IList<SelectListItem> AvailableStores { get; set; }

        #endregion
    }
}