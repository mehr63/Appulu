using System.Collections.Generic;
using Appulu.Core.Domain.Catalog;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Models.User
{
    public partial class UserAttributeModel : BaseAppEntityModel
    {
        public UserAttributeModel()
        {
            Values = new List<UserAttributeValueModel>();
        }

        public string Name { get; set; }

        public bool IsRequired { get; set; }

        /// <summary>
        /// Default value for textboxes
        /// </summary>
        public string DefaultValue { get; set; }

        public AttributeControlType AttributeControlType { get; set; }

        public IList<UserAttributeValueModel> Values { get; set; }

    }

    public partial class UserAttributeValueModel : BaseAppEntityModel
    {
        public string Name { get; set; }

        public bool IsPreSelected { get; set; }
    }
}