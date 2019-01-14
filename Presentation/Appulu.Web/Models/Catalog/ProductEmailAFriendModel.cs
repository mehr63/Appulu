using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;
using Appulu.Web.Validators.Catalog;

namespace Appulu.Web.Models.Catalog
{
    [Validator(typeof(ProductEmailAFriendValidator))]
    public partial class ProductEmailAFriendModel : BaseAppModel
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductSeName { get; set; }

        [DataType(DataType.EmailAddress)]
        [AppResourceDisplayName("Products.EmailAFriend.FriendEmail")]
        public string FriendEmail { get; set; }

        [AppResourceDisplayName("Products.EmailAFriend.YourEmailAddress")]
        public string YourEmailAddress { get; set; }

        [AppResourceDisplayName("Products.EmailAFriend.PersonalMessage")]
        public string PersonalMessage { get; set; }

        public bool SuccessfullySent { get; set; }
        public string Result { get; set; }

        public bool DisplayCaptcha { get; set; }
    }
}