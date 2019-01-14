using Appulu.Web.Areas.Admin.Models.Common;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Orders
{
    public partial class OrderAddressModel : BaseAppModel
    {
        #region Ctor

        public OrderAddressModel()
        {
            Address = new AddressModel();
        }

        #endregion

        #region Properties

        public int OrderId { get; set; }

        public AddressModel Address { get; set; }

        #endregion
    }
}