using FluentValidation;
using Appulu.Services.Localization;
using Appulu.Web.Framework.Validators;
using Appulu.Web.Models.Boards;

namespace Appulu.Web.Validators.Boards
{
    public partial class EditForumTopicValidator : BaseAppValidator<EditForumTopicModel>
    {
        public EditForumTopicValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Subject).NotEmpty().WithMessage(localizationService.GetResource("Forum.TopicSubjectCannotBeEmpty"));
            RuleFor(x => x.Text).NotEmpty().WithMessage(localizationService.GetResource("Forum.TextCannotBeEmpty"));
        }
    }
}