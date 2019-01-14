using FluentValidation;
using Appulu.Web.Areas.Admin.Models.Directory;
using Appulu.Core.Domain.Directory;
using Appulu.Data;
using Appulu.Services.Localization;
using Appulu.Web.Framework.Validators;

namespace Appulu.Web.Areas.Admin.Validators.Directory
{
    public partial class MeasureWeightValidator : BaseAppValidator<MeasureWeightModel>
    {
        public MeasureWeightValidator(ILocalizationService localizationService, IDbContext dbContext)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Configuration.Shipping.Measures.Weights.Fields.Name.Required"));
            RuleFor(x => x.SystemKeyword).NotEmpty().WithMessage(localizationService.GetResource("Admin.Configuration.Shipping.Measures.Weights.Fields.SystemKeyword.Required"));

            SetDatabaseValidationRules<MeasureWeight>(dbContext);
        }
    }
}