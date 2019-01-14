using System.Collections.Generic;
using Newtonsoft.Json;

namespace Appulu.Plugin.Payments.Worldpay.Domain.Models
{
    /// <summary>
    /// Represents a contents of the retrieved user record.
    /// </summary>
    public class VaultUser
    {
        /// <summary>
        /// Gets or sets a user identifier.
        /// </summary>
        [JsonProperty("userId")]
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets a billing address of the user.
        /// </summary>
        [JsonProperty("address")]
        public Address BillingAddress { get; set; }

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
        /// Gets o sets an email address of the user.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets o sets a value indicating whether an email receipt should be sent to the user whenever a transaction is completed.
        /// </summary>
        [JsonProperty("emailReceipt")]
        public bool EmailReceiptEnabled { get; set; }

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
        /// Gets o sets all payment methods on file.
        /// </summary>
        [JsonProperty("paymentMethods")]
        public IList<PaymentMethod> PaymentMethods { get; set; }

        /// <summary>
        /// Gets o sets a primary payment method for the user.
        /// </summary>
        [JsonProperty("primaryPaymentMethodId")]
        public string PrimaryPaymentMethodId { get; set; }

        /// <summary>
        /// Gets o sets all variable payment plans associated with the user.
        /// </summary>
        [JsonProperty("variablePaymentPlans")]
        public IList<VariablePaymentPlan> VariablePaymentPlans { get; set; }

        /// <summary>
        /// Gets o sets all recurring payment plans associated with the user.
        /// </summary>
        [JsonProperty("recurringPaymentPlans")]
        public IList<RecurringPaymentPlan> RecurringPaymentPlans { get; set; }

        /// <summary>
        /// Gets o sets all installment payment plans associated with the user.
        /// </summary>
        [JsonProperty("installmentPaymentPlans")]
        public IList<InstallmentPaymentPlan> InstallmentPaymentPlans { get; set; }
    }
}