using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Vendors
{
    /// <summary>
    /// Represents a vendor associated user model
    /// </summary>
    public partial class VendorAssociatedUserModel : BaseAppEntityModel
    {
        #region Properties

        public string Email { get; set; }

        #endregion
    }
}