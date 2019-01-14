using FluentValidation;
using Appulu.Web.Areas.Admin.Models.Directory;
using Appulu.Services.Localization;
using Appulu.Web.Framework.Validators;

namespace Appulu.Web.Areas.Admin.Validators.Directory
{
    public partial class CountryValidator : BaseAppValidator<CountryModel>
    {
        public CountryValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(localizationService.GetResource("Admin.Configuration.Countries.Fields.Name.Required"));
            RuleFor(p => p.Name).Length(1, 100);

            RuleFor(x => x.TwoLetterIsoCode)
                .NotEmpty()
                .WithMessage(localizationService.GetResource("Admin.Configuration.Countries.Fields.TwoLetterIsoCode.Required"));
            RuleFor(x => x.TwoLetterIsoCode)
                .Length(2)
                .WithMessage(localizationService.GetResource("Admin.Configuration.Countries.Fields.TwoLetterIsoCode.Length"));

            RuleFor(x => x.ThreeLetterIsoCode)
                .NotEmpty()
                .WithMessage(localizationService.GetResource("Admin.Configuration.Countries.Fields.ThreeLetterIsoCode.Required"));
            RuleFor(x => x.ThreeLetterIsoCode)
                .Length(3)
                .WithMessage(localizationService.GetResource("Admin.Configuration.Countries.Fields.ThreeLetterIsoCode.Length"));
        }
    }
}