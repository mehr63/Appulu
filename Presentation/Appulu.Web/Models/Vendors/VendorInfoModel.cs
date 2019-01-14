using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;
using Appulu.Web.Validators.Vendors;

namespace Appulu.Web.Models.Vendors
{
    [Validator(typeof(VendorInfoValidator))]
    public class VendorInfoModel : BaseAppModel
    {
        public VendorInfoModel()
        {
            this.VendorAttributes = new List<VendorAttributeModel>();
        }

        [AppResourceDisplayName("Account.VendorInfo.Name")]
        public string Name { get; set; }

        [DataType(DataType.EmailAddress)]
        [AppResourceDisplayName("Account.VendorInfo.Email")]
        public string Email { get; set; }

        [AppResourceDisplayName("Account.VendorInfo.Description")]
        public string Description { get; set; }

        [AppResourceDisplayName("Account.VendorInfo.Picture")]
        public string PictureUrl { get; set; }

        public IList<VendorAttributeModel> VendorAttributes { get; set; }
    }
}