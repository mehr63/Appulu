using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Plugin.Payments.Worldpay.Models.User
{
    public class WorldpayUserModel : BaseAppEntityModel
    {
        [AppResourceDisplayName("Plugins.Payments.Worldpay.Fields.WorldpayUserId")]
        public string WorldpayUserId { get; set; }

        public bool UserExists { get; set; }
    }
}