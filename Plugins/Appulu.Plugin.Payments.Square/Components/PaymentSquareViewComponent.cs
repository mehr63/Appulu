using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Appulu.Core;
using Appulu.Core.Domain.Users;
using Appulu.Plugin.Payments.Square.Models;
using Appulu.Plugin.Payments.Square.Services;
using Appulu.Services.Common;
using Appulu.Services.Localization;

namespace Appulu.Plugin.Payments.Square.Components
{
    [ViewComponent(Name = SquarePaymentDefaults.ViewComponentName)]
    public class PaymentSquareViewComponent : ViewComponent
    {
        #region Fields

        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;
        private readonly SquarePaymentManager _squarePaymentManager;

        #endregion

        #region Ctor

        public PaymentSquareViewComponent(IGenericAttributeService genericAttributeService,
            ILocalizationService localizationService,
            IWorkContext workContext,
            SquarePaymentManager squarePaymentManager)
        {
            this._genericAttributeService = genericAttributeService;
            this._localizationService = localizationService;
            this._workContext = workContext;
            this._squarePaymentManager = squarePaymentManager;
        }

        #endregion

        #region Methods

        public IViewComponentResult Invoke()
        {
            var model = new PaymentInfoModel
            {

                //whether current user is guest
                IsGuest = _workContext.CurrentUser.IsGuest(),

                //get postal code from the billing address or from the shipping one
                PostalCode = _workContext.CurrentUser.BillingAddress?.ZipPostalCode
                ?? _workContext.CurrentUser.ShippingAddress?.ZipPostalCode
            };

            //whether user already has stored cards
            var userId = _genericAttributeService.GetAttribute<string>(_workContext.CurrentUser, SquarePaymentDefaults.UserIdAttribute);
            var user = _squarePaymentManager.GetUser(userId);
            if (user?.Cards != null)
            {
                var cardNumberMask = _localizationService.GetResource("Plugins.Payments.Square.Fields.StoredCard.Mask");
                model.StoredCards = user.Cards.Select(card => new SelectListItem { Text = string.Format(cardNumberMask, card.Last4), Value = card.Id }).ToList();
            }

            //add the special item for 'select card' with value 0
            if (model.StoredCards.Any())
            {
                var selectCardText = _localizationService.GetResource("Plugins.Payments.Square.Fields.StoredCard.SelectCard");
                model.StoredCards.Insert(0, new SelectListItem { Text = selectCardText, Value = "0" });
            }

            return View("~/Plugins/Payments.Square/Views/PaymentInfo.cshtml", model);
        }

        #endregion
    }
}