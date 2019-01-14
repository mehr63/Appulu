using FluentValidation;
using Appulu.Web.Areas.Admin.Models.Messages;
using Appulu.Core.Domain.Messages;
using Appulu.Data;
using Appulu.Services.Localization;
using Appulu.Web.Framework.Validators;

namespace Appulu.Web.Areas.Admin.Validators.Messages
{
    public partial class MessageTemplateValidator : BaseAppValidator<MessageTemplateModel>
    {
        public MessageTemplateValidator(ILocalizationService localizationService, IDbContext dbContext)
        {
            RuleFor(x => x.Subject).NotEmpty().WithMessage(localizationService.GetResource("Admin.ContentManagement.MessageTemplates.Fields.Subject.Required"));
            RuleFor(x => x.Body).NotEmpty().WithMessage(localizationService.GetResource("Admin.ContentManagement.MessageTemplates.Fields.Body.Required"));

            SetDatabaseValidationRules<MessageTemplate>(dbContext);
        }
    }
}