using FluentValidation;
using Appulu.Web.Areas.Admin.Models.Forums;
using Appulu.Core.Domain.Forums;
using Appulu.Data;
using Appulu.Services.Localization;
using Appulu.Web.Framework.Validators;

namespace Appulu.Web.Areas.Admin.Validators.Forums
{
    public partial class ForumGroupValidator : BaseAppValidator<ForumGroupModel>
    {
        public ForumGroupValidator(ILocalizationService localizationService, IDbContext dbContext)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.ContentManagement.Forums.ForumGroup.Fields.Name.Required"));

            SetDatabaseValidationRules<ForumGroup>(dbContext);
        }
    }
}