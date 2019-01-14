using System;
using System.Collections.Generic;
using System.Linq;
using Appulu.Core;
using Appulu.Core.Domain.Catalog;
using Appulu.Core.Domain.Common;
using Appulu.Core.Domain.Users;
using Appulu.Core.Domain.Orders;
using Appulu.Core.Domain.Shipping;
using Appulu.Core.Domain.Tax;
using Appulu.Core.Domain.Vendors;
using Appulu.Services.Catalog;
using Appulu.Services.Directory;
using Appulu.Services.Helpers;
using Appulu.Services.Localization;
using Appulu.Services.Media;
using Appulu.Services.Orders;
using Appulu.Services.Payments;
using Appulu.Services.Seo;
using Appulu.Services.Shipping;
using Appulu.Services.Vendors;
using Appulu.Web.Models.Common;
using Appulu.Web.Models.Order;

namespace Appulu.Web.Factories
{
    /// <summary>
    /// Represents the order model factory
    /// </summary>
    public partial class OrderModelFactory : IOrderModelFactory
    {
        #region Fields

        private readonly AddressSettings _addressSettings;
        private readonly CatalogSettings _catalogSettings;
        private readonly IAddressModelFactory _addressModelFactory;
        private readonly ICountryService _countryService;
        private readonly ICurrencyService _currencyService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IDownloadService _downloadService;
        private readonly ILocalizationService _localizationService;
        private readonly IOrderProcessingService _orderProcessingService;
        private readonly IOrderService _orderService;
        private readonly IOrderTotalCalculationService _orderTotalCalculationService;
        private readonly IPaymentService _paymentService;
        private readonly IPriceFormatter _priceFormatter;
        private readonly IProductService _productService;
        private readonly IRewardPointService _rewardPointService;
        private readonly IShipmentService _shipmentService;
        private readonly IStoreContext _storeContext;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IVendorService _vendorService;
        private readonly IWorkContext _workContext;
        private readonly OrderSettings _orderSettings;
        private readonly PdfSettings _pdfSettings;
        private readonly RewardPointsSettings _rewardPointsSettings;
        private readonly ShippingSettings _shippingSettings;
        private readonly TaxSettings _taxSettings;
        private readonly VendorSettings _vendorSettings;

        #endregion

        #region Ctor

