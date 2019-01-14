using System.Net;
using Newtonsoft.Json;

namespace Appulu.Plugin.Payments.Worldpay.Domain.Requests
{
    public class GetUserRequest : WorldpayRequest
    {
        #region Properties

        /// <summary>
        /// Gets or sets a user identifier.
        /// </summary>
        [JsonIgnore]
        public string UserId { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Get a request endpoint URL
        /// </summary>
        /// <returns>URL</returns>
        public override string GetRequestUrl() => $"api/Users/{UserId}";

        /// <summary>
        /// Get a request method
        /// </summary>
        /// <returns>Request method</returns>
        public override string GetRequestMethod() => WebRequestMethods.Http.Get;

        #endregion
    }
}