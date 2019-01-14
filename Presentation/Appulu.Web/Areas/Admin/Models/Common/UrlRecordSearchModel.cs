using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Common
{
    /// <summary>
    /// Represents an URL record search model
    /// </summary>
    public partial class UrlRecordSearchModel : BaseSearchModel
    {
        #region Properties

        [AppResourceDisplayName("Admin.System.SeNames.Name")]
        public string SeName { get; set; }

        #endregion
    }
}