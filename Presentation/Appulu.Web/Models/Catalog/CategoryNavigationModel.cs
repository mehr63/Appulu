using System.Collections.Generic;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Models.Catalog
{
    public partial class CategoryNavigationModel : BaseAppModel
    {
        public CategoryNavigationModel()
        {
            Categories = new List<CategorySimpleModel>();
        }

        public int CurrentCategoryId { get; set; }
        public List<CategorySimpleModel> Categories { get; set; }

        #region Nested classes

        public class CategoryLineModel : BaseAppModel
        {
            public int CurrentCategoryId { get; set; }
            public CategorySimpleModel Category { get; set; }
        }

        #endregion
    }
}