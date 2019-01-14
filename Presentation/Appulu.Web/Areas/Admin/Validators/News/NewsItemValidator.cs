using FluentValidation;
using Appulu.Web.Areas.Admin.Models.News;
using Appulu.Core.Domain.News;
using Appulu.Data;
using Appulu.Services.Localization;
using Appulu.Services.Seo;
using Appulu.Web.Framework.Validators;

namespace Appulu.Web.Areas.Admin.Validators.News
{
    public partial class NewsItemValidator : BaseAppValidator<NewsItemModel>
    {
        public NewsItemValidator(ILocalizationService localizationService, IDbContext dbContext)
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage(localizationService.GetResource("Admin.ContentManagement.News.NewsItems.Fields.Title.Required"));

            RuleFor(x => x.Short).NotEmpty().WithMessage(localizationService.GetResource("Admin.ContentManagement.News.NewsItems.Fields.Short.Required"));

            RuleFor(x => x.Full).NotEmpty().WithMessage(localizationService.GetResource("Admin.ContentManagement.News.NewsItems.Fields.Full.Required"));

            RuleFor(x => x.SeName).Length(0, AppSeoDefaults.SearchEngineNameLength)
                .WithMessage(string.Format(localizationService.GetResource("Admin.SEO.SeName.MaxLengthValidation"), AppSeoDefaults.SearchEngineNameLength));

            SetDatabaseValidationRules<NewsItem>(dbContext);
        }
    }
}