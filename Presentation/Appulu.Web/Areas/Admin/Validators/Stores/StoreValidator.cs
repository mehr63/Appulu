using FluentValidation;
using Appulu.Web.Areas.Admin.Models.Stores;
using Appulu.Core.Domain.Stores;
using Appulu.Data;
using Appulu.Services.Localization;
using Appulu.Web.Framework.Validators;

namespace Appulu.Web.Areas.Admin.Validators.Stores
{
    public partial class StoreValidator : BaseAppValidator<StoreModel>
    {
        public StoreValidator(ILocalizationService localizationService, IDbContext dbContext)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Configuration.Stores.Fields.Name.Required"));
            RuleFor(x => x.Url).NotEmpty().WithMessage(localizationService.GetResource("Admin.Configuration.Stores.Fields.Url.Required"));

            SetDatabaseValidationRules<Store>(dbContext);
        }
    }
}