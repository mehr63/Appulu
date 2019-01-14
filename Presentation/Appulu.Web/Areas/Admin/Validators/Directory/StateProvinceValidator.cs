using FluentValidation;
using Appulu.Web.Areas.Admin.Models.Directory;
using Appulu.Core.Domain.Directory;
using Appulu.Data;
using Appulu.Services.Localization;
using Appulu.Web.Framework.Validators;

namespace Appulu.Web.Areas.Admin.Validators.Directory
{
    public partial class StateProvinceValidator : BaseAppValidator<StateProvinceModel>
    {
        public StateProvinceValidator(ILocalizationService localizationService, IDbContext dbContext)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Configuration.Countries.States.Fields.Name.Required"));

            SetDatabaseValidationRules<StateProvince>(dbContext);
        }
    }
}