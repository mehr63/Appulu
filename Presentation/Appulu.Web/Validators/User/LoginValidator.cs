using FluentValidation;
using Appulu.Core.Domain.Users;
using Appulu.Services.Localization;
using Appulu.Web.Framework.Validators;
using Appulu.Web.Models.User;

namespace Appulu.Web.Validators.User
{
    public partial class LoginValidator : BaseAppValidator<LoginModel>
    {
        public LoginValidator(ILocalizationService localizationService, UserSettings userSettings)
        {
            if (!userSettings.UsernamesEnabled)
            {
                //login by email
                RuleFor(x => x.Email).NotEmpty().WithMessage(localizationService.GetResource("Account.Login.Fields.Email.Required"));
                RuleFor(x => x.Email).EmailAddress().WithMessage(localizationService.GetResource("Common.WrongEmail"));
            }
        }
    }
}