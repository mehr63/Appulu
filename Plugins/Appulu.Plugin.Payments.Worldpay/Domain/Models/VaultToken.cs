using Newtonsoft.Json;
using Appulu.Plugin.Payments.Worldpay.Domain.Enums;
using Appulu.Plugin.Payments.Worldpay.Domain.Enums.Converters;

namespace Appulu.Plugin.Payments.Worldpay.Domain.Models
{
    /// <summary>
    /// Represents a data from a Vault payment account.
    /// </summary>
    public class VaultToken
    {
        /// <summary>
        /// Gets or sets a user identifier.
        /// </summary>
        [JsonProperty("userId")]
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets an identifier of the payment method to be used for billing or token.
        /// </summary>
        [JsonProperty("paymentMethodId")]
        public string PaymentMethodId { get; set; }

        /// <summary>
        /// Gets or sets a public key used to identify the merchant.
        /// </summary>
        [JsonProperty("publicKey")]
        public string PublicKey { get; set; }

        /// <summary>
        /// Gets or sets a payment type that is stored or about to be stored in the Vault.
        /// </summary>
        [JsonConverter(typeof(NullableStringEnumConverter))]
        [JsonProperty("paymentType")]
        public PaymentMethodType? PaymentMethodType { get; set; }
    }
}