        public OrderModelFactory(AddressSettings addressSettings,
            CatalogSettings catalogSettings,
            IAddressModelFactory addressModelFactory,
            ICountryService countryService,
            ICurrencyService currencyService,
            IDateTimeHelper dateTimeHelper,
            IDownloadService downloadService,
            ILocalizationService localizationService,
            IOrderProcessingService orderProcessingService,
            IOrderService orderService,
            IOrderTotalCalculationService orderTotalCalculationService,
            IPaymentService paymentService,
            IPriceFormatter priceFormatter,
            IProductService productService,
            IRewardPointService rewardPointService,
            IShipmentService shipmentService,
            IStoreContext storeContext,
            IUrlRecordService urlRecordService,
            IVendorService vendorService,
            IWorkContext workContext,
            OrderSettings orderSettings,
            PdfSettings pdfSettings,
            RewardPointsSettings rewardPointsSettings,
            ShippingSettings shippingSettings,
            TaxSettings taxSettings,
            VendorSettings vendorSettings)
        {
            this._addressSettings = addressSettings;
            this._catalogSettings = catalogSettings;
            this._addressModelFactory = addressModelFactory;
            this._countryService = countryService;
            this._currencyService = currencyService;
            this._dateTimeHelper = dateTimeHelper;
            this._downloadService = downloadService;
            this._localizationService = localizationService;
            this._orderProcessingService = orderProcessingService;
            this._orderService = orderService;
            this._orderTotalCalculationService = orderTotalCalculationService;
            this._paymentService = paymentService;
            this._priceFormatter = priceFormatter;
            this._productService = productService;
            this._rewardPointService = rewardPointService;
            this._shipmentService = shipmentService;
            this._storeContext = storeContext;
            this._urlRecordService = urlRecordService;
            this._vendorService = vendorService;
            this._workContext = workContext;
            this._orderSettings = orderSettings;
            this._pdfSettings = pdfSettings;
            this._rewardPointsSettings = rewardPointsSettings;
            this._shippingSettings = shippingSettings;
            this._taxSettings = taxSettings;
            this._vendorSettings = vendorSettings;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare the user order list model
        /// </summary>
        /// <returns>User order list model</returns>
        public virtual UserOrderListModel PrepareUserOrderListModel()
        {
            var model = new UserOrderListModel();
            var orders = _orderService.SearchOrders(storeId: _storeContext.CurrentStore.Id,
                userId: _workContext.CurrentUser.Id);
            foreach (var order in orders)
            {
                var orderModel = new UserOrderListModel.OrderDetailsModel
                {
                    Id = order.Id,
                    CreatedOn = _dateTimeHelper.ConvertToUserTime(order.CreatedOnUtc, DateTimeKind.Utc),
                    OrderStatusEnum = order.OrderStatus,
                    OrderStatus = _localizationService.GetLocalizedEnum(order.OrderStatus),
                    PaymentStatus = _localizationService.GetLocalizedEnum(order.PaymentStatus),
                    ShippingStatus = _localizationService.GetLocalizedEnum(order.ShippingStatus),
                    IsReturnRequestAllowed = _orderProcessingService.IsReturnRequestAllowed(order),
                    CustomOrderNumber = order.CustomOrderNumber
                };
                var orderTotalInUserCurrency = _currencyService.ConvertCurrency(order.OrderTotal, order.CurrencyRate);
                orderModel.OrderTotal = _priceFormatter.FormatPrice(orderTotalInUserCurrency, true, order.UserCurrencyCode, false, _workContext.WorkingLanguage);

                model.Orders.Add(orderModel);
            }

            var recurringPayments = _orderService.SearchRecurringPayments(_storeContext.CurrentStore.Id,
                _workContext.CurrentUser.Id);
            foreach (var recurringPayment in recurringPayments)
            {
                var recurringPaymentModel = new UserOrderListModel.RecurringOrderModel
                {
                    Id = recurringPayment.Id,
                    StartDate = _dateTimeHelper.ConvertToUserTime(recurringPayment.StartDateUtc, DateTimeKind.Utc).ToString(),
                    CycleInfo = $"{recurringPayment.CycleLength} {_localizationService.GetLocalizedEnum(recurringPayment.CyclePeriod)}",
                    NextPayment = recurringPayment.NextPaymentDate.HasValue ? _dateTimeHelper.ConvertToUserTime(recurringPayment.NextPaymentDate.Value, DateTimeKind.Utc).ToString() : "",
                    TotalCycles = recurringPayment.TotalCycles,
                    CyclesRemaining = recurringPayment.CyclesRemaining,
                    InitialOrderId = recurringPayment.InitialOrder.Id,
                    InitialOrderNumber = recurringPayment.InitialOrder.CustomOrderNumber,
                    CanCancel = _orderProcessingService.CanCancelRecurringPayment(_workContext.CurrentUser, recurringPayment),
                    CanRetryLastPayment = _orderProcessingService.CanRetryLastRecurringPayment(_workContext.CurrentUser, recurringPayment)
                };

                model.RecurringOrders.Add(recurringPaymentModel);
            }

            return model;
        }

        /// <summary>
        /// Prepare the order details model
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>Order details model</returns>
        public virtual OrderDetailsModel PrepareOrderDetailsModel(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));
            var model = new OrderDetailsModel
            {
                Id = order.Id,
                CreatedOn = _dateTimeHelper.ConvertToUserTime(order.CreatedOnUtc, DateTimeKind.Utc),
                OrderStatus = _localizationService.GetLocalizedEnum(order.OrderStatus),
                IsReOrderAllowed = _orderSettings.IsReOrderAllowed,
                IsReturnRequestAllowed = _orderProcessingService.IsReturnRequestAllowed(order),
                PdfInvoiceDisabled = _pdfSettings.DisablePdfInvoicesForPendingOrders && order.OrderStatus == OrderStatus.Pending,
                CustomOrderNumber = order.CustomOrderNumber,

                //shipping info
                ShippingStatus = _localizationService.GetLocalizedEnum(order.ShippingStatus)
            };
            if (order.ShippingStatus != ShippingStatus.ShippingNotRequired)
            {
                model.IsShippable = true;
                model.PickUpInStore = order.PickUpInStore;
                if (!order.PickUpInStore)
                {
                    _addressModelFactory.PrepareAddressModel(model.ShippingAddress,
                        address: order.ShippingAddress,
                        excludeProperties: false,
                        addressSettings: _addressSettings);
                }
                else
                    if (order.PickupAddress != null)
                    model.PickupAddress = new AddressModel
                    {
                        Address1 = order.PickupAddress.Address1,
                        City = order.PickupAddress.City,
                        County = order.PickupAddress.County,
                        CountryName = order.PickupAddress.Country != null ? order.PickupAddress.Country.Name : string.Empty,
                        ZipPostalCode = order.PickupAddress.ZipPostalCode
                    };
                model.ShippingMethod = order.ShippingMethod;

                //shipments (only already shipped)
                var shipments = order.Shipments.Where(x => x.ShippedDateUtc.HasValue).OrderBy(x => x.CreatedOnUtc).ToList();
                foreach (var shipment in shipments)
                {
                    var shipmentModel = new OrderDetailsModel.ShipmentBriefModel
                    {
                        Id = shipment.Id,
                        TrackingNumber = shipment.TrackingNumber,
                    };
                    if (shipment.ShippedDateUtc.HasValue)
                        shipmentModel.ShippedDate = _dateTimeHelper.ConvertToUserTime(shipment.ShippedDateUtc.Value, DateTimeKind.Utc);
                    if (shipment.DeliveryDateUtc.HasValue)
                        shipmentModel.DeliveryDate = _dateTimeHelper.ConvertToUserTime(shipment.DeliveryDateUtc.Value, DateTimeKind.Utc);
                    model.Shipments.Add(shipmentModel);
                }
            }

