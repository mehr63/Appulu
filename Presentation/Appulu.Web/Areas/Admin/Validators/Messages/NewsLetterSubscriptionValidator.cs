using FluentValidation;
using Appulu.Web.Areas.Admin.Models.Messages;
using Appulu.Core.Domain.Messages;
using Appulu.Data;
using Appulu.Services.Localization;
using Appulu.Web.Framework.Validators;

namespace Appulu.Web.Areas.Admin.Validators.Messages
{
    public partial class NewsLetterSubscriptionValidator : BaseAppValidator<NewsletterSubscriptionModel>
    {
        public NewsLetterSubscriptionValidator(ILocalizationService localizationService, IDbContext dbContext)
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage(localizationService.GetResource("Admin.Promotions.NewsLetterSubscriptions.Fields.Email.Required"));
            RuleFor(x => x.Email).EmailAddress().WithMessage(localizationService.GetResource("Admin.Common.WrongEmail"));

            SetDatabaseValidationRules<NewsLetterSubscription>(dbContext);
        }
    }
}