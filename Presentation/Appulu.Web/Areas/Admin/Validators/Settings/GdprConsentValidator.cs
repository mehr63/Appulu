using FluentValidation;
using Appulu.Core.Domain.Gdpr;
using Appulu.Data;
using Appulu.Services.Localization;
using Appulu.Web.Areas.Admin.Models.Settings;
using Appulu.Web.Framework.Validators;

namespace Appulu.Web.Areas.Admin.Validators.Settings
{
    public partial class GdprConsentValidator : BaseAppValidator<GdprConsentModel>
    {
        public GdprConsentValidator(ILocalizationService localizationService, IDbContext dbContext)
        {
            RuleFor(x => x.Message).NotEmpty().WithMessage(localizationService.GetResource("Admin.Configuration.Settings.Gdpr.Consent.Message.Required"));
            RuleFor(x => x.RequiredMessage)
                .NotEmpty()
                .WithMessage(localizationService.GetResource("Admin.Configuration.Settings.Gdpr.Consent.RequiredMessage.Required"))
                .When(x => x.IsRequired);

            SetDatabaseValidationRules<GdprConsent>(dbContext);
        }
    }
}