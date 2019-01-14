using FluentValidation;
using Appulu.Core.Domain.Users;
using Appulu.Services.Localization;
using Appulu.Web.Framework.Validators;
using Appulu.Web.Models.User;

namespace Appulu.Web.Validators.User
{
    public partial class PasswordRecoveryConfirmValidator : BaseAppValidator<PasswordRecoveryConfirmModel>
    {
        public PasswordRecoveryConfirmValidator(ILocalizationService localizationService, UserSettings userSettings)
        {
            RuleFor(x => x.NewPassword).NotEmpty().WithMessage(localizationService.GetResource("Account.PasswordRecovery.NewPassword.Required"));
            RuleFor(x => x.NewPassword).Length(userSettings.PasswordMinLength, 999).WithMessage(string.Format(localizationService.GetResource("Account.PasswordRecovery.NewPassword.LengthValidation"), userSettings.PasswordMinLength));
            RuleFor(x => x.ConfirmNewPassword).NotEmpty().WithMessage(localizationService.GetResource("Account.PasswordRecovery.ConfirmNewPassword.Required"));
            RuleFor(x => x.ConfirmNewPassword).Equal(x => x.NewPassword).WithMessage(localizationService.GetResource("Account.PasswordRecovery.NewPassword.EnteredPasswordsDoNotMatch"));
        }
    }
}