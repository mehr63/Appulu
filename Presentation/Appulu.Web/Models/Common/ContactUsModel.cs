using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;
using Appulu.Web.Validators.Common;

namespace Appulu.Web.Models.Common
{
    [Validator(typeof(ContactUsValidator))]
    public partial class ContactUsModel : BaseAppModel
    {
        [DataType(DataType.EmailAddress)]
        [AppResourceDisplayName("ContactUs.Email")]
        public string Email { get; set; }
        
        [AppResourceDisplayName("ContactUs.Subject")]
        public string Subject { get; set; }
        public bool SubjectEnabled { get; set; }

        [AppResourceDisplayName("ContactUs.Enquiry")]
        public string Enquiry { get; set; }

        [AppResourceDisplayName("ContactUs.FullName")]
        public string FullName { get; set; }

        public bool SuccessfullySent { get; set; }
        public string Result { get; set; }

        public bool DisplayCaptcha { get; set; }
    }
}