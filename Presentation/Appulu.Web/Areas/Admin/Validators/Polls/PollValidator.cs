using FluentValidation;
using Appulu.Web.Areas.Admin.Models.Polls;
using Appulu.Services.Localization;
using Appulu.Web.Framework.Validators;

namespace Appulu.Web.Areas.Admin.Validators.Polls
{
    public partial class PollValidator : BaseAppValidator<PollModel>
    {
        public PollValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.ContentManagement.Polls.Fields.Name.Required"));
        }
    }
}