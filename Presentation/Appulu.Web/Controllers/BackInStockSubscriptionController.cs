using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Appulu.Core;
using Appulu.Core.Domain.Catalog;
using Appulu.Core.Domain.Users;
using Appulu.Services.Catalog;
using Appulu.Services.Localization;
using Appulu.Services.Seo;
using Appulu.Web.Models.Catalog;
using Appulu.Web.Models.Common;

namespace Appulu.Web.Controllers
{
    public partial class BackInStockSubscriptionController : BasePublicController
    {
        #region Fields

        private readonly CatalogSettings _catalogSettings;
        private readonly UserSettings _userSettings;
        private readonly IBackInStockSubscriptionService _backInStockSubscriptionService;
        private readonly ILocalizationService _localizationService;
        private readonly IProductService _productService;
        private readonly IStoreContext _storeContext;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public BackInStockSubscriptionController(CatalogSettings catalogSettings,
            UserSettings userSettings,
            IBackInStockSubscriptionService backInStockSubscriptionService,
            ILocalizationService localizationService,
            IProductService productService,
            IStoreContext storeContext,
            IUrlRecordService urlRecordService,
            IWorkContext workContext)
        {
            this._catalogSettings = catalogSettings;
            this._userSettings = userSettings;
            this._backInStockSubscriptionService = backInStockSubscriptionService;
            this._localizationService = localizationService;
            this._productService = productService;
            this._storeContext = storeContext;
            this._urlRecordService = urlRecordService;
            this._workContext = workContext;
        }

        #endregion

        #region Methods

        // Product details page > back in stock subscribe
        public virtual IActionResult SubscribePopup(int productId)
        {
            var product = _productService.GetProductById(productId);
            if (product == null || product.Deleted)
                throw new ArgumentException("No product found with the specified id");

            var model = new BackInStockSubscribeModel
            {
                ProductId = product.Id,
                ProductName = _localizationService.GetLocalized(product, x => x.Name),
                ProductSeName = _urlRecordService.GetSeName(product),
                IsCurrentUserRegistered = _workContext.CurrentUser.IsRegistered(),
                MaximumBackInStockSubscriptions = _catalogSettings.MaximumBackInStockSubscriptions,
                CurrentNumberOfBackInStockSubscriptions = _backInStockSubscriptionService
                .GetAllSubscriptionsByUserId(_workContext.CurrentUser.Id, _storeContext.CurrentStore.Id, 0, 1)
                .TotalCount
            };
            if (product.ManageInventoryMethod == ManageInventoryMethod.ManageStock &&
                product.BackorderMode == BackorderMode.NoBackorders &&
                product.AllowBackInStockSubscriptions &&
                _productService.GetTotalStockQuantity(product) <= 0)
            {
                //out of stock
                model.SubscriptionAllowed = true;
                model.AlreadySubscribed = _backInStockSubscriptionService
                    .FindSubscription(_workContext.CurrentUser.Id, product.Id, _storeContext.CurrentStore.Id) != null;
            }
            return PartialView(model);
        }

        [HttpPost]
        public virtual IActionResult SubscribePopupPOST(int productId)
        {
            var product = _productService.GetProductById(productId);
            if (product == null || product.Deleted)
                throw new ArgumentException("No product found with the specified id");

            if (!_workContext.CurrentUser.IsRegistered())
                return Content(_localizationService.GetResource("BackInStockSubscriptions.OnlyRegistered"));

            if (product.ManageInventoryMethod == ManageInventoryMethod.ManageStock &&
                product.BackorderMode == BackorderMode.NoBackorders &&
                product.AllowBackInStockSubscriptions &&
                _productService.GetTotalStockQuantity(product) <= 0)
            {
                //out of stock
                var subscription = _backInStockSubscriptionService
                    .FindSubscription(_workContext.CurrentUser.Id, product.Id, _storeContext.CurrentStore.Id);
                if (subscription != null)
                {
                    //subscription already exists
                    //unsubscribe
                    _backInStockSubscriptionService.DeleteSubscription(subscription);

                    return Json(new
                    {
                        result = "Unsubscribed"
                    });
                }

                //subscription does not exist
                //subscribe
                if (_backInStockSubscriptionService
                    .GetAllSubscriptionsByUserId(_workContext.CurrentUser.Id, _storeContext.CurrentStore.Id, 0, 1)
                    .TotalCount >= _catalogSettings.MaximumBackInStockSubscriptions)
                {
                    return Json(new
                    {
                        result = string.Format(_localizationService.GetResource("BackInStockSubscriptions.MaxSubscriptions"), _catalogSettings.MaximumBackInStockSubscriptions)
                    });
                }
                subscription = new BackInStockSubscription
                {
                    User = _workContext.CurrentUser,
                    Product = product,
                    StoreId = _storeContext.CurrentStore.Id,
                    CreatedOnUtc = DateTime.UtcNow
                };
                _backInStockSubscriptionService.InsertSubscription(subscription);

                return Json(new
                {
                    result = "Subscribed"
                });
            }

            //subscription not possible
            return Content(_localizationService.GetResource("BackInStockSubscriptions.NotAllowed"));
        }

        // My account / Back in stock subscriptions
        public virtual IActionResult UserSubscriptions(int? pageNumber)
        {
            if (_userSettings.HideBackInStockSubscriptionsTab)
            {
                return RedirectToRoute("UserInfo");
            }

            var pageIndex = 0;
            if (pageNumber > 0)
            {
                pageIndex = pageNumber.Value - 1;
            }
            var pageSize = 10;

            var user = _workContext.CurrentUser;
            var list = _backInStockSubscriptionService.GetAllSubscriptionsByUserId(user.Id,
                _storeContext.CurrentStore.Id, pageIndex, pageSize);

            var model = new UserBackInStockSubscriptionsModel();

            foreach (var subscription in list)
            {
                var product = subscription.Product;

                if (product != null)
                {
                    var subscriptionModel = new UserBackInStockSubscriptionsModel.BackInStockSubscriptionModel
                    {
                        Id = subscription.Id,
                        ProductId = product.Id,
                        ProductName = _localizationService.GetLocalized(product, x => x.Name),
                        SeName = _urlRecordService.GetSeName(product),
                    };
                    model.Subscriptions.Add(subscriptionModel);
                }
            }

            model.PagerModel = new PagerModel
            {
                PageSize = list.PageSize,
                TotalRecords = list.TotalCount,
                PageIndex = list.PageIndex,
                ShowTotalSummary = false,
                RouteActionName = "UserBackInStockSubscriptionsPaged",
                UseRouteLinks = true,
                RouteValues = new BackInStockSubscriptionsRouteValues { pageNumber = pageIndex }
            };

            return View(model);
        }

        [HttpPost, ActionName("UserSubscriptions")]
        public virtual IActionResult UserSubscriptionsPOST(IFormCollection formCollection)
        {
            foreach (var key in formCollection.Keys)
            {
                var value = formCollection[key];

                if (value.Equals("on") && key.StartsWith("biss", StringComparison.InvariantCultureIgnoreCase))
                {
                    var id = key.Replace("biss", "").Trim();
                    if (int.TryParse(id, out int subscriptionId))
                    {
                        var subscription = _backInStockSubscriptionService.GetSubscriptionById(subscriptionId);
                        if (subscription != null && subscription.UserId == _workContext.CurrentUser.Id)
                        {
                            _backInStockSubscriptionService.DeleteSubscription(subscription);
                        }
                    }
                }
            }

            return RedirectToRoute("UserBackInStockSubscriptions");
        }

        #endregion
    }
}