using FluentValidation.Attributes;
using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Validators.User;
using System.ComponentModel.DataAnnotations;

namespace Appulu.Web.Models.User
{
    [Validator(typeof(GiftCardValidator))]
    public partial class CheckGiftCardBalanceModel : BaseAppModel
    {
        public string Result { get; set; }

        public string Message { get; set; }
        
        [AppResourceDisplayName("ShoppingCart.GiftCardCouponCode.Tooltip")]
        public string GiftCardCode { get; set; }
    }
}
