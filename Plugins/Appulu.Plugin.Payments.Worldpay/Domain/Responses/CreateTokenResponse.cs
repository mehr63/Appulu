using Newtonsoft.Json;

namespace Appulu.Plugin.Payments.Worldpay.Domain.Responses
{
    /// <summary>
    /// Represents return values of create token requests
    /// </summary>
    public class CreateTokenResponse : WorldpayResponse
    {
        /// <summary>
        /// Gets or sets a user identifier. 
        /// </summary>
        [JsonProperty("userId")]
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets a token that can be used in replacement of paymentMethodId in the paymentVaultToken object.
        /// </summary>
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}