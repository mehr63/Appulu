using Appulu.Web.Framework.Models;

namespace Appulu.Plugin.Payments.Worldpay.Models.User
{
    public class WorldpayCardModel : BaseAppModel
    {
        public string Id { get; set; }

        public string CardId { get; set; }
        
        public string MaskedNumber { get; set; }

        public string ExpirationDate { get; set; }
        
        public string CardType { get; set; }
    }
}