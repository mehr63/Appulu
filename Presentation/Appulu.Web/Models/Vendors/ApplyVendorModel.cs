using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;
using Appulu.Web.Validators.Vendors;

namespace Appulu.Web.Models.Vendors
{
    [Validator(typeof(ApplyVendorValidator))]
    public partial class ApplyVendorModel : BaseAppModel
    {
        public ApplyVendorModel()
        {
            this.VendorAttributes = new List<VendorAttributeModel>();
        }

        [AppResourceDisplayName("Vendors.ApplyAccount.Name")]
        public string Name { get; set; }

        [DataType(DataType.EmailAddress)]
        [AppResourceDisplayName("Vendors.ApplyAccount.Email")]
        public string Email { get; set; }

        [AppResourceDisplayName("Vendors.ApplyAccount.Description")]
        public string Description { get; set; }

        public IList<VendorAttributeModel> VendorAttributes { get; set; }

        public bool DisplayCaptcha { get; set; }

        public bool TermsOfServiceEnabled { get; set; }
        public bool TermsOfServicePopup { get; set; }

        public bool DisableFormInput { get; set; }
        public string Result { get; set; }
    }
}