            //billing info
            _addressModelFactory.PrepareAddressModel(model.BillingAddress,
                address: order.BillingAddress,
                excludeProperties: false,
                addressSettings: _addressSettings);

            //VAT number
            model.VatNumber = order.VatNumber;

            //payment method
            var paymentMethod = _paymentService.LoadPaymentMethodBySystemName(order.PaymentMethodSystemName);
            model.PaymentMethod = paymentMethod != null ? _localizationService.GetLocalizedFriendlyName(paymentMethod, _workContext.WorkingLanguage.Id) : order.PaymentMethodSystemName;
            model.PaymentMethodStatus = _localizationService.GetLocalizedEnum(order.PaymentStatus);
            model.CanRePostProcessPayment = _paymentService.CanRePostProcessPayment(order);
            //custom values
            model.CustomValues = _paymentService.DeserializeCustomValues(order);

            //order subtotal
            if (order.UserTaxDisplayType == TaxDisplayType.IncludingTax && !_taxSettings.ForceTaxExclusionFromOrderSubtotal)
            {
                //including tax

                //order subtotal
                var orderSubtotalInclTaxInUserCurrency = _currencyService.ConvertCurrency(order.OrderSubtotalInclTax, order.CurrencyRate);
                model.OrderSubtotal = _priceFormatter.FormatPrice(orderSubtotalInclTaxInUserCurrency, true, order.UserCurrencyCode, _workContext.WorkingLanguage, true);
                //discount (applied to order subtotal)
                var orderSubTotalDiscountInclTaxInUserCurrency = _currencyService.ConvertCurrency(order.OrderSubTotalDiscountInclTax, order.CurrencyRate);
                if (orderSubTotalDiscountInclTaxInUserCurrency > decimal.Zero)
                    model.OrderSubTotalDiscount = _priceFormatter.FormatPrice(-orderSubTotalDiscountInclTaxInUserCurrency, true, order.UserCurrencyCode, _workContext.WorkingLanguage, true);
            }
            else
            {
                //excluding tax

                //order subtotal
                var orderSubtotalExclTaxInUserCurrency = _currencyService.ConvertCurrency(order.OrderSubtotalExclTax, order.CurrencyRate);
                model.OrderSubtotal = _priceFormatter.FormatPrice(orderSubtotalExclTaxInUserCurrency, true, order.UserCurrencyCode, _workContext.WorkingLanguage, false);
                //discount (applied to order subtotal)
                var orderSubTotalDiscountExclTaxInUserCurrency = _currencyService.ConvertCurrency(order.OrderSubTotalDiscountExclTax, order.CurrencyRate);
                if (orderSubTotalDiscountExclTaxInUserCurrency > decimal.Zero)
                    model.OrderSubTotalDiscount = _priceFormatter.FormatPrice(-orderSubTotalDiscountExclTaxInUserCurrency, true, order.UserCurrencyCode, _workContext.WorkingLanguage, false);
            }

