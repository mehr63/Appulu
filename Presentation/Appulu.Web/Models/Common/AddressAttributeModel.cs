using System.Collections.Generic;
using Appulu.Core.Domain.Catalog;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Models.Common
{
    public partial class AddressAttributeModel : BaseAppEntityModel
    {
        public AddressAttributeModel()
        {
            Values = new List<AddressAttributeValueModel>();
        }

        public string Name { get; set; }

        public bool IsRequired { get; set; }

        /// <summary>
        /// Default value for textboxes
        /// </summary>
        public string DefaultValue { get; set; }

        public AttributeControlType AttributeControlType { get; set; }

        public IList<AddressAttributeValueModel> Values { get; set; }
    }

    public partial class AddressAttributeValueModel : BaseAppEntityModel
    {
        public string Name { get; set; }

        public bool IsPreSelected { get; set; }
    }
}