using System;
using System.Collections.Generic;
using System.Linq;
using Appulu.Core;
using Appulu.Core.Caching;
using Appulu.Core.Domain.Orders;
using Appulu.Core.Domain.Tax;
using Appulu.Services.Catalog;
using Appulu.Services.Directory;
using Appulu.Services.Helpers;
using Appulu.Services.Localization;
using Appulu.Services.Media;
using Appulu.Services.Orders;
using Appulu.Services.Seo;
using Appulu.Web.Infrastructure.Cache;
using Appulu.Web.Models.Order;

namespace Appulu.Web.Factories
{
    /// <summary>
    /// Represents the return request model factory
    /// </summary>
    public partial class ReturnRequestModelFactory : IReturnRequestModelFactory
    {
        #region Fields

        private readonly ICurrencyService _currencyService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IDownloadService _downloadService;
        private readonly ILocalizationService _localizationService;
        private readonly IOrderService _orderService;
        private readonly IPriceFormatter _priceFormatter;
        private readonly IReturnRequestService _returnRequestService;
        private readonly IStaticCacheManager _cacheManager;
        private readonly IStoreContext _storeContext;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IWorkContext _workContext;
        private readonly OrderSettings _orderSettings;

        #endregion

        #region Ctor

        public ReturnRequestModelFactory(ICurrencyService currencyService,
            IDateTimeHelper dateTimeHelper,
            IDownloadService downloadService,
            ILocalizationService localizationService,
            IOrderService orderService,
            IPriceFormatter priceFormatter,
            IReturnRequestService returnRequestService,
            IStaticCacheManager cacheManager,
            IStoreContext storeContext,
            IUrlRecordService urlRecordService,
            IWorkContext workContext,
            OrderSettings orderSettings)
        {
            this._currencyService = currencyService;
            this._dateTimeHelper = dateTimeHelper;
            this._downloadService = downloadService;
            this._localizationService = localizationService;
            this._orderService = orderService;
            this._priceFormatter = priceFormatter;
            this._returnRequestService = returnRequestService;
            this._cacheManager = cacheManager;
            this._storeContext = storeContext;
            this._urlRecordService = urlRecordService;
            this._workContext = workContext;
            this._orderSettings = orderSettings;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare the order item model
        /// </summary>
        /// <param name="orderItem">Order item</param>
        /// <returns>Order item model</returns>
        public virtual SubmitReturnRequestModel.OrderItemModel PrepareSubmitReturnRequestOrderItemModel(OrderItem orderItem)
        {
            if (orderItem == null)
                throw new ArgumentNullException(nameof(orderItem));

            var order = orderItem.Order;

            var model = new SubmitReturnRequestModel.OrderItemModel
            {
                Id = orderItem.Id,
                ProductId = orderItem.Product.Id,
                ProductName = _localizationService.GetLocalized(orderItem.Product, x => x.Name),
                ProductSeName = _urlRecordService.GetSeName(orderItem.Product),
                AttributeInfo = orderItem.AttributeDescription,
                Quantity = orderItem.Quantity
            };

            //unit price
            if (order.UserTaxDisplayType == TaxDisplayType.IncludingTax)
            {
                //including tax
                var unitPriceInclTaxInUserCurrency = _currencyService.ConvertCurrency(orderItem.UnitPriceInclTax, order.CurrencyRate);
                model.UnitPrice = _priceFormatter.FormatPrice(unitPriceInclTaxInUserCurrency, true, order.UserCurrencyCode, _workContext.WorkingLanguage, true);
            }
            else
            {
                //excluding tax
                var unitPriceExclTaxInUserCurrency = _currencyService.ConvertCurrency(orderItem.UnitPriceExclTax, order.CurrencyRate);
                model.UnitPrice = _priceFormatter.FormatPrice(unitPriceExclTaxInUserCurrency, true, order.UserCurrencyCode, _workContext.WorkingLanguage, false);
            }

            return model;
        }

        /// <summary>
        /// Prepare the submit return request model
        /// </summary>
        /// <param name="model">Submit return request model</param>
        /// <param name="order">Order</param>
        /// <returns>Submit return request model</returns>
        public virtual SubmitReturnRequestModel PrepareSubmitReturnRequestModel(SubmitReturnRequestModel model, Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            if (model == null)
                throw new ArgumentNullException(nameof(model));

            model.OrderId = order.Id;
            model.AllowFiles = _orderSettings.ReturnRequestsAllowFiles;
            model.CustomOrderNumber = order.CustomOrderNumber;

            //return reasons
            model.AvailableReturnReasons = _cacheManager.Get(string.Format(ModelCacheEventConsumer.RETURNREQUESTREASONS_MODEL_KEY, _workContext.WorkingLanguage.Id),
                () =>
                {
                    var reasons = new List<SubmitReturnRequestModel.ReturnRequestReasonModel>();
                    foreach (var rrr in _returnRequestService.GetAllReturnRequestReasons())
                        reasons.Add(new SubmitReturnRequestModel.ReturnRequestReasonModel
                        {
                            Id = rrr.Id,
                            Name = _localizationService.GetLocalized(rrr, x => x.Name)
                        });
                    return reasons;
                });

            //return actions
            model.AvailableReturnActions = _cacheManager.Get(string.Format(ModelCacheEventConsumer.RETURNREQUESTACTIONS_MODEL_KEY, _workContext.WorkingLanguage.Id),
                () =>
                {
                    var actions = new List<SubmitReturnRequestModel.ReturnRequestActionModel>();
                    foreach (var rra in _returnRequestService.GetAllReturnRequestActions())
                        actions.Add(new SubmitReturnRequestModel.ReturnRequestActionModel
                        {
                            Id = rra.Id,
                            Name = _localizationService.GetLocalized(rra, x => x.Name)
                        });
                    return actions;
                });

            //returnable products
            var orderItems = order.OrderItems.Where(oi => !oi.Product.NotReturnable);
            foreach (var orderItem in orderItems)
            {
                var orderItemModel = PrepareSubmitReturnRequestOrderItemModel(orderItem);
                model.Items.Add(orderItemModel);
            }

            return model;
        }

        /// <summary>
        /// Prepare the user return requests model
        /// </summary>
        /// <returns>User return requests model</returns>
        public virtual UserReturnRequestsModel PrepareUserReturnRequestsModel()
        {
            var model = new UserReturnRequestsModel();

            var returnRequests = _returnRequestService.SearchReturnRequests(_storeContext.CurrentStore.Id, _workContext.CurrentUser.Id);
            foreach (var returnRequest in returnRequests)
            {
                var orderItem = _orderService.GetOrderItemById(returnRequest.OrderItemId);
                if (orderItem != null)
                {
                    var product = orderItem.Product;
                    var download = _downloadService.GetDownloadById(returnRequest.UploadedFileId);

                    var itemModel = new UserReturnRequestsModel.ReturnRequestModel
                    {
                        Id = returnRequest.Id,
                        CustomNumber = returnRequest.CustomNumber,
                        ReturnRequestStatus = _localizationService.GetLocalizedEnum(returnRequest.ReturnRequestStatus),
                        ProductId = product.Id,
                        ProductName = _localizationService.GetLocalized(product, x => x.Name),
                        ProductSeName = _urlRecordService.GetSeName(product),
                        Quantity = returnRequest.Quantity,
                        ReturnAction = returnRequest.RequestedAction,
                        ReturnReason = returnRequest.ReasonForReturn,
                        Comments = returnRequest.UserComments,
                        UploadedFileGuid = download != null ? download.DownloadGuid : Guid.Empty,
                        CreatedOn = _dateTimeHelper.ConvertToUserTime(returnRequest.CreatedOnUtc, DateTimeKind.Utc),
                    };
                    model.Items.Add(itemModel);
                }
            }

            return model;
        }

        #endregion
    }
}