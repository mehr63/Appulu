using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Affiliates
{
    /// <summary>
    /// Represents an affiliated user search model
    /// </summary>
    public partial class AffiliatedUserSearchModel : BaseSearchModel
    {
        #region Properties

        public int AffliateId { get; set; }

        #endregion
    }
}