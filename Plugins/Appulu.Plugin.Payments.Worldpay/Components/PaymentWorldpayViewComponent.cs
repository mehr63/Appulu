using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Appulu.Core;
using Appulu.Core.Domain.Users;
using Appulu.Plugin.Payments.Worldpay.Domain.Enums;
using Appulu.Plugin.Payments.Worldpay.Models;
using Appulu.Plugin.Payments.Worldpay.Services;
using Appulu.Services.Common;
using Appulu.Services.Localization;

namespace Appulu.Plugin.Payments.Worldpay.Components
{
    [ViewComponent(Name = WorldpayPaymentDefaults.ViewComponentName)]
    public class PaymentWorldpayViewComponent : ViewComponent
    {
        #region Fields

        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;
        private readonly WorldpayPaymentManager _worldpayPaymentManager;

        #endregion

        #region Ctor

        public PaymentWorldpayViewComponent(IGenericAttributeService genericAttributeService,
            ILocalizationService localizationService,
            IWorkContext workContext,
            WorldpayPaymentManager worldpayPaymentManager)
        {
            this._genericAttributeService = genericAttributeService;
            this._localizationService = localizationService;
            this._workContext = workContext;
            this._worldpayPaymentManager = worldpayPaymentManager;
        }

        #endregion

        #region Methods

        public IViewComponentResult Invoke()
        {
            var model = new PaymentInfoModel();

            //prepare years
            for (var i = 0; i < 15; i++)
            {
                var year = (DateTime.Now.Year + i).ToString();
                model.ExpireYears.Add(new SelectListItem { Text = year, Value = year, });
            }

            //prepare months
            for (var i = 1; i <= 12; i++)
            {
                model.ExpireMonths.Add(new SelectListItem { Text = i.ToString("D2"), Value = i.ToString(), });
            }

            //prepare card types
            model.CardTypes = Enum.GetValues(typeof(CreditCardType)).OfType<CreditCardType>().Select(cardType => new SelectListItem
            {
                Value = JsonConvert.SerializeObject(cardType, new StringEnumConverter()),
                Text = _localizationService.GetLocalizedEnum(cardType)
            }).ToList();

            //whether current user is guest
            model.IsGuest = _workContext.CurrentUser.IsGuest();
            if (!model.IsGuest)
            {
                //whether user already has stored cards
                var user = _worldpayPaymentManager.GetUser(_genericAttributeService.GetAttribute<string>(_workContext.CurrentUser, WorldpayPaymentDefaults.UserIdAttribute));
                if (user?.PaymentMethods != null)
                {
                    model.StoredCards = user.PaymentMethods.Where(method => method?.Card != null)
                        .Select(method => new SelectListItem { Text = method.Card.MaskedNumber, Value = method.PaymentId }).ToList();
                }

                //add the special item for 'select card' with empty GUID value 
                if (model.StoredCards.Any())
                {
                    var selectCardText = _localizationService.GetResource("Plugins.Payments.Worldpay.Fields.StoredCard.SelectCard");
                    model.StoredCards.Insert(0, new SelectListItem { Text = selectCardText, Value = Guid.Empty.ToString() });
                }
            }

            return View("~/Plugins/Payments.Worldpay/Views/PaymentInfo.cshtml", model);
        }

        #endregion
    }
}