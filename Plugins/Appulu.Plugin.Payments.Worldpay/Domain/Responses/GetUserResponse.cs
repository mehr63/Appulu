using Newtonsoft.Json;
using Appulu.Plugin.Payments.Worldpay.Domain.Models;

namespace Appulu.Plugin.Payments.Worldpay.Domain.Responses
{
    /// <summary>
    /// Represents return values of get user requests
    /// </summary>
    public class GetUserResponse : WorldpayResponse
    {
        /// <summary>
        /// Gets or sets a user identifier.
        /// </summary>
        [JsonProperty("userId")]
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the retrieved user record.
        /// </summary>
        [JsonProperty("vaultUser")]
        public VaultUser User { get; set; }
    }
}