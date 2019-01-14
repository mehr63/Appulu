using Newtonsoft.Json;

namespace Appulu.Plugin.Payments.Worldpay.Domain.Requests
{
    /// <summary>
    /// Represents request parameters to delete a credit card from Vault.
    /// </summary>
    public class DeleteCardRequest : WorldpayPostRequest
    {
        #region Properties

        /// <summary>
        /// Gets or sets a identifier for the user whose payment method is being deleted.
        /// </summary>
        [JsonIgnore]
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets a unique identifier (per user), which can be used with userId to reference the payment method.
        /// </summary>
        [JsonIgnore]
        public string PaymentId { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Get a request endpoint URL
        /// </summary>
        /// <returns>URL</returns>
        public override string GetRequestUrl() => $"api/Users/{UserId}/PaymentMethod/{PaymentId}";

        /// <summary>
        /// Get a request method
        /// </summary>
        /// <returns>Request method</returns>
        public override string GetRequestMethod() => "DELETE";

        #endregion
    }
}