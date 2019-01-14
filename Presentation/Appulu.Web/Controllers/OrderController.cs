using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Appulu.Core;
using Appulu.Core.Domain.Users;
using Appulu.Core.Domain.Orders;
using Appulu.Services.Common;
using Appulu.Services.Orders;
using Appulu.Services.Payments;
using Appulu.Services.Shipping;
using Appulu.Web.Factories;
using Appulu.Web.Framework.Controllers;
using Appulu.Web.Framework.Mvc.Filters;
using Appulu.Web.Framework.Security;

namespace Appulu.Web.Controllers
{
    public partial class OrderController : BasePublicController
    {
        #region Fields

        private readonly IOrderModelFactory _orderModelFactory;
        private readonly IOrderProcessingService _orderProcessingService;
        private readonly IOrderService _orderService;
        private readonly IPaymentService _paymentService;
        private readonly IPdfService _pdfService;
        private readonly IShipmentService _shipmentService;
        private readonly IWebHelper _webHelper;
        private readonly IWorkContext _workContext;
        private readonly RewardPointsSettings _rewardPointsSettings;

        #endregion

		#region Ctor

        public OrderController(IOrderModelFactory orderModelFactory,
            IOrderProcessingService orderProcessingService, 
            IOrderService orderService, 
            IPaymentService paymentService, 
            IPdfService pdfService,
            IShipmentService shipmentService, 
            IWebHelper webHelper,
            IWorkContext workContext,
            RewardPointsSettings rewardPointsSettings)
        {
            this._orderModelFactory = orderModelFactory;
            this._orderProcessingService = orderProcessingService;
            this._orderService = orderService;
            this._paymentService = paymentService;
            this._pdfService = pdfService;
            this._shipmentService = shipmentService;
            this._webHelper = webHelper;
            this._workContext = workContext;
            this._rewardPointsSettings = rewardPointsSettings;
        }

        #endregion

        #region Methods

        //My account / Orders
        [HttpsRequirement(SslRequirement.Yes)]
        public virtual IActionResult UserOrders()
        {
            if (!_workContext.CurrentUser.IsRegistered())
                return Challenge();

            var model = _orderModelFactory.PrepareUserOrderListModel();
            return View(model);
        }

        //My account / Orders / Cancel recurring order
        [HttpPost, ActionName("UserOrders")]
        [PublicAntiForgery]
        [FormValueRequired(FormValueRequirement.StartsWith, "cancelRecurringPayment")]
        public virtual IActionResult CancelRecurringPayment(IFormCollection form)
        {
            if (!_workContext.CurrentUser.IsRegistered())
                return Challenge();

            //get recurring payment identifier
            var recurringPaymentId = 0;
            foreach (var formValue in form.Keys)
                if (formValue.StartsWith("cancelRecurringPayment", StringComparison.InvariantCultureIgnoreCase))
                    recurringPaymentId = Convert.ToInt32(formValue.Substring("cancelRecurringPayment".Length));

            var recurringPayment = _orderService.GetRecurringPaymentById(recurringPaymentId);
            if (recurringPayment == null)
            {
                return RedirectToRoute("UserOrders");
            }

            if (_orderProcessingService.CanCancelRecurringPayment(_workContext.CurrentUser, recurringPayment))
            {
                var errors = _orderProcessingService.CancelRecurringPayment(recurringPayment);

                var model = _orderModelFactory.PrepareUserOrderListModel();
                model.RecurringPaymentErrors = errors;

                return View(model);
            }

            return RedirectToRoute("UserOrders");
        }

        //My account / Orders / Retry last recurring order
        [HttpPost, ActionName("UserOrders")]
        [PublicAntiForgery]
        [FormValueRequired(FormValueRequirement.StartsWith, "retryLastPayment")]
        public virtual IActionResult RetryLastRecurringPayment(IFormCollection form)
        {
            if (!_workContext.CurrentUser.IsRegistered())
                return Challenge();

            //get recurring payment identifier
            var recurringPaymentId = 0;
            if (!form.Keys.Any(formValue => formValue.StartsWith("retryLastPayment", StringComparison.InvariantCultureIgnoreCase) &&
                int.TryParse(formValue.Substring(formValue.IndexOf('_') + 1), out recurringPaymentId)))
            {
                return RedirectToRoute("UserOrders");
            }

            var recurringPayment = _orderService.GetRecurringPaymentById(recurringPaymentId);
            if (recurringPayment == null)
                return RedirectToRoute("UserOrders");

            if (!_orderProcessingService.CanRetryLastRecurringPayment(_workContext.CurrentUser, recurringPayment))
                return RedirectToRoute("UserOrders");

            var errors = _orderProcessingService.ProcessNextRecurringPayment(recurringPayment);
            var model = _orderModelFactory.PrepareUserOrderListModel();
            model.RecurringPaymentErrors = errors.ToList();

            return View(model);
        }

