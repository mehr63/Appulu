using FluentValidation;
using Appulu.Services.Localization;
using Appulu.Web.Framework.Validators;
using Appulu.Web.Models.User;

namespace Appulu.Web.Validators.User
{
    public partial class GiftCardValidator : BaseAppValidator<CheckGiftCardBalanceModel>
    {
        public GiftCardValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.GiftCardCode).NotEmpty().WithMessage(localizationService.GetResource("CheckGiftCardBalance.GiftCardCouponCode.Empty"));            
        }
    }
}
