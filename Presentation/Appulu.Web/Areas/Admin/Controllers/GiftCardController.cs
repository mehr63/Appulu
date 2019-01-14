using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Appulu.Core;
using Appulu.Core.Domain.Directory;
using Appulu.Core.Domain.Localization;
using Appulu.Core.Domain.Orders;
using Appulu.Services.Catalog;
using Appulu.Services.Directory;
using Appulu.Services.Helpers;
using Appulu.Services.Localization;
using Appulu.Services.Logging;
using Appulu.Services.Messages;
using Appulu.Services.Orders;
using Appulu.Services.Security;
using Appulu.Web.Areas.Admin.Factories;
using Appulu.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Appulu.Web.Areas.Admin.Models.Orders;
using Appulu.Web.Framework.Controllers;
using Appulu.Web.Framework.Mvc.Filters;

namespace Appulu.Web.Areas.Admin.Controllers
{
    public partial class GiftCardController : BaseAdminController
    {
        #region Fields

        private readonly CurrencySettings _currencySettings;
        private readonly ICurrencyService _currencyService;
        private readonly IUserActivityService _userActivityService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IGiftCardModelFactory _giftCardModelFactory;
        private readonly IGiftCardService _giftCardService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly IPriceFormatter _priceFormatter;
        private readonly IWorkflowMessageService _workflowMessageService;
        private readonly LocalizationSettings _localizationSettings;

        #endregion

        #region Ctor

        public GiftCardController(CurrencySettings currencySettings,
            ICurrencyService currencyService,
            IUserActivityService userActivityService,
            IDateTimeHelper dateTimeHelper,
            IGiftCardModelFactory giftCardModelFactory,
            IGiftCardService giftCardService,
            ILanguageService languageService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            IPriceFormatter priceFormatter,
            IWorkflowMessageService workflowMessageService,
            LocalizationSettings localizationSettings)
        {
            this._currencySettings = currencySettings;
            this._currencyService = currencyService;
            this._userActivityService = userActivityService;
            this._dateTimeHelper = dateTimeHelper;
            this._giftCardModelFactory = giftCardModelFactory;
            this._giftCardService = giftCardService;
            this._languageService = languageService;
            this._localizationService = localizationService;
            this._permissionService = permissionService;
            this._priceFormatter = priceFormatter;
            this._workflowMessageService = workflowMessageService;
            this._localizationSettings = localizationSettings;
        }

        #endregion

        #region Methods

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageGiftCards))
                return AccessDeniedView();

            //prepare model
            var model = _giftCardModelFactory.PrepareGiftCardSearchModel(new GiftCardSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult GiftCardList(GiftCardSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageGiftCards))
                return AccessDeniedKendoGridJson();

            //prepare model
            var model = _giftCardModelFactory.PrepareGiftCardListModel(searchModel);

            return Json(model);
        }

        public virtual IActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageGiftCards))
                return AccessDeniedView();

            //prepare model
            var model = _giftCardModelFactory.PrepareGiftCardModel(new GiftCardModel(), null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Create(GiftCardModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageGiftCards))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var giftCard = model.ToEntity<GiftCard>();
                giftCard.CreatedOnUtc = DateTime.UtcNow;
                _giftCardService.InsertGiftCard(giftCard);

                //activity log
                _userActivityService.InsertActivity("AddNewGiftCard",
                    string.Format(_localizationService.GetResource("ActivityLog.AddNewGiftCard"), giftCard.GiftCardCouponCode), giftCard);

                SuccessNotification(_localizationService.GetResource("Admin.GiftCards.Added"));

                return continueEditing ? RedirectToAction("Edit", new { id = giftCard.Id }) : RedirectToAction("List");
            }

            //prepare model
            model = _giftCardModelFactory.PrepareGiftCardModel(model, null, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        public virtual IActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageGiftCards))
                return AccessDeniedView();

            //try to get a gift card with the specified id
            var giftCard = _giftCardService.GetGiftCardById(id);
            if (giftCard == null)
                return RedirectToAction("List");

            //prepare model
            var model = _giftCardModelFactory.PrepareGiftCardModel(null, giftCard);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public virtual IActionResult Edit(GiftCardModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageGiftCards))
                return AccessDeniedView();

            //try to get a gift card with the specified id
            var giftCard = _giftCardService.GetGiftCardById(model.Id);
            if (giftCard == null)
                return RedirectToAction("List");

            model.PurchasedWithOrderId = giftCard.PurchasedWithOrderItem != null ? (int?)giftCard.PurchasedWithOrderItem.OrderId : null;
            model.RemainingAmountStr = _priceFormatter.FormatPrice(_giftCardService.GetGiftCardRemainingAmount(giftCard), true, false);
            model.AmountStr = _priceFormatter.FormatPrice(giftCard.Amount, true, false);
            model.CreatedOn = _dateTimeHelper.ConvertToUserTime(giftCard.CreatedOnUtc, DateTimeKind.Utc);
            model.PrimaryStoreCurrencyCode = _currencyService.GetCurrencyById(_currencySettings.PrimaryStoreCurrencyId).CurrencyCode;
            model.PurchasedWithOrderNumber = giftCard.PurchasedWithOrderItem?.Order.CustomOrderNumber;

            if (ModelState.IsValid)
            {
                giftCard = model.ToEntity(giftCard);
                _giftCardService.UpdateGiftCard(giftCard);

                //activity log
                _userActivityService.InsertActivity("EditGiftCard",
                    string.Format(_localizationService.GetResource("ActivityLog.EditGiftCard"), giftCard.GiftCardCouponCode), giftCard);

                SuccessNotification(_localizationService.GetResource("Admin.GiftCards.Updated"));

                if (!continueEditing)
                    return RedirectToAction("List");

                //selected tab
                SaveSelectedTabName();

                return RedirectToAction("Edit", new { id = giftCard.Id });
            }

            //prepare model
            model = _giftCardModelFactory.PrepareGiftCardModel(model, giftCard, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public virtual IActionResult GenerateCouponCode()
        {
            return Json(new { CouponCode = _giftCardService.GenerateGiftCardCode() });
        }

        [HttpPost, ActionName("Edit")]
        [FormValueRequired("notifyRecipient")]
        public virtual IActionResult NotifyRecipient(GiftCardModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageGiftCards))
                return AccessDeniedView();

            //try to get a gift card with the specified id
            var giftCard = _giftCardService.GetGiftCardById(model.Id);
            if (giftCard == null)
                return RedirectToAction("List");

            model = giftCard.ToModel(model);
            model.PurchasedWithOrderId = giftCard.PurchasedWithOrderItem != null ? (int?)giftCard.PurchasedWithOrderItem.OrderId : null;
            model.RemainingAmountStr = _priceFormatter.FormatPrice(_giftCardService.GetGiftCardRemainingAmount(giftCard), true, false);
            model.AmountStr = _priceFormatter.FormatPrice(giftCard.Amount, true, false);
            model.CreatedOn = _dateTimeHelper.ConvertToUserTime(giftCard.CreatedOnUtc, DateTimeKind.Utc);
            model.PrimaryStoreCurrencyCode = _currencyService.GetCurrencyById(_currencySettings.PrimaryStoreCurrencyId).CurrencyCode;
            model.PurchasedWithOrderNumber = giftCard.PurchasedWithOrderItem?.Order.CustomOrderNumber;

            try
            {
                if (!CommonHelper.IsValidEmail(giftCard.RecipientEmail))
                    throw new AppException("Recipient email is not valid");

                if (!CommonHelper.IsValidEmail(giftCard.SenderEmail))
                    throw new AppException("Sender email is not valid");

                var languageId = 0;
                var order = giftCard.PurchasedWithOrderItem?.Order;
                if (order != null)
                {
                    var userLang = _languageService.GetLanguageById(order.UserLanguageId);
                    if (userLang == null)
                        userLang = _languageService.GetAllLanguages().FirstOrDefault();
                    if (userLang != null)
                        languageId = userLang.Id;
                }
                else
                {
                    languageId = _localizationSettings.DefaultAdminLanguageId;
                }

                var queuedEmailIds = _workflowMessageService.SendGiftCardNotification(giftCard, languageId);
                if (queuedEmailIds.Any())
                {
                    giftCard.IsRecipientNotified = true;
                    _giftCardService.UpdateGiftCard(giftCard);
                    model.IsRecipientNotified = true;
                }
            }
            catch (Exception exc)
            {
                ErrorNotification(exc, false);
            }

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageGiftCards))
                return AccessDeniedView();

            //try to get a gift card with the specified id
            var giftCard = _giftCardService.GetGiftCardById(id);
            if (giftCard == null)
                return RedirectToAction("List");

            _giftCardService.DeleteGiftCard(giftCard);

            //activity log
            _userActivityService.InsertActivity("DeleteGiftCard",
                string.Format(_localizationService.GetResource("ActivityLog.DeleteGiftCard"), giftCard.GiftCardCouponCode), giftCard);

            SuccessNotification(_localizationService.GetResource("Admin.GiftCards.Deleted"));

            return RedirectToAction("List");
        }

        [HttpPost]
        public virtual IActionResult UsageHistoryList(GiftCardUsageHistorySearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageGiftCards))
                return AccessDeniedKendoGridJson();

            //try to get a gift card with the specified id
            var giftCard = _giftCardService.GetGiftCardById(searchModel.GiftCardId)
                ?? throw new ArgumentException("No gift card found with the specified id");

            //prepare model
            var model = _giftCardModelFactory.PrepareGiftCardUsageHistoryListModel(searchModel, giftCard);

            return Json(model);
        }

        #endregion
    }
}