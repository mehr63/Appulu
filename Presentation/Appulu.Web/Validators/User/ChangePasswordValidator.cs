using FluentValidation;
using Appulu.Core.Domain.Users;
using Appulu.Services.Localization;
using Appulu.Web.Framework.Validators;
using Appulu.Web.Models.User;

namespace Appulu.Web.Validators.User
{
    public partial class ChangePasswordValidator : BaseAppValidator<ChangePasswordModel>
    {
        public ChangePasswordValidator(ILocalizationService localizationService, UserSettings userSettings)
        {
            RuleFor(x => x.OldPassword).NotEmpty().WithMessage(localizationService.GetResource("Account.ChangePassword.Fields.OldPassword.Required"));
            RuleFor(x => x.NewPassword).NotEmpty().WithMessage(localizationService.GetResource("Account.ChangePassword.Fields.NewPassword.Required"));
            RuleFor(x => x.NewPassword).Length(userSettings.PasswordMinLength, 999).WithMessage(string.Format(localizationService.GetResource("Account.ChangePassword.Fields.NewPassword.LengthValidation"), userSettings.PasswordMinLength));
            RuleFor(x => x.ConfirmNewPassword).NotEmpty().WithMessage(localizationService.GetResource("Account.ChangePassword.Fields.ConfirmNewPassword.Required"));
            RuleFor(x => x.ConfirmNewPassword).Equal(x => x.NewPassword).WithMessage(localizationService.GetResource("Account.ChangePassword.Fields.NewPassword.EnteredPasswordsDoNotMatch"));
        }
    }
}