            if (order.UserTaxDisplayType == TaxDisplayType.IncludingTax)
            {
                //including tax

                //order shipping
                var orderShippingInclTaxInUserCurrency = _currencyService.ConvertCurrency(order.OrderShippingInclTax, order.CurrencyRate);
                model.OrderShipping = _priceFormatter.FormatShippingPrice(orderShippingInclTaxInUserCurrency, true, order.UserCurrencyCode, _workContext.WorkingLanguage, true);
                //payment method additional fee
                var paymentMethodAdditionalFeeInclTaxInUserCurrency = _currencyService.ConvertCurrency(order.PaymentMethodAdditionalFeeInclTax, order.CurrencyRate);
                if (paymentMethodAdditionalFeeInclTaxInUserCurrency > decimal.Zero)
                    model.PaymentMethodAdditionalFee = _priceFormatter.FormatPaymentMethodAdditionalFee(paymentMethodAdditionalFeeInclTaxInUserCurrency, true, order.UserCurrencyCode, _workContext.WorkingLanguage, true);
            }
            else
            {
                //excluding tax

                //order shipping
                var orderShippingExclTaxInUserCurrency = _currencyService.ConvertCurrency(order.OrderShippingExclTax, order.CurrencyRate);
                model.OrderShipping = _priceFormatter.FormatShippingPrice(orderShippingExclTaxInUserCurrency, true, order.UserCurrencyCode, _workContext.WorkingLanguage, false);
                //payment method additional fee
                var paymentMethodAdditionalFeeExclTaxInUserCurrency = _currencyService.ConvertCurrency(order.PaymentMethodAdditionalFeeExclTax, order.CurrencyRate);
                if (paymentMethodAdditionalFeeExclTaxInUserCurrency > decimal.Zero)
                    model.PaymentMethodAdditionalFee = _priceFormatter.FormatPaymentMethodAdditionalFee(paymentMethodAdditionalFeeExclTaxInUserCurrency, true, order.UserCurrencyCode, _workContext.WorkingLanguage, false);
            }

            //tax
            var displayTax = true;
            var displayTaxRates = true;
            if (_taxSettings.HideTaxInOrderSummary && order.UserTaxDisplayType == TaxDisplayType.IncludingTax)
            {
                displayTax = false;
                displayTaxRates = false;
            }
            else
            {
                if (order.OrderTax == 0 && _taxSettings.HideZeroTax)
                {
                    displayTax = false;
                    displayTaxRates = false;
                }
                else
                {
                    var taxRates = _orderService.ParseTaxRates(order, order.TaxRates);
                    displayTaxRates = _taxSettings.DisplayTaxRates && taxRates.Any();
                    displayTax = !displayTaxRates;

                    var orderTaxInUserCurrency = _currencyService.ConvertCurrency(order.OrderTax, order.CurrencyRate);
                    //TODO pass languageId to _priceFormatter.FormatPrice
                    model.Tax = _priceFormatter.FormatPrice(orderTaxInUserCurrency, true, order.UserCurrencyCode, false, _workContext.WorkingLanguage);

                    foreach (var tr in taxRates)
                    {
                        model.TaxRates.Add(new OrderDetailsModel.TaxRate
                        {
                            Rate = _priceFormatter.FormatTaxRate(tr.Key),
                            //TODO pass languageId to _priceFormatter.FormatPrice
                            Value = _priceFormatter.FormatPrice(_currencyService.ConvertCurrency(tr.Value, order.CurrencyRate), true, order.UserCurrencyCode, false, _workContext.WorkingLanguage),
                        });
                    }
                }
            }
            model.DisplayTaxRates = displayTaxRates;
            model.DisplayTax = displayTax;
            model.DisplayTaxShippingInfo = _catalogSettings.DisplayTaxShippingInfoOrderDetailsPage;
            model.PricesIncludeTax = order.UserTaxDisplayType == TaxDisplayType.IncludingTax;

            //discount (applied to order total)
            var orderDiscountInUserCurrency = _currencyService.ConvertCurrency(order.OrderDiscount, order.CurrencyRate);
            if (orderDiscountInUserCurrency > decimal.Zero)
                model.OrderTotalDiscount = _priceFormatter.FormatPrice(-orderDiscountInUserCurrency, true, order.UserCurrencyCode, false, _workContext.WorkingLanguage);

