using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Appulu.Core;
using Appulu.Core.Domain.Users;
using Appulu.Core.Domain.Localization;
using Appulu.Core.Domain.Media;
using Appulu.Core.Domain.Orders;
using Appulu.Core.Infrastructure;
using Appulu.Services.Users;
using Appulu.Services.Localization;
using Appulu.Services.Media;
using Appulu.Services.Messages;
using Appulu.Services.Orders;
using Appulu.Web.Factories;
using Appulu.Web.Framework.Mvc.Filters;
using Appulu.Web.Framework.Security;
using Appulu.Web.Models.Order;

namespace Appulu.Web.Controllers
{
    public partial class ReturnRequestController : BasePublicController
    {
        #region Fields

        private readonly IUserService _userService;
        private readonly ICustomNumberFormatter _customNumberFormatter;
        private readonly IDownloadService _downloadService;
        private readonly ILocalizationService _localizationService;
        private readonly IAppFileProvider _fileProvider;
        private readonly IOrderProcessingService _orderProcessingService;
        private readonly IOrderService _orderService;
        private readonly IReturnRequestModelFactory _returnRequestModelFactory;
        private readonly IReturnRequestService _returnRequestService;
        private readonly IStoreContext _storeContext;
        private readonly IWorkContext _workContext;
        private readonly IWorkflowMessageService _workflowMessageService;
        private readonly LocalizationSettings _localizationSettings;
        private readonly OrderSettings _orderSettings;

        #endregion

        #region Ctor

        public ReturnRequestController(IUserService userService,
            ICustomNumberFormatter customNumberFormatter,
            IDownloadService downloadService,
            ILocalizationService localizationService,
            IAppFileProvider fileProvider,
            IOrderProcessingService orderProcessingService,
            IOrderService orderService,
            IReturnRequestModelFactory returnRequestModelFactory,
            IReturnRequestService returnRequestService,
            IStoreContext storeContext,
            IWorkContext workContext,
            IWorkflowMessageService workflowMessageService,
            LocalizationSettings localizationSettings,
            OrderSettings orderSettings)
        {
            this._userService = userService;
            this._customNumberFormatter = customNumberFormatter;
            this._downloadService = downloadService;
            this._localizationService = localizationService;
            this._fileProvider = fileProvider;
            this._orderProcessingService = orderProcessingService;
            this._orderService = orderService;
            this._returnRequestModelFactory = returnRequestModelFactory;
            this._returnRequestService = returnRequestService;
            this._storeContext = storeContext;
            this._workContext = workContext;
            this._workflowMessageService = workflowMessageService;
            this._localizationSettings = localizationSettings;
            this._orderSettings = orderSettings;
        }

        #endregion

        #region Methods

        [HttpsRequirement(SslRequirement.Yes)]
        public virtual IActionResult UserReturnRequests()
        {
            if (!_workContext.CurrentUser.IsRegistered())
                return Challenge();

            var model = _returnRequestModelFactory.PrepareUserReturnRequestsModel();
            return View(model);
        }

        [HttpsRequirement(SslRequirement.Yes)]
        public virtual IActionResult ReturnRequest(int orderId)
        {
            var order = _orderService.GetOrderById(orderId);
            if (order == null || order.Deleted || _workContext.CurrentUser.Id != order.UserId)
                return Challenge();

            if (!_orderProcessingService.IsReturnRequestAllowed(order))
                return RedirectToRoute("HomePage");

            var model = new SubmitReturnRequestModel();
            model = _returnRequestModelFactory.PrepareSubmitReturnRequestModel(model, order);
            return View(model);
        }

