using FluentValidation;
using Appulu.Web.Areas.Admin.Models.Shipping;
using Appulu.Core.Domain.Shipping;
using Appulu.Data;
using Appulu.Services.Localization;
using Appulu.Web.Framework.Validators;

namespace Appulu.Web.Areas.Admin.Validators.Shipping
{
    public partial class ProductAvailabilityRangeValidator : BaseAppValidator<ProductAvailabilityRangeModel>
    {
        public ProductAvailabilityRangeValidator(ILocalizationService localizationService, IDbContext dbContext)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Configuration.Shipping.ProductAvailabilityRanges.Fields.Name.Required"));

            SetDatabaseValidationRules<ProductAvailabilityRange>(dbContext);
        }
    }
}