using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;
using Appulu.Web.Validators.Common;

namespace Appulu.Web.Models.Common
{
    [Validator(typeof(ContactVendorValidator))]
    public partial class ContactVendorModel : BaseAppModel
    {
        public int VendorId { get; set; }
        public string VendorName { get; set; }

        [DataType(DataType.EmailAddress)]
        [AppResourceDisplayName("ContactVendor.Email")]
        public string Email { get; set; }

        [AppResourceDisplayName("ContactVendor.Subject")]
        public string Subject { get; set; }
        public bool SubjectEnabled { get; set; }

        [AppResourceDisplayName("ContactVendor.Enquiry")]
        public string Enquiry { get; set; }

        [AppResourceDisplayName("ContactVendor.FullName")]
        public string FullName { get; set; }

        public bool SuccessfullySent { get; set; }
        public string Result { get; set; }

        public bool DisplayCaptcha { get; set; }
    }
}