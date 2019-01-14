using System.Collections.Generic;
using System.Runtime.Serialization;
using Square.Connect.Model;

namespace Appulu.Plugin.Payments.Square.Domain
{
    /// <summary>
    /// Represents the parameters (with additional one) that can be included in the body of a request to the charge endpoint.
    /// </summary>
    public class ExtendedChargeRequest : ChargeRequest
    {
        #region Ctor

        public ExtendedChargeRequest(string IntegrationId = null,
            string IdempotencyKey = null, 
            Money AmountMoney = null, 
            string CardNonce = null, 
            string UserCardId = null, 
            bool? DelayCapture = null, 
            string ReferenceId = null, 
            string Note = null, 
            string UserId = null, 
            Address BillingAddress = null, 
            Address ShippingAddress = null, 
            string BuyerEmailAddress = null, 
            string OrderId = null, 
            List<AdditionalRecipient> AdditionalRecipients = null) : base(IdempotencyKey,
                AmountMoney,
                CardNonce,
                UserCardId,
                DelayCapture,
                ReferenceId,
                Note,
                UserId,
                BillingAddress,
                ShippingAddress,
                BuyerEmailAddress,
                OrderId,
                AdditionalRecipients)
        {
            this.IntegrationId = IntegrationId;
        }

        #endregion

        #region Properties

        /// <summary>
        /// An extra parameter in the request in order to track revenue share transaction attribution from 3rd party merchant/developer hosted apps (where the application id will not be known up front).
        /// </summary>
        /// <value>An extra parameter in the request in order to track revenue share transaction attribution from 3rd party merchant/developer hosted apps (where the application id will not be known up front).</value>
        [DataMember(Name = "integration_id", EmitDefaultValue = false)]
        public string IntegrationId { get; set; }

        #endregion
    }
}