using FluentValidation;
using Appulu.Web.Areas.Admin.Models.Discounts;
using Appulu.Core.Domain.Discounts;
using Appulu.Data;
using Appulu.Services.Localization;
using Appulu.Web.Framework.Validators;

namespace Appulu.Web.Areas.Admin.Validators.Discounts
{
    public partial class DiscountValidator : BaseAppValidator<DiscountModel>
    {
        public DiscountValidator(ILocalizationService localizationService, IDbContext dbContext)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Promotions.Discounts.Fields.Name.Required"));

            SetDatabaseValidationRules<Discount>(dbContext);
        }
    }
}