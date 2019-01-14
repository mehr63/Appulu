using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using Appulu.Plugin.Payments.Worldpay.Domain.Enums;
using Appulu.Plugin.Payments.Worldpay.Domain.Models;

namespace Appulu.Plugin.Payments.Worldpay.Domain.Requests
{
    /// <summary>
    /// Represents request parameters to update a user in Vault.
    /// </summary>
    public class UpdateUserRequest : WorldpayPostRequest
    {
        #region Ctor

        public UpdateUserRequest(CreateUserRequest request)
        {
            BillingAddress = request.BillingAddress;
            Company = request.Company;
            UserDuplicateCheckType = request.UserDuplicateCheckType;
            UserId = request.UserId;
            Email = request.Email;
            EmailReceiptEnabled = request.EmailReceiptEnabled;
            FirstName = request.FirstName;
            LastName = request.LastName;
            Notes = request.Notes;
            Phone = request.Phone;
            UserDefinedFields = request.UserDefinedFields;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a user identifier.
        /// </summary>
        [JsonProperty("userId")]
        public string UserId { get; set; }

        /// <summary>
        /// Gets o sets a first name of the user.
        /// </summary>
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets o sets a last name of the user.
        /// </summary>
        [JsonProperty("lastName")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets a billing address of the user.
        /// </summary>
        [JsonProperty("address")]
        public Address BillingAddress { get; set; }

        /// <summary>
        /// Gets o sets an email address of the user.
        /// </summary>
        [JsonProperty("emailAddress")]
        public string Email { get; set; }

        /// <summary>
        /// Gets o sets a value indicating whether an email receipt should be sent to the user whenever a transaction is completed.
        /// </summary>
        [JsonProperty("sendEmailReceipts")]
        public bool EmailReceiptEnabled { get; set; }

        /// <summary>
        /// Gets o sets a phone number of the user.
        /// </summary>
        [JsonProperty("phoneNumber")]
        public string Phone { get; set; }

        /// <summary>
        /// Gets o sets a company of the user.
        /// </summary>
        [JsonProperty("company")]
        public string Company { get; set; }

        /// <summary>
        /// Gets o sets a notes associated with the user.
        /// </summary>
        [JsonProperty("notes")]
        public string Notes { get; set; }

        /// <summary>
        /// Gets or sets custom user-defined fields for reporting purposes.
        /// </summary>
        [JsonProperty("userDefinedFields")]
        public IList<KeyValuePair<string, string>> UserDefinedFields { get; set; }

        /// <summary>
        /// Gets o sets a value indicating how the method should behave if the User ID already exists.
        /// </summary>
        [JsonProperty("userDuplicateCheckIndicator")]
        public UserDuplicateCheckType? UserDuplicateCheckType { get; set; }

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
        public override string GetRequestMethod() => WebRequestMethods.Http.Put;

        #endregion
    }
}