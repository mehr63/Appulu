using FluentValidation;
using Appulu.Web.Areas.Admin.Models.Users;
using Appulu.Core.Domain.Users;
using Appulu.Data;
using Appulu.Services.Localization;
using Appulu.Web.Framework.Validators;

namespace Appulu.Web.Areas.Admin.Validators.Users
{
    public partial class UserAttributeValueValidator : BaseAppValidator<UserAttributeValueModel>
    {
        public UserAttributeValueValidator(ILocalizationService localizationService, IDbContext dbContext)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Users.UserAttributes.Values.Fields.Name.Required"));

            SetDatabaseValidationRules<UserAttributeValue>(dbContext);
        }
    }
}