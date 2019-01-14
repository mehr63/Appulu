using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Common
{
    /// <summary>
    /// Represents an URL record model
    /// </summary>
    public partial class UrlRecordModel : BaseAppEntityModel
    {
        #region Properties

        [AppResourceDisplayName("Admin.System.SeNames.Name")]
        public string Name { get; set; }

        [AppResourceDisplayName("Admin.System.SeNames.EntityId")]
        public int EntityId { get; set; }

        [AppResourceDisplayName("Admin.System.SeNames.EntityName")]
        public string EntityName { get; set; }

        [AppResourceDisplayName("Admin.System.SeNames.IsActive")]
        public bool IsActive { get; set; }

        [AppResourceDisplayName("Admin.System.SeNames.Language")]
        public string Language { get; set; }

        [AppResourceDisplayName("Admin.System.SeNames.Details")]
        public string DetailsUrl { get; set; }

        #endregion
    }
}