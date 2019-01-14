using FluentValidation;
using Appulu.Web.Areas.Admin.Models.Messages;
using Appulu.Core.Domain.Messages;
using Appulu.Data;
using Appulu.Services.Localization;
using Appulu.Web.Framework.Validators;

namespace Appulu.Web.Areas.Admin.Validators.Messages
{
    public partial class QueuedEmailValidator : BaseAppValidator<QueuedEmailModel>
    {
        public QueuedEmailValidator(ILocalizationService localizationService, IDbContext dbContext)
        {
            RuleFor(x => x.From).NotEmpty().WithMessage(localizationService.GetResource("Admin.System.QueuedEmails.Fields.From.Required"));
            RuleFor(x => x.To).NotEmpty().WithMessage(localizationService.GetResource("Admin.System.QueuedEmails.Fields.To.Required"));

            RuleFor(x => x.SentTries).NotNull().WithMessage(localizationService.GetResource("Admin.System.QueuedEmails.Fields.SentTries.Required"))
                                    .InclusiveBetween(0, 99999).WithMessage(localizationService.GetResource("Admin.System.QueuedEmails.Fields.SentTries.Range"));

            SetDatabaseValidationRules<QueuedEmail>(dbContext);

        }
    }
}