using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;
using Appulu.Web.Validators.ShoppingCart;

namespace Appulu.Web.Models.ShoppingCart
{
    [Validator(typeof(WishlistEmailAFriendValidator))]
    public partial class WishlistEmailAFriendModel : BaseAppModel
    {
        [DataType(DataType.EmailAddress)]
        [AppResourceDisplayName("Wishlist.EmailAFriend.FriendEmail")]
        public string FriendEmail { get; set; }
        
        [AppResourceDisplayName("Wishlist.EmailAFriend.YourEmailAddress")]
        public string YourEmailAddress { get; set; }
        
        [AppResourceDisplayName("Wishlist.EmailAFriend.PersonalMessage")]
        public string PersonalMessage { get; set; }

        public bool SuccessfullySent { get; set; }
        public string Result { get; set; }

        public bool DisplayCaptcha { get; set; }
    }
}