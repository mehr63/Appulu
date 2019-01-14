using FluentValidation;
using Appulu.Web.Areas.Admin.Models.Blogs;
using Appulu.Core.Domain.Blogs;
using Appulu.Data;
using Appulu.Services.Localization;
using Appulu.Services.Seo;
using Appulu.Web.Framework.Validators;

namespace Appulu.Web.Areas.Admin.Validators.Blogs
{
    public partial class BlogPostValidator : BaseAppValidator<BlogPostModel>
    {
        public BlogPostValidator(ILocalizationService localizationService, IDbContext dbContext)
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage(localizationService.GetResource("Admin.ContentManagement.Blog.BlogPosts.Fields.Title.Required"));

            RuleFor(x => x.Body)
                .NotEmpty()
                .WithMessage(localizationService.GetResource("Admin.ContentManagement.Blog.BlogPosts.Fields.Body.Required"));

            //blog tags should not contain dots
            //current implementation does not support it because it can be handled as file extension
            RuleFor(x => x.Tags)
                .Must(x => x == null || !x.Contains("."))
                .WithMessage(localizationService.GetResource("Admin.ContentManagement.Blog.BlogPosts.Fields.Tags.NoDots"));

            RuleFor(x => x.SeName).Length(0, AppSeoDefaults.SearchEngineNameLength)
                .WithMessage(string.Format(localizationService.GetResource("Admin.SEO.SeName.MaxLengthValidation"), AppSeoDefaults.SearchEngineNameLength));

            SetDatabaseValidationRules<BlogPost>(dbContext);
        }
    }
}