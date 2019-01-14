using FluentValidation;
using Appulu.Services.Localization;
using Appulu.Web.Framework.Validators;
using Appulu.Web.Models.ShoppingCart;

namespace Appulu.Web.Validators.ShoppingCart
{
    public partial class WishlistEmailAFriendValidator : BaseAppValidator<WishlistEmailAFriendModel>
    {
        public WishlistEmailAFriendValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.FriendEmail).NotEmpty().WithMessage(localizationService.GetResource("Wishlist.EmailAFriend.FriendEmail.Required"));
            RuleFor(x => x.FriendEmail).EmailAddress().WithMessage(localizationService.GetResource("Common.WrongEmail"));

            RuleFor(x => x.YourEmailAddress).NotEmpty().WithMessage(localizationService.GetResource("Wishlist.EmailAFriend.YourEmailAddress.Required"));
            RuleFor(x => x.YourEmailAddress).EmailAddress().WithMessage(localizationService.GetResource("Common.WrongEmail"));
        }
    }
}