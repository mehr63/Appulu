using FluentValidation;
using Appulu.Web.Areas.Admin.Models.Orders;
using Appulu.Services.Localization;
using Appulu.Web.Framework.Validators;

namespace Appulu.Web.Areas.Admin.Validators.Orders
{
    public partial class ReturnRequestValidator : BaseAppValidator<ReturnRequestModel>
    {
        public ReturnRequestValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.ReasonForReturn).NotEmpty().WithMessage(localizationService.GetResource("Admin.ReturnRequests.Fields.ReasonForReturn.Required"));
            RuleFor(x => x.RequestedAction).NotEmpty().WithMessage(localizationService.GetResource("Admin.ReturnRequests.Fields.RequestedAction.Required"));
        }
    }
}