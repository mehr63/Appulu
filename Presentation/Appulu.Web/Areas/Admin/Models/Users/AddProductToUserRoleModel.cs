using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Users
{
    /// <summary>
    /// Represents a product model to add to the user role 
    /// </summary>
    public partial class AddProductToUserRoleModel : BaseAppEntityModel
    {
        #region Properties

        public int AssociatedToProductId { get; set; }

        #endregion
    }
}