            //gift cards
            foreach (var gcuh in order.GiftCardUsageHistory)
            {
                model.GiftCards.Add(new OrderDetailsModel.GiftCard
                {
                    CouponCode = gcuh.GiftCard.GiftCardCouponCode,
                    Amount = _priceFormatter.FormatPrice(-(_currencyService.ConvertCurrency(gcuh.UsedValue, order.CurrencyRate)), true, order.UserCurrencyCode, false, _workContext.WorkingLanguage),
                });
            }

            //reward points           
            if (order.RedeemedRewardPointsEntry != null)
            {
                model.RedeemedRewardPoints = -order.RedeemedRewardPointsEntry.Points;
                model.RedeemedRewardPointsAmount = _priceFormatter.FormatPrice(-(_currencyService.ConvertCurrency(order.RedeemedRewardPointsEntry.UsedAmount, order.CurrencyRate)), true, order.UserCurrencyCode, false, _workContext.WorkingLanguage);
            }

            //total
            var orderTotalInUserCurrency = _currencyService.ConvertCurrency(order.OrderTotal, order.CurrencyRate);
            model.OrderTotal = _priceFormatter.FormatPrice(orderTotalInUserCurrency, true, order.UserCurrencyCode, false, _workContext.WorkingLanguage);

            //checkout attributes
            model.CheckoutAttributeInfo = order.CheckoutAttributeDescription;

            //order notes
            foreach (var orderNote in order.OrderNotes
                .Where(on => on.DisplayToUser)
                .OrderByDescending(on => on.CreatedOnUtc)
                .ToList())
            {
                model.OrderNotes.Add(new OrderDetailsModel.OrderNote
                {
                    Id = orderNote.Id,
                    HasDownload = orderNote.DownloadId > 0,
                    Note = _orderService.FormatOrderNoteText(orderNote),
                    CreatedOn = _dateTimeHelper.ConvertToUserTime(orderNote.CreatedOnUtc, DateTimeKind.Utc)
                });
            }

            //purchased products
            model.ShowSku = _catalogSettings.ShowSkuOnProductDetailsPage;
            model.ShowVendorName = _vendorSettings.ShowVendorOnOrderDetailsPage;

            var orderItems = order.OrderItems;

            var vendors = _vendorSettings.ShowVendorOnOrderDetailsPage ? _vendorService.GetVendorsByIds(orderItems.Select(item => item.Product.VendorId).ToArray()) : new List<Vendor>();

            foreach (var orderItem in orderItems)
            {
                var orderItemModel = new OrderDetailsModel.OrderItemModel
                {
                    Id = orderItem.Id,
                    OrderItemGuid = orderItem.OrderItemGuid,
                    Sku = _productService.FormatSku(orderItem.Product, orderItem.AttributesXml),
                    VendorName = vendors.FirstOrDefault(v => v.Id == orderItem.Product.VendorId)?.Name ?? string.Empty,
                    ProductId = orderItem.Product.Id,
                    ProductName = _localizationService.GetLocalized(orderItem.Product, x => x.Name),
                    ProductSeName = _urlRecordService.GetSeName(orderItem.Product),
                    Quantity = orderItem.Quantity,
                    AttributeInfo = orderItem.AttributeDescription,
                };
                //rental info
                if (orderItem.Product.IsRental)
                {
                    var rentalStartDate = orderItem.RentalStartDateUtc.HasValue
                        ? _productService.FormatRentalDate(orderItem.Product, orderItem.RentalStartDateUtc.Value) : "";
                    var rentalEndDate = orderItem.RentalEndDateUtc.HasValue
                        ? _productService.FormatRentalDate(orderItem.Product, orderItem.RentalEndDateUtc.Value) : "";
                    orderItemModel.RentalInfo = string.Format(_localizationService.GetResource("Order.Rental.FormattedDate"),
                        rentalStartDate, rentalEndDate);
                }
                model.Items.Add(orderItemModel);

                //unit price, subtotal
                if (order.UserTaxDisplayType == TaxDisplayType.IncludingTax)
                {
                    //including tax
                    var unitPriceInclTaxInUserCurrency = _currencyService.ConvertCurrency(orderItem.UnitPriceInclTax, order.CurrencyRate);
                    orderItemModel.UnitPrice = _priceFormatter.FormatPrice(unitPriceInclTaxInUserCurrency, true, order.UserCurrencyCode, _workContext.WorkingLanguage, true);

                    var priceInclTaxInUserCurrency = _currencyService.ConvertCurrency(orderItem.PriceInclTax, order.CurrencyRate);
                    orderItemModel.SubTotal = _priceFormatter.FormatPrice(priceInclTaxInUserCurrency, true, order.UserCurrencyCode, _workContext.WorkingLanguage, true);
                }
                else
                {
                    //excluding tax
                    var unitPriceExclTaxInUserCurrency = _currencyService.ConvertCurrency(orderItem.UnitPriceExclTax, order.CurrencyRate);
                    orderItemModel.UnitPrice = _priceFormatter.FormatPrice(unitPriceExclTaxInUserCurrency, true, order.UserCurrencyCode, _workContext.WorkingLanguage, false);

                    var priceExclTaxInUserCurrency = _currencyService.ConvertCurrency(orderItem.PriceExclTax, order.CurrencyRate);
                    orderItemModel.SubTotal = _priceFormatter.FormatPrice(priceExclTaxInUserCurrency, true, order.UserCurrencyCode, _workContext.WorkingLanguage, false);
                }

                //downloadable products
                if (_downloadService.IsDownloadAllowed(orderItem))
                    orderItemModel.DownloadId = orderItem.Product.DownloadId;
                if (_downloadService.IsLicenseDownloadAllowed(orderItem))
                    orderItemModel.LicenseId = orderItem.LicenseDownloadId.HasValue ? orderItem.LicenseDownloadId.Value : 0;
            }

