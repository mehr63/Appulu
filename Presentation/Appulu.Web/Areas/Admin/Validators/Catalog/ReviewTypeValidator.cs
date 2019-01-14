using FluentValidation;
using Appulu.Core.Domain.Catalog;
using Appulu.Data;
using Appulu.Services.Localization;
using Appulu.Web.Areas.Admin.Models.Catalog;
using Appulu.Web.Framework.Validators;

namespace Appulu.Web.Areas.Admin.Validators.Catalog
{
    /// <summary>
    /// Represent a review type validator
    /// </summary>
    public partial class ReviewTypeValidator : BaseAppValidator<ReviewTypeModel>
    {
        public ReviewTypeValidator(ILocalizationService localizationService, IDbContext dbContext)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Settings.ReviewType.Fields.Name.Required"));
            RuleFor(x => x.Description).NotEmpty().WithMessage(localizationService.GetResource("Admin.Settings.ReviewType.Fields.Description.Required"));

            SetDatabaseValidationRules<ReviewType>(dbContext);
        }
    }
}
