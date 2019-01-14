using System.Linq;
using System.Security.Claims;
using Appulu.Core.Domain.Users;
using Appulu.Services.Authentication.External;
using Appulu.Services.Common;
using Appulu.Services.Events;

namespace Appulu.Plugin.ExternalAuth.Facebook.Infrastructure.Cache
{
    /// <summary>
    /// Facebook authentication event consumer (used for saving user fields on registration)
    /// </summary>
    public partial class FacebookAuthenticationEventConsumer : IConsumer<UserAutoRegisteredByExternalMethodEvent>
    {
        #region Fields
        
        private readonly IGenericAttributeService _genericAttributeService;

        #endregion

        #region Ctor

        public FacebookAuthenticationEventConsumer(IGenericAttributeService genericAttributeService)
        {
            this._genericAttributeService = genericAttributeService;
        }

        #endregion

        #region Methods

        public void HandleEvent(UserAutoRegisteredByExternalMethodEvent eventMessage)
        {
            if (eventMessage?.User == null || eventMessage.AuthenticationParameters == null)
                return;

            //handle event only for this authentication method
            if (!eventMessage.AuthenticationParameters.ProviderSystemName.Equals(FacebookAuthenticationDefaults.ProviderSystemName))
                return;

            //store some of the user fields
            var firstName = eventMessage.AuthenticationParameters.Claims?.FirstOrDefault(claim => claim.Type == ClaimTypes.GivenName)?.Value;
            if (!string.IsNullOrEmpty(firstName))
                _genericAttributeService.SaveAttribute(eventMessage.User, AppUserDefaults.FirstNameAttribute, firstName);

            var lastName = eventMessage.AuthenticationParameters.Claims?.FirstOrDefault(claim => claim.Type == ClaimTypes.Surname)?.Value;
            if (!string.IsNullOrEmpty(lastName))
                _genericAttributeService.SaveAttribute(eventMessage.User, AppUserDefaults.LastNameAttribute, lastName);
        }

        #endregion
    }
}