            return model;
        }

        /// <summary>
        /// Prepare the shipment details model
        /// </summary>
        /// <param name="shipment">Shipment</param>
        /// <returns>Shipment details model</returns>
        public virtual ShipmentDetailsModel PrepareShipmentDetailsModel(Shipment shipment)
        {
            if (shipment == null)
                throw new ArgumentNullException(nameof(shipment));

            var order = shipment.Order;
            if (order == null)
                throw new Exception("order cannot be loaded");
            var model = new ShipmentDetailsModel
            {
                Id = shipment.Id
            };
            if (shipment.ShippedDateUtc.HasValue)
                model.ShippedDate = _dateTimeHelper.ConvertToUserTime(shipment.ShippedDateUtc.Value, DateTimeKind.Utc);
            if (shipment.DeliveryDateUtc.HasValue)
                model.DeliveryDate = _dateTimeHelper.ConvertToUserTime(shipment.DeliveryDateUtc.Value, DateTimeKind.Utc);

            //tracking number and shipment information
            if (!string.IsNullOrEmpty(shipment.TrackingNumber))
            {
                model.TrackingNumber = shipment.TrackingNumber;
                var shipmentTracker = _shipmentService.GetShipmentTracker(shipment);
                if (shipmentTracker != null)
                {
                    model.TrackingNumberUrl = shipmentTracker.GetUrl(shipment.TrackingNumber);
                    if (_shippingSettings.DisplayShipmentEventsToUsers)
                    {
                        var shipmentEvents = shipmentTracker.GetShipmentEvents(shipment.TrackingNumber);
                        if (shipmentEvents != null)
                            foreach (var shipmentEvent in shipmentEvents)
                            {
                                var shipmentStatusEventModel = new ShipmentDetailsModel.ShipmentStatusEventModel();
                                var shipmentEventCountry = _countryService.GetCountryByTwoLetterIsoCode(shipmentEvent.CountryCode);
                                shipmentStatusEventModel.Country = shipmentEventCountry != null 
                                    ? _localizationService.GetLocalized(shipmentEventCountry, x => x.Name) : shipmentEvent.CountryCode;
                                shipmentStatusEventModel.Date = shipmentEvent.Date;
                                shipmentStatusEventModel.EventName = shipmentEvent.EventName;
                                shipmentStatusEventModel.Location = shipmentEvent.Location;
                                model.ShipmentStatusEvents.Add(shipmentStatusEventModel);
                            }
                    }
                }
            }

            //products in this shipment
            model.ShowSku = _catalogSettings.ShowSkuOnProductDetailsPage;
            foreach (var shipmentItem in shipment.ShipmentItems)
            {
                var orderItem = _orderService.GetOrderItemById(shipmentItem.OrderItemId);
                if (orderItem == null)
                    continue;

                var shipmentItemModel = new ShipmentDetailsModel.ShipmentItemModel
                {
                    Id = shipmentItem.Id,
                    Sku = _productService.FormatSku(orderItem.Product, orderItem.AttributesXml),
                    ProductId = orderItem.Product.Id,
                    ProductName = _localizationService.GetLocalized(orderItem.Product, x => x.Name),
                    ProductSeName = _urlRecordService.GetSeName(orderItem.Product),
                    AttributeInfo = orderItem.AttributeDescription,
                    QuantityOrdered = orderItem.Quantity,
                    QuantityShipped = shipmentItem.Quantity,
                };
                //rental info
                if (orderItem.Product.IsRental)
                {
                    var rentalStartDate = orderItem.RentalStartDateUtc.HasValue
                        ? _productService.FormatRentalDate(orderItem.Product, orderItem.RentalStartDateUtc.Value) : "";
                    var rentalEndDate = orderItem.RentalEndDateUtc.HasValue
                        ? _productService.FormatRentalDate(orderItem.Product, orderItem.RentalEndDateUtc.Value) : "";
                    shipmentItemModel.RentalInfo = string.Format(_localizationService.GetResource("Order.Rental.FormattedDate"),
                        rentalStartDate, rentalEndDate);
                }
                model.Items.Add(shipmentItemModel);
            }

            //order details model
            model.Order = PrepareOrderDetailsModel(order);

            return model;
        }