        //My account / Reward points
        [HttpsRequirement(SslRequirement.Yes)]
        public virtual IActionResult UserRewardPoints(int? pageNumber)
        {
            if (!_workContext.CurrentUser.IsRegistered())
                return Challenge();

            if (!_rewardPointsSettings.Enabled)
                return RedirectToRoute("UserInfo");

            var model = _orderModelFactory.PrepareUserRewardPoints(pageNumber);
            return View(model);
        }

        //My account / Order details page
        [HttpsRequirement(SslRequirement.Yes)]
        public virtual IActionResult Details(int orderId)
        {
            var order = _orderService.GetOrderById(orderId);
            if (order == null || order.Deleted || _workContext.CurrentUser.Id != order.UserId)
                return Challenge();

            var model = _orderModelFactory.PrepareOrderDetailsModel(order);
            return View(model);
        }

        //My account / Order details page / Print
        [HttpsRequirement(SslRequirement.Yes)]
        public virtual IActionResult PrintOrderDetails(int orderId)
        {
            var order = _orderService.GetOrderById(orderId);
            if (order == null || order.Deleted || _workContext.CurrentUser.Id != order.UserId)
                return Challenge();

            var model = _orderModelFactory.PrepareOrderDetailsModel(order);
            model.PrintMode = true;

            return View("Details", model);
        }

        //My account / Order details page / PDF invoice
        public virtual IActionResult GetPdfInvoice(int orderId)
        {
            var order = _orderService.GetOrderById(orderId);
            if (order == null || order.Deleted || _workContext.CurrentUser.Id != order.UserId)
                return Challenge();

            var orders = new List<Order>();
            orders.Add(order);
            byte[] bytes;
            using (var stream = new MemoryStream())
            {
                _pdfService.PrintOrdersToPdf(stream, orders, _workContext.WorkingLanguage.Id);
                bytes = stream.ToArray();
            }
            return File(bytes, MimeTypes.ApplicationPdf, $"order_{order.Id}.pdf");
        }

        //My account / Order details page / re-order
        public virtual IActionResult ReOrder(int orderId)
        {
            var order = _orderService.GetOrderById(orderId);
            if (order == null || order.Deleted || _workContext.CurrentUser.Id != order.UserId)
                return Challenge();

            _orderProcessingService.ReOrder(order);
            return RedirectToRoute("ShoppingCart");
        }

        //My account / Order details page / Complete payment
        [HttpPost, ActionName("Details")]
        [PublicAntiForgery]
        [FormValueRequired("repost-payment")]
        public virtual IActionResult RePostPayment(int orderId)
        {
            var order = _orderService.GetOrderById(orderId);
            if (order == null || order.Deleted || _workContext.CurrentUser.Id != order.UserId)
                return Challenge();

            if (!_paymentService.CanRePostProcessPayment(order))
                return RedirectToRoute("OrderDetails", new { orderId = orderId });

            var postProcessPaymentRequest = new PostProcessPaymentRequest
            {
                Order = order
            };
            _paymentService.PostProcessPayment(postProcessPaymentRequest);

            if (_webHelper.IsRequestBeingRedirected || _webHelper.IsPostBeingDone)
            {
                //redirection or POST has been done in PostProcessPayment
                return Content("Redirected");
            }

            //if no redirection has been done (to a third-party payment page)
            //theoretically it's not possible
            return RedirectToRoute("OrderDetails", new { orderId = orderId });
        }

        //My account / Order details page / Shipment details page
        [HttpsRequirement(SslRequirement.Yes)]
        public virtual IActionResult ShipmentDetails(int shipmentId)
        {
            var shipment = _shipmentService.GetShipmentById(shipmentId);
            if (shipment == null)
                return Challenge();

            var order = shipment.Order;
            if (order == null || order.Deleted || _workContext.CurrentUser.Id != order.UserId)
                return Challenge();

            var model = _orderModelFactory.PrepareShipmentDetailsModel(shipment);
            return View(model);
        }
        
        #endregion
    }
}