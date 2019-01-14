using FluentValidation;
using Appulu.Web.Areas.Admin.Models.Topics;
using Appulu.Core.Domain.Topics;
using Appulu.Data;
using Appulu.Services.Localization;
using Appulu.Services.Seo;
using Appulu.Web.Framework.Validators;

namespace Appulu.Web.Areas.Admin.Validators.Topics
{
    public partial class TopicValidator : BaseAppValidator<TopicModel>
    {
        public TopicValidator(ILocalizationService localizationService, IDbContext dbContext)
        {
            RuleFor(x => x.SeName).Length(0, AppSeoDefaults.ForumTopicLength)
                .WithMessage(string.Format(localizationService.GetResource("Admin.SEO.SeName.MaxLengthValidation"), AppSeoDefaults.ForumTopicLength));

            SetDatabaseValidationRules<Topic>(dbContext);
        }
    }
}
