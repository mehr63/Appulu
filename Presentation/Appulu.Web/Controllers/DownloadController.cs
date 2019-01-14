using System;
using Microsoft.AspNetCore.Mvc;
using Appulu.Core;
using Appulu.Core.Domain.Users;
using Appulu.Services.Catalog;
using Appulu.Services.Localization;
using Appulu.Services.Media;
using Appulu.Services.Orders;

namespace Appulu.Web.Controllers
{
    public partial class DownloadController : BasePublicController
    {
        private readonly UserSettings _userSettings;
        private readonly IDownloadService _downloadService;
        private readonly ILocalizationService _localizationService;
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        private readonly IWorkContext _workContext;

        public DownloadController(UserSettings userSettings,
            IDownloadService downloadService,
            ILocalizationService localizationService,
            IOrderService orderService,
            IProductService productService,
            IWorkContext workContext)
        {
            this._userSettings = userSettings;
            this._downloadService = downloadService;
            this._localizationService = localizationService;
            this._orderService = orderService;
            this._productService = productService;
            this._workContext = workContext;
        }
        
        public virtual IActionResult Sample(int productId)
        {
            var product = _productService.GetProductById(productId);
            if (product == null)
                return InvokeHttp404();

            if (!product.HasSampleDownload)
                return Content("Product doesn't have a sample download.");

            var download = _downloadService.GetDownloadById(product.SampleDownloadId);
            if (download == null)
                return Content("Sample download is not available any more.");

            if (download.UseDownloadUrl)
                return new RedirectResult(download.DownloadUrl);
            
            if (download.DownloadBinary == null)
                return Content("Download data is not available any more.");
            
            var fileName = !string.IsNullOrWhiteSpace(download.Filename) ? download.Filename : product.Id.ToString();
            var contentType = !string.IsNullOrWhiteSpace(download.ContentType) ? download.ContentType : MimeTypes.ApplicationOctetStream;
            return new FileContentResult(download.DownloadBinary, contentType) { FileDownloadName = fileName + download.Extension }; 
        }

        public virtual IActionResult GetDownload(Guid orderItemId, bool agree = false)
        {
            var orderItem = _orderService.GetOrderItemByGuid(orderItemId);
            if (orderItem == null)
                return InvokeHttp404();

            var order = orderItem.Order;
            var product = orderItem.Product;
            if (!_downloadService.IsDownloadAllowed(orderItem))
                return Content("Downloads are not allowed");

            if (_userSettings.DownloadableProductsValidateUser)
            {
                if (_workContext.CurrentUser == null)
                    return Challenge();

                if (order.UserId != _workContext.CurrentUser.Id)
                    return Content("This is not your order");
            }

            var download = _downloadService.GetDownloadById(product.DownloadId);
            if (download == null)
                return Content("Download is not available any more.");

            if (product.HasUserAgreement && !agree)
                return RedirectToRoute("DownloadUserAgreement", new { orderItemId = orderItemId });


            if (!product.UnlimitedDownloads && orderItem.DownloadCount >= product.MaxNumberOfDownloads)
                return Content(string.Format(_localizationService.GetResource("DownloadableProducts.ReachedMaximumNumber"), product.MaxNumberOfDownloads));
           
            if (download.UseDownloadUrl)
            {
                //increase download
                orderItem.DownloadCount++;
                _orderService.UpdateOrder(order);

                //return result
                return new RedirectResult(download.DownloadUrl);
            }
            
            //binary download
            if (download.DownloadBinary == null)
                    return Content("Download data is not available any more.");

            //increase download
            orderItem.DownloadCount++;
            _orderService.UpdateOrder(order);

            //return result
            var fileName = !string.IsNullOrWhiteSpace(download.Filename) ? download.Filename : product.Id.ToString();
            var contentType = !string.IsNullOrWhiteSpace(download.ContentType) ? download.ContentType : MimeTypes.ApplicationOctetStream;
            return new FileContentResult(download.DownloadBinary, contentType) { FileDownloadName = fileName + download.Extension };  
        }

        public virtual IActionResult GetLicense(Guid orderItemId)
        {
            var orderItem = _orderService.GetOrderItemByGuid(orderItemId);
            if (orderItem == null)
                return InvokeHttp404();

            var order = orderItem.Order;
            var product = orderItem.Product;
            if (!_downloadService.IsLicenseDownloadAllowed(orderItem))
                return Content("Downloads are not allowed");

            if (_userSettings.DownloadableProductsValidateUser)
            {
                if (_workContext.CurrentUser == null || order.UserId != _workContext.CurrentUser.Id)
                    return Challenge();
            }

            var download = _downloadService.GetDownloadById(orderItem.LicenseDownloadId.HasValue ? orderItem.LicenseDownloadId.Value : 0);
            if (download == null)
                return Content("Download is not available any more.");
            
            if (download.UseDownloadUrl)
                return new RedirectResult(download.DownloadUrl);

            //binary download
            if (download.DownloadBinary == null)
                return Content("Download data is not available any more.");
                
            //return result
            var fileName = !string.IsNullOrWhiteSpace(download.Filename) ? download.Filename : product.Id.ToString();
            var contentType = !string.IsNullOrWhiteSpace(download.ContentType) ? download.ContentType : MimeTypes.ApplicationOctetStream;
            return new FileContentResult(download.DownloadBinary, contentType) { FileDownloadName = fileName + download.Extension };
        }

        public virtual IActionResult GetFileUpload(Guid downloadId)
        {
            var download = _downloadService.GetDownloadByGuid(downloadId);
            if (download == null)
                return Content("Download is not available any more.");

            if (download.UseDownloadUrl)
                return new RedirectResult(download.DownloadUrl);

            //binary download
            if (download.DownloadBinary == null)
                return Content("Download data is not available any more.");

            //return result
            var fileName = !string.IsNullOrWhiteSpace(download.Filename) ? download.Filename : downloadId.ToString();
            var contentType = !string.IsNullOrWhiteSpace(download.ContentType) ? download.ContentType : MimeTypes.ApplicationOctetStream;
            return new FileContentResult(download.DownloadBinary, contentType) { FileDownloadName = fileName + download.Extension };
        }

        public virtual IActionResult GetOrderNoteFile(int orderNoteId)
        {
            var orderNote = _orderService.GetOrderNoteById(orderNoteId);
            if (orderNote == null)
                return InvokeHttp404();

            var order = orderNote.Order;

            if (_workContext.CurrentUser == null || order.UserId != _workContext.CurrentUser.Id)
                return Challenge();

            var download = _downloadService.GetDownloadById(orderNote.DownloadId);
            if (download == null)
                return Content("Download is not available any more.");

            if (download.UseDownloadUrl)
                return new RedirectResult(download.DownloadUrl);

            //binary download
            if (download.DownloadBinary == null)
                return Content("Download data is not available any more.");

            //return result
            var fileName = !string.IsNullOrWhiteSpace(download.Filename) ? download.Filename : orderNote.Id.ToString();
            var contentType = !string.IsNullOrWhiteSpace(download.ContentType) ? download.ContentType : MimeTypes.ApplicationOctetStream;
            return new FileContentResult(download.DownloadBinary, contentType) { FileDownloadName = fileName + download.Extension };
        }
    }
}