using FluentValidation;
using Appulu.Web.Areas.Admin.Models.Catalog;
using Appulu.Core.Domain.Catalog;
using Appulu.Data;
using Appulu.Services.Localization;
using Appulu.Services.Seo;
using Appulu.Web.Framework.Validators;

namespace Appulu.Web.Areas.Admin.Validators.Catalog
{
    public partial class ProductValidator : BaseAppValidator<ProductModel>
    {
        public ProductValidator(ILocalizationService localizationService, IDbContext dbContext)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Catalog.Products.Fields.Name.Required"));
            RuleFor(x => x.SeName).Length(0, AppSeoDefaults.SearchEngineNameLength)
                .WithMessage(string.Format(localizationService.GetResource("Admin.SEO.SeName.MaxLengthValidation"), AppSeoDefaults.SearchEngineNameLength));

            SetDatabaseValidationRules<Product>(dbContext);
        }
    }
}