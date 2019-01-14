using FluentValidation;
using Appulu.Web.Areas.Admin.Models.Catalog;
using Appulu.Core.Domain.Catalog;
using Appulu.Data;
using Appulu.Services.Localization;
using Appulu.Services.Seo;
using Appulu.Web.Framework.Validators;

namespace Appulu.Web.Areas.Admin.Validators.Catalog
{
    public partial class CategoryValidator : BaseAppValidator<CategoryModel>
    {
        public CategoryValidator(ILocalizationService localizationService, IDbContext dbContext)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Catalog.Categories.Fields.Name.Required"));
            RuleFor(x => x.PageSizeOptions).Must(ValidatorUtilities.PageSizeOptionsValidator).WithMessage(localizationService.GetResource("Admin.Catalog.Categories.Fields.PageSizeOptions.ShouldHaveUniqueItems"));
            RuleFor(x => x.PageSize).Must((x, context) =>
            {
                if (!x.AllowUsersToSelectPageSize && x.PageSize <= 0)
                    return false;

                return true;
            }).WithMessage(localizationService.GetResource("Admin.Catalog.Categories.Fields.PageSize.Positive"));
            RuleFor(x => x.SeName).Length(0, AppSeoDefaults.SearchEngineNameLength)
                .WithMessage(string.Format(localizationService.GetResource("Admin.SEO.SeName.MaxLengthValidation"), AppSeoDefaults.SearchEngineNameLength));

            SetDatabaseValidationRules<Category>(dbContext);
        }
    }
}