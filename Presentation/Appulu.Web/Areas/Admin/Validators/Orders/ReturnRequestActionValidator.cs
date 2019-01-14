using FluentValidation;
using Appulu.Web.Areas.Admin.Models.Orders;
using Appulu.Services.Localization;
using Appulu.Web.Framework.Validators;

namespace Appulu.Web.Areas.Admin.Validators.Orders
{
    public partial class ReturnRequestActionValidator : BaseAppValidator<ReturnRequestActionModel>
    {
        public ReturnRequestActionValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Configuration.Settings.Order.ReturnRequestActions.Name.Required"));
        }
    }
}