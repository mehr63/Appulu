using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Tax
{
    /// <summary>
    /// Represents a tax configuration model
    /// </summary>
    public partial class TaxConfigurationModel : BaseAppModel
    {
        #region Ctor

        public TaxConfigurationModel()
        {
            TaxProviders = new TaxProviderSearchModel();
            TaxCategories = new TaxCategorySearchModel();
        }

        #endregion

        #region Properties

        public TaxProviderSearchModel TaxProviders { get; set; }

        public TaxCategorySearchModel TaxCategories { get; set; }

        #endregion
    }
}
