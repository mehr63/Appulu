using System.Linq;
using Appulu.Services.Events;
using Appulu.Services.Localization;
using Appulu.Services.Payments;
using Appulu.Services.Tasks;
using Appulu.Web.Areas.Admin.Models.Tasks;
using Appulu.Web.Framework.Events;
using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.UI;

namespace Appulu.Plugin.Payments.Square.Services
{
    /// <summary>
    /// Represents event consumer of the Square payment plugin
    /// </summary>
    public class EventConsumer :
        IConsumer<PageRenderingEvent>,
        IConsumer<ModelReceivedEvent<BaseAppModel>>
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly IPaymentService _paymentService;
        private readonly IScheduleTaskService _scheduleTaskService;

        #endregion

        #region Ctor

        public EventConsumer(ILocalizationService localizationService,
            IPaymentService paymentService,
            IScheduleTaskService scheduleTaskService)
        {
            this._localizationService = localizationService;
            this._paymentService = paymentService;
            this._scheduleTaskService = scheduleTaskService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handle page rendering event
        /// </summary>
        /// <param name="eventMessage">Event message</param>
        public void HandleEvent(PageRenderingEvent eventMessage)
        {
            if (eventMessage?.Helper?.ViewContext?.ActionDescriptor == null)
                return;

            //check whether the plugin is installed and is active
            var squarePaymentMethod = _paymentService.LoadPaymentMethodBySystemName(SquarePaymentDefaults.SystemName);
            if (!(squarePaymentMethod?.PluginDescriptor?.Installed ?? false) || !_paymentService.IsPaymentMethodActive(squarePaymentMethod))
                return;

            //add js script to one page checkout
            if (eventMessage.GetRouteNames().Any(r => r.Equals("CheckoutOnePage")))
            {
                eventMessage.Helper.AddScriptParts(ResourceLocation.Footer, SquarePaymentDefaults.PaymentFormScriptPath, excludeFromBundle: true);
            }
        }

        /// <summary>
        /// Handle model received event
        /// </summary>
        /// <param name="eventMessage">Event message</param>
        public void HandleEvent(ModelReceivedEvent<BaseAppModel> eventMessage)
        {
            //whether received model is ScheduleTaskModel
            if (!(eventMessage?.Model is ScheduleTaskModel scheduleTaskModel))
                return;

            //whether renew access token task is changed
            var scheduleTask = _scheduleTaskService.GetTaskById(scheduleTaskModel.Id);
            if (!scheduleTask?.Type.Equals(SquarePaymentDefaults.RenewAccessTokenTask) ?? true)
                return;

            //check whether the plugin is installed
            if (!(_paymentService.LoadPaymentMethodBySystemName(SquarePaymentDefaults.SystemName)?.PluginDescriptor?.Installed ?? false))
                return;

            //check token renewal limit
            var accessTokenRenewalPeriod = scheduleTaskModel.Seconds / 60 / 60 / 24;
            if (accessTokenRenewalPeriod > SquarePaymentDefaults.AccessTokenRenewalPeriodMax)
            {
                var error = string.Format(_localizationService.GetResource("Plugins.Payments.Square.AccessTokenRenewalPeriod.Error"),
                    SquarePaymentDefaults.AccessTokenRenewalPeriodMax, SquarePaymentDefaults.AccessTokenRenewalPeriodRecommended);
                eventMessage.ModelState.AddModelError(string.Empty, error);
            }
        }

        #endregion
    }
}