        [HttpPost, ActionName("ReturnRequest")]
        [PublicAntiForgery]
        public virtual IActionResult ReturnRequestSubmit(int orderId, SubmitReturnRequestModel model)
        {
            var order = _orderService.GetOrderById(orderId);
            if (order == null || order.Deleted || _workContext.CurrentUser.Id != order.UserId)
                return Challenge();

            if (!_orderProcessingService.IsReturnRequestAllowed(order))
                return RedirectToRoute("HomePage");

            var count = 0;

            var downloadId = 0;
            if (_orderSettings.ReturnRequestsAllowFiles)
            {
                var download = _downloadService.GetDownloadByGuid(model.UploadedFileGuid);
                if (download != null)
                    downloadId = download.Id;
            }

            //returnable products
            var form = model.Form;
            var orderItems = order.OrderItems.Where(oi => !oi.Product.NotReturnable);
            foreach (var orderItem in orderItems)
            {
                var quantity = 0; //parse quantity
                foreach (var formKey in form.Keys)
                    if (formKey.Equals($"quantity{orderItem.Id}", StringComparison.InvariantCultureIgnoreCase))
                    {
                        int.TryParse(form[formKey], out quantity);
                        break;
                    }
                if (quantity > 0)
                {
                    var rrr = _returnRequestService.GetReturnRequestReasonById(model.ReturnRequestReasonId);
                    var rra = _returnRequestService.GetReturnRequestActionById(model.ReturnRequestActionId);

                    var rr = new ReturnRequest
                    {
                        CustomNumber = "",
                        StoreId = _storeContext.CurrentStore.Id,
                        OrderItemId = orderItem.Id,
                        Quantity = quantity,
                        UserId = _workContext.CurrentUser.Id,
                        ReasonForReturn = rrr != null ? _localizationService.GetLocalized(rrr, x => x.Name) : "not available",
                        RequestedAction = rra != null ? _localizationService.GetLocalized(rra, x => x.Name) : "not available",
                        UserComments = model.Comments,
                        UploadedFileId = downloadId,
                        StaffNotes = string.Empty,
                        ReturnRequestStatus = ReturnRequestStatus.Pending,
                        CreatedOnUtc = DateTime.UtcNow,
                        UpdatedOnUtc = DateTime.UtcNow
                    };
                    _workContext.CurrentUser.ReturnRequests.Add(rr);
                    _userService.UpdateUser(_workContext.CurrentUser);
                    //set return request custom number
                    rr.CustomNumber = _customNumberFormatter.GenerateReturnRequestCustomNumber(rr);
                    _userService.UpdateUser(_workContext.CurrentUser);
                    //notify store owner
                    _workflowMessageService.SendNewReturnRequestStoreOwnerNotification(rr, orderItem, _localizationSettings.DefaultAdminLanguageId);
                    //notify user
                    _workflowMessageService.SendNewReturnRequestUserNotification(rr, orderItem, order.UserLanguageId);

                    count++;
                }
            }

            model = _returnRequestModelFactory.PrepareSubmitReturnRequestModel(model, order);
            if (count > 0)
                model.Result = _localizationService.GetResource("ReturnRequests.Submitted");
            else
                model.Result = _localizationService.GetResource("ReturnRequests.NoItemsSubmitted");

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult UploadFileReturnRequest()
        {
            if (!_orderSettings.ReturnRequestsEnabled || !_orderSettings.ReturnRequestsAllowFiles)
            {
                return Json(new
                {
                    success = false,
                    downloadGuid = Guid.Empty,
                });
            }

            var httpPostedFile = Request.Form.Files.FirstOrDefault();
            if (httpPostedFile == null)
            {
                return Json(new
                {
                    success = false,
                    message = "No file uploaded",
                    downloadGuid = Guid.Empty,
                });
            }

            var fileBinary = _downloadService.GetDownloadBits(httpPostedFile);

            var qqFileNameParameter = "qqfilename";
            var fileName = httpPostedFile.FileName;
            if (string.IsNullOrEmpty(fileName) && Request.Form.ContainsKey(qqFileNameParameter))
                fileName = Request.Form[qqFileNameParameter].ToString();
            //remove path (passed in IE)
            fileName = _fileProvider.GetFileName(fileName);

            var contentType = httpPostedFile.ContentType;

            var fileExtension = _fileProvider.GetFileExtension(fileName);
            if (!string.IsNullOrEmpty(fileExtension))
                fileExtension = fileExtension.ToLowerInvariant();

            var validationFileMaximumSize = _orderSettings.ReturnRequestsFileMaximumSize;
            if (validationFileMaximumSize > 0)
            {
                //compare in bytes
                var maxFileSizeBytes = validationFileMaximumSize * 1024;
                if (fileBinary.Length > maxFileSizeBytes)
                {
                    return Json(new
                    {
                        success = false,
                        message = string.Format(_localizationService.GetResource("ShoppingCart.MaximumUploadedFileSize"), validationFileMaximumSize),
                        downloadGuid = Guid.Empty,
                    });
                }
            }

            var download = new Download
            {
                DownloadGuid = Guid.NewGuid(),
                UseDownloadUrl = false,
                DownloadUrl = "",
                DownloadBinary = fileBinary,
                ContentType = contentType,
                //we store filename without extension for downloads
                Filename = _fileProvider.GetFileNameWithoutExtension(fileName),
                Extension = fileExtension,
                IsNew = true
            };
            _downloadService.InsertDownload(download);

            //when returning JSON the mime-type must be set to text/plain
            //otherwise some browsers will pop-up a "Save As" dialog.
            return Json(new
            {
                success = true,
                message = _localizationService.GetResource("ShoppingCart.FileUploaded"),
                downloadUrl = Url.Action("GetFileUpload", "Download", new { downloadId = download.DownloadGuid }),
                downloadGuid = download.DownloadGuid,
            });
        }

        #endregion
    }
}