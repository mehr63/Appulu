using System.Collections.Generic;
using Appulu.Core.Domain.Catalog;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Models.Vendors
{
    public partial class VendorAttributeModel : BaseAppEntityModel
    {
        public VendorAttributeModel()
        {
            Values = new List<VendorAttributeValueModel>();
        }

        public string Name { get; set; }

        public bool IsRequired { get; set; }

        /// <summary>
        /// Default value for textboxes
        /// </summary>
        public string DefaultValue { get; set; }

        public AttributeControlType AttributeControlType { get; set; }

        public IList<VendorAttributeValueModel> Values { get; set; }

    }

    public partial class VendorAttributeValueModel : BaseAppEntityModel
    {
        public string Name { get; set; }

        public bool IsPreSelected { get; set; }
    }
}