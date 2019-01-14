using System.Collections.Generic;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Models.Catalog
{
    public partial class VendorNavigationModel : BaseAppModel
    {
        public VendorNavigationModel()
        {
            this.Vendors = new List<VendorBriefInfoModel>();
        }

        public IList<VendorBriefInfoModel> Vendors { get; set; }

        public int TotalVendors { get; set; }
    }

    public partial class VendorBriefInfoModel : BaseAppEntityModel
    {
        public string Name { get; set; }

        public string SeName { get; set; }
    }
}