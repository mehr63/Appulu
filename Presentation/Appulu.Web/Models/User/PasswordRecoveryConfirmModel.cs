using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;
using Appulu.Web.Validators.User;

namespace Appulu.Web.Models.User
{
    [Validator(typeof(PasswordRecoveryConfirmValidator))]
    public partial class PasswordRecoveryConfirmModel : BaseAppModel
    {
        [DataType(DataType.Password)]
        [NoTrim]
        [AppResourceDisplayName("Account.PasswordRecovery.NewPassword")]
        public string NewPassword { get; set; }
        
        [NoTrim]
        [DataType(DataType.Password)]
        [AppResourceDisplayName("Account.PasswordRecovery.ConfirmNewPassword")]
        public string ConfirmNewPassword { get; set; }

        public bool DisablePasswordChanging { get; set; }
        public string Result { get; set; }
    }
}