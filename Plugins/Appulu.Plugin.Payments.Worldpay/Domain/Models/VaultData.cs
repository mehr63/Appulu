using Newtonsoft.Json;

namespace Appulu.Plugin.Payments.Worldpay.Domain.Models
{
    /// <summary>
    /// Represents a Vault data
    /// </summary>
    public class VaultData
    {
        /// <summary>
        /// Gets or sets a vault token.
        /// </summary>
        [JsonProperty("token")]
        public VaultToken Token { get; set; }

        /// <summary>
        /// Gets or sets a company of the user.
        /// </summary>
        [JsonProperty("company")]
        public string Company { get; set; }

        /// <summary>
        /// Gets or sets a first name of the user.
        /// </summary>
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets a last name of the user.
        /// </summary>
        [JsonProperty("lastName")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets an email address of the user.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets a phone number of the user.
        /// </summary>
        [JsonProperty("phone")]
        public string Phone { get; set; }
    }
}