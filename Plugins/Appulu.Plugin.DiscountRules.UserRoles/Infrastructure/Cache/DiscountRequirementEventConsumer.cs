using Appulu.Core.Domain.Discounts;
using Appulu.Core.Events;
using Appulu.Services.Configuration;
using Appulu.Services.Events;

namespace Appulu.Plugin.DiscountRules.UserRoles.Infrastructure.Cache
{
    /// <summary>
    /// Discount requirement rule event consumer (used for removing unused settings)
    /// </summary>
    public partial class DiscountRequirementEventConsumer : IConsumer<EntityDeletedEvent<DiscountRequirement>>
    {
        #region Fields
        
        private readonly ISettingService _settingService;

        #endregion

        #region Ctor

        public DiscountRequirementEventConsumer(ISettingService settingService)
        {
            this._settingService = settingService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handle discount requirement deleted event
        /// </summary>
        /// <param name="eventMessage">Event message</param>
        public void HandleEvent(EntityDeletedEvent<DiscountRequirement> eventMessage)
        {
            var discountRequirement = eventMessage?.Entity;
            if (discountRequirement == null)
                return;

            //delete saved restricted user role identifier if exists
            var setting = _settingService.GetSetting(string.Format(DiscountRequirementDefaults.SettingsKey, discountRequirement.Id));
            if (setting != null)
                _settingService.DeleteSetting(setting);
        }

        #endregion
    }
}