        /// <summary>
        /// Prepare the user reward points model
        /// </summary>
        /// <param name="page">Number of items page; pass null to load the first page</param>
        /// <returns>User reward points model</returns>
        public virtual UserRewardPointsModel PrepareUserRewardPoints(int? page)
        {
            //get reward points history
            var user = _workContext.CurrentUser;
            var store = _storeContext.CurrentStore;
            var pageSize = _rewardPointsSettings.PageSize;
            var rewardPoints = _rewardPointService.GetRewardPointsHistory(user.Id, store.Id, true, pageIndex: --page ?? 0, pageSize: pageSize);

            //prepare model
            var model = new UserRewardPointsModel();
            model.RewardPoints = rewardPoints.Select(historyEntry =>
            {
                var activatingDate = _dateTimeHelper.ConvertToUserTime(historyEntry.CreatedOnUtc, DateTimeKind.Utc);
                return new UserRewardPointsModel.RewardPointsHistoryModel
                {
                    Points = historyEntry.Points,
                    PointsBalance = historyEntry.PointsBalance.HasValue ? historyEntry.PointsBalance.ToString()
                        : string.Format(_localizationService.GetResource("RewardPoints.ActivatedLater"), activatingDate),
                    Message = historyEntry.Message,
                    CreatedOn = activatingDate,
                    EndDate = !historyEntry.EndDateUtc.HasValue ? null :
                        (DateTime?)_dateTimeHelper.ConvertToUserTime(historyEntry.EndDateUtc.Value, DateTimeKind.Utc)
                };
            }).ToList();

            model.PagerModel = new PagerModel
            {
                PageSize = rewardPoints.PageSize,
                TotalRecords = rewardPoints.TotalCount,
                PageIndex = rewardPoints.PageIndex,
                ShowTotalSummary = true,
                RouteActionName = "UserRewardPointsPaged",
                UseRouteLinks = true,
                RouteValues = new RewardPointsRouteValues { pageNumber = page ?? 0 }
            };

            //current amount/balance
            var rewardPointsBalance = _rewardPointService.GetRewardPointsBalance(user.Id, _storeContext.CurrentStore.Id);
            var rewardPointsAmountBase = _orderTotalCalculationService.ConvertRewardPointsToAmount(rewardPointsBalance);
            var rewardPointsAmount = _currencyService.ConvertFromPrimaryStoreCurrency(rewardPointsAmountBase, _workContext.WorkingCurrency);
            model.RewardPointsBalance = rewardPointsBalance;
            model.RewardPointsAmount = _priceFormatter.FormatPrice(rewardPointsAmount, true, false);

            //minimum amount/balance
            var minimumRewardPointsBalance = _rewardPointsSettings.MinimumRewardPointsToUse;
            var minimumRewardPointsAmountBase = _orderTotalCalculationService.ConvertRewardPointsToAmount(minimumRewardPointsBalance);
            var minimumRewardPointsAmount = _currencyService.ConvertFromPrimaryStoreCurrency(minimumRewardPointsAmountBase, _workContext.WorkingCurrency);
            model.MinimumRewardPointsBalance = minimumRewardPointsBalance;
            model.MinimumRewardPointsAmount = _priceFormatter.FormatPrice(minimumRewardPointsAmount, true, false);

            return model;
        }

        #endregion
    }
}