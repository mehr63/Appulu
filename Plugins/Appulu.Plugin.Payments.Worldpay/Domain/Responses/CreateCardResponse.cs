using Newtonsoft.Json;
using Appulu.Plugin.Payments.Worldpay.Domain.Models;

namespace Appulu.Plugin.Payments.Worldpay.Domain.Responses
{
    /// <summary>
    /// Represents return values of create card requests
    /// </summary>
    public class CreateCardResponse : WorldpayResponse
    {
        /// <summary>
        /// Gets or sets a newly created payment method.
        /// </summary>
        [JsonProperty("vaultPaymentMethod")]
        public PaymentMethod PaymentMethod { get; set; }
    }
}