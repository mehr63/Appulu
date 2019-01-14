using System.Collections.Generic;
using Appulu.Web.Framework.Models;
using Appulu.Web.Models.Common;

namespace Appulu.Web.Models.User
{
    public partial class UserAddressListModel : BaseAppModel
    {
        public UserAddressListModel()
        {
            Addresses = new List<AddressModel>();
        }

        public IList<AddressModel> Addresses { get; set; }
    }
}