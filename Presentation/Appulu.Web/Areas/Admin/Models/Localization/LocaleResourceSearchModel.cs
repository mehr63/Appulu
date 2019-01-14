using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Localization
{
    /// <summary>
    /// Represents a locale resource search model
    /// </summary>
    public partial class LocaleResourceSearchModel : BaseSearchModel
    {
        #region Properties
        
        public int LanguageId { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Languages.Resources.SearchResourceName")]
        public string SearchResourceName { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Languages.Resources.SearchResourceValue")]
        public string SearchResourceValue { get; set; }

        #endregion
    }
}