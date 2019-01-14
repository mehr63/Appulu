using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Appulu.Plugin.Payments.Worldpay.Domain.Enums;
using Appulu.Plugin.Payments.Worldpay.Domain.Enums.Converters;

namespace Appulu.Plugin.Payments.Worldpay.Domain.Models
{
    /// <summary>
    /// Represents a user's payment method.
    /// </summary>
    public class PaymentMethod
    {
        /// <summary>
        /// Gets or sets a user identifier.
        /// </summary>
        [JsonProperty("userId")]
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets a payment method identifier or token.
        /// </summary>
        [JsonProperty("paymentId")]
        public string PaymentId { get; set; }

        /// <summary>
        /// Gets or sets a credit card account associated with the payment method.
        /// </summary>
        [JsonProperty("card")]
        public Card Card { get; set; }

        /// <summary>
        /// Gets or sets a notes associated with the payment method.
        /// </summary>
        [JsonProperty("notes")]
        public string Notes { get; set; }

        /// <summary>
        /// Gets or sets a payment method type.
        /// </summary>
        [JsonConverter(typeof(NullableStringEnumConverter))]
        [JsonProperty("method")]
        public PaymentMethodType? PaymentMethodType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the account is set as the primary account for the associated user.
        /// </summary>
        [JsonProperty("primary")]
        public bool IsPrimary { get; set; }

        /// <summary>
        /// Gets or sets a most recent date the payment method was accessed.
        /// </summary>
        [JsonProperty("lastAccessDate")]
        public DateTime? LastAccessDate { get; set; }

        /// <summary>
        /// Gets or sets custom user-defined fields for reporting purposes.
        /// </summary>
        [JsonProperty("userDefinedFields")]
        public IList<KeyValuePair<string, string>> UserDefinedFields { get; set; }
    }
}