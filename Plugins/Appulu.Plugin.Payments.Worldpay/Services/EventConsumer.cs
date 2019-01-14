using System.Linq;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Appulu.Core.Domain.Users;
using Appulu.Plugin.Payments.Worldpay.Models.User;
using Appulu.Services.Common;
using Appulu.Services.Users;
using Appulu.Services.Events;
using Appulu.Services.Localization;
using Appulu.Services.Payments;
using Appulu.Web.Areas.Admin.Models.Users;
using Appulu.Web.Framework.Events;
using Appulu.Web.Framework.Extensions;
using Appulu.Web.Framework.UI;

namespace Appulu.Plugin.Payments.Worldpay.Services
{
    /// <summary>
    /// Represents event consumer of the Worldpay payment plugin
    /// </summary>
    public class EventConsumer :
        IConsumer<PageRenderingEvent>,
        IConsumer<AdminTabStripCreated>
    {
        #region Fields

        private readonly IUserService _userService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ILocalizationService _localizationService;
        private readonly IPaymentService _paymentService;
        private readonly WorldpayPaymentManager _worldpayPaymentManager;

        #endregion

        #region Ctor

        public EventConsumer(IUserService userService,
            IGenericAttributeService genericAttributeService,
            ILocalizationService localizationService,
            IPaymentService paymentService,
            WorldpayPaymentManager worldpayPaymentManager)
        {
            this._userService = userService;
            this._genericAttributeService = genericAttributeService;
            this._localizationService = localizationService;
            this._paymentService = paymentService;
            this._worldpayPaymentManager = worldpayPaymentManager;
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

            //check whether the payment plugin is installed and is active
            var worldpayPaymentMethod = _paymentService.LoadPaymentMethodBySystemName(WorldpayPaymentDefaults.SystemName);
            if (!(worldpayPaymentMethod?.PluginDescriptor?.Installed ?? false) || !_paymentService.IsPaymentMethodActive(worldpayPaymentMethod))
                return;

            //add js sсript to one page checkout
            if (eventMessage.GetRouteNames().Any(r => r.Equals("CheckoutOnePage")))
                eventMessage.Helper.AddScriptParts(ResourceLocation.Footer, WorldpayPaymentDefaults.PaymentScriptPath, excludeFromBundle: true);
        }

        /// <summary>
        /// Handle admin tabstrip created event
        /// </summary>
        /// <param name="eventMessage">Event message</param>
        public async void HandleEvent(AdminTabStripCreated eventMessage)
        {
            if (eventMessage?.Helper == null)
                return;

            //we need user details page
            var tabsElementId = "user-edit";
            if (!eventMessage.TabStripName.Equals(tabsElementId))
                return;

            //check whether the payment plugin is installed and is active
            var worldpayPaymentMethod = _paymentService.LoadPaymentMethodBySystemName(WorldpayPaymentDefaults.SystemName);
            if (!(worldpayPaymentMethod?.PluginDescriptor?.Installed ?? false) || !_paymentService.IsPaymentMethodActive(worldpayPaymentMethod))
                return;

            //get the view model
            if (!(eventMessage.Helper.ViewData.Model is UserModel userModel))
                return;

            //check whether a user exists and isn't guest
            var user = _userService.GetUserById(userModel.Id);
            if (user == null || user.IsGuest())
                return;

            //try to get stored in Vault user
            var vaultUser = _worldpayPaymentManager.GetUser(_genericAttributeService.GetAttribute<string>(user, WorldpayPaymentDefaults.UserIdAttribute));

            //prepare model
            var model = new WorldpayUserModel
            {
                Id = userModel.Id,
                UserExists = vaultUser != null,
                WorldpayUserId = vaultUser?.UserId
            };

            //compose script to create a new tab
            var worldpayUserTabElementId = "tab-worldpay";
            var worldpayUserTab = new HtmlString($@"
                <script>
                    $(document).ready(function() {{
                        $(`
                            <li>
                                <a data-tab-name='{worldpayUserTabElementId}' data-toggle='tab' href='#{worldpayUserTabElementId}'>
                                    {_localizationService.GetResource("Plugins.Payments.Worldpay.WorldpayUser")}
                                </a>
                            </li>
                        `).appendTo('#{tabsElementId} .nav-tabs:first');
                        $(`
                            <div class='tab-pane' id='{worldpayUserTabElementId}'>
                                {
                                    (await eventMessage.Helper.PartialAsync("~/Plugins/Payments.Worldpay/Views/User/_CreateOrUpdate.Worldpay.cshtml", model)).RenderHtmlContent()
                                        .Replace("</script>", "<\\/script>") //we need escape a closing script tag to prevent terminating the script block early
                                }
                            </div>
                        `).appendTo('#{tabsElementId} .tab-content:first');
                    }});
                </script>");

            //add this tab as a block to render on the user details page
            eventMessage.BlocksToRender.Add(worldpayUserTab);
        }

        #endregion
    }
}