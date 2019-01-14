using Appulu.Web.Framework.Models;
using Appulu.Web.Models.Common;

namespace Appulu.Web.Models.User
{
    public partial class UserAddressEditModel : BaseAppModel
    {
        public UserAddressEditModel()
        {
            this.Address = new AddressModel();
        }
        
        public AddressModel Address { get; set; }
    }
}