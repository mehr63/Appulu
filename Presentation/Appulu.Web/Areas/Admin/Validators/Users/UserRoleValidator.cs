using FluentValidation;
using Appulu.Web.Areas.Admin.Models.Users;
using Appulu.Core.Domain.Users;
using Appulu.Data;
using Appulu.Services.Localization;
using Appulu.Web.Framework.Validators;

namespace Appulu.Web.Areas.Admin.Validators.Users
{
    public partial class UserRoleValidator : BaseAppValidator<UserRoleModel>
    {
        public UserRoleValidator(ILocalizationService localizationService, IDbContext dbContext)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Users.UserRoles.Fields.Name.Required"));

            SetDatabaseValidationRules<UserRole>(dbContext);
        }
    }
}