using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Affiliates
{
    /// <summary>
    /// Represents an affiliated user model
    /// </summary>
    public partial class AffiliatedUserModel : BaseAppEntityModel
    {
        #region Properties

        [AppResourceDisplayName("Admin.Affiliates.Users.Name")]
        public string Name { get; set; }

        #endregion
    }
}