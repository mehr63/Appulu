using Appulu.Web.Areas.Admin.Models.Common;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Users
{
    /// <summary>
    /// Represents a user address model
    /// </summary>
    public partial class UserAddressModel : BaseAppModel
    {
        #region Ctor

        public UserAddressModel()
        {
            this.Address = new AddressModel();
        }

        #endregion

        #region Properties

        public int UserId { get; set; }

        public AddressModel Address { get; set; }

        #endregion
    }
}