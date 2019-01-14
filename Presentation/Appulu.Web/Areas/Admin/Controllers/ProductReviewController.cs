using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Appulu.Core;
using Appulu.Core.Domain.Catalog;
using Appulu.Core.Domain.Users;
using Appulu.Services.Catalog;
using Appulu.Services.Common;
using Appulu.Services.Events;
using Appulu.Services.Localization;
using Appulu.Services.Logging;
using Appulu.Services.Messages;
using Appulu.Services.Security;
using Appulu.Web.Areas.Admin.Factories;
using Appulu.Web.Areas.Admin.Models.Catalog;
using Appulu.Web.Framework.Mvc.Filters;

namespace Appulu.Web.Areas.Admin.Controllers
{
    public partial class ProductReviewController : BaseAdminController
    {
        #region Fields

        private readonly CatalogSettings _catalogSettings;
        private readonly IUserActivityService _userActivityService;
        private readonly IEventPublisher _eventPublisher;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly IProductReviewModelFactory _productReviewModelFactory;
        private readonly IProductService _productService;
        private readonly IWorkContext _workContext;
        private readonly IWorkflowMessageService _workflowMessageService;

        #endregion Fields

        #region Ctor

        public ProductReviewController(CatalogSettings catalogSettings,
            IUserActivityService userActivityService,
            IEventPublisher eventPublisher,
            IGenericAttributeService genericAttributeService,
            ILocalizationService localizationService,
            IPermissionService permissionService,
            IProductReviewModelFactory productReviewModelFactory,
            IProductService productService,
            IWorkContext workContext,
            IWorkflowMessageService workflowMessageService)
        {
            this._catalogSettings = catalogSettings;
            this._userActivityService = userActivityService;
            this._eventPublisher = eventPublisher;
            this._genericAttributeService = genericAttributeService;
            this._localizationService = localizationService;
            this._permissionService = permissionService;
            this._productReviewModelFactory = productReviewModelFactory;
            this._productService = productService;
            this._workContext = workContext;
            this._workflowMessageService = workflowMessageService;
        }

        #endregion

        #region Methods

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProductReviews))
                return AccessDeniedView();

            //prepare model
            var model = _productReviewModelFactory.PrepareProductReviewSearchModel(new ProductReviewSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult List(ProductReviewSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProductReviews))
                return AccessDeniedKendoGridJson();

            //prepare model
            var model = _productReviewModelFactory.PrepareProductReviewListModel(searchModel);

            return Json(model);
        }

        public virtual IActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProductReviews))
                return AccessDeniedView();

            //try to get a product review with the specified id
            var productReview = _productService.GetProductReviewById(id);
            if (productReview == null)
                return RedirectToAction("List");

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null && productReview.Product.VendorId != _workContext.CurrentVendor.Id)
                return RedirectToAction("List");

            //prepare model
            var model = _productReviewModelFactory.PrepareProductReviewModel(null, productReview);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Edit(ProductReviewModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProductReviews))
                return AccessDeniedView();

            //try to get a product review with the specified id
            var productReview = _productService.GetProductReviewById(model.Id);
            if (productReview == null)
                return RedirectToAction("List");

            //a vendor should have access only to his products
            if (_workContext.CurrentVendor != null && productReview.Product.VendorId != _workContext.CurrentVendor.Id)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                var previousIsApproved = productReview.IsApproved;

                //vendor can edit "Reply text" only
                var isLoggedInAsVendor = _workContext.CurrentVendor != null;
                if (!isLoggedInAsVendor)
                {
                    productReview.Title = model.Title;
                    productReview.ReviewText = model.ReviewText;
                    productReview.IsApproved = model.IsApproved;
                }

                productReview.ReplyText = model.ReplyText;

                //notify user about reply
                if (productReview.IsApproved && !string.IsNullOrEmpty(productReview.ReplyText)
                    && _catalogSettings.NotifyUserAboutProductReviewReply && !productReview.UserNotifiedOfReply)
                {
                    var userLanguageId = _genericAttributeService.GetAttribute<int>(productReview.User,
                        AppUserDefaults.LanguageIdAttribute, productReview.StoreId);
                    var queuedEmailIds = _workflowMessageService.SendProductReviewReplyUserNotificationMessage(productReview, userLanguageId);
                    if (queuedEmailIds.Any())
                        productReview.UserNotifiedOfReply = true;
                }

                _productService.UpdateProduct(productReview.Product);

                //activity log
                _userActivityService.InsertActivity("EditProductReview",
                   string.Format(_localizationService.GetResource("ActivityLog.EditProductReview"), productReview.Id), productReview);

                //vendor can edit "Reply text" only
                if (!isLoggedInAsVendor)
                {
                    //update product totals
                    _productService.UpdateProductReviewTotals(productReview.Product);

                    //raise event (only if it wasn't approved before and is approved now)
                    if (!previousIsApproved && productReview.IsApproved)
                        _eventPublisher.Publish(new ProductReviewApprovedEvent(productReview));
                }

                SuccessNotification(_localizationService.GetResource("Admin.Catalog.ProductReviews.Updated"));

                return continueEditing ? RedirectToAction("Edit", new { id = productReview.Id }) : RedirectToAction("List");
            }

            //prepare model
            model = _productReviewModelFactory.PrepareProductReviewModel(model, productReview, true);

            //if we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProductReviews))
                return AccessDeniedView();

            //try to get a product review with the specified id
            var productReview = _productService.GetProductReviewById(id);
            if (productReview == null)
                return RedirectToAction("List");

            //a vendor does not have access to this functionality
            if (_workContext.CurrentVendor != null)
                return RedirectToAction("List");

            var product = productReview.Product;
            _productService.DeleteProductReview(productReview);

            //activity log
            _userActivityService.InsertActivity("DeleteProductReview",
                string.Format(_localizationService.GetResource("ActivityLog.DeleteProductReview"), productReview.Id), productReview);

            //update product totals
            _productService.UpdateProductReviewTotals(product);

            SuccessNotification(_localizationService.GetResource("Admin.Catalog.ProductReviews.Deleted"));

            return RedirectToAction("List");
        }

        [HttpPost]
        public virtual IActionResult ApproveSelected(ICollection<int> selectedIds)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProductReviews))
                return AccessDeniedView();

            //a vendor does not have access to this functionality
            if (_workContext.CurrentVendor != null)
                return RedirectToAction("List");

            if (selectedIds == null)
                return Json(new { Result = true });

            //filter not approved reviews
            var productReviews = _productService.GetProducReviewsByIds(selectedIds.ToArray()).Where(review => !review.IsApproved);
            foreach (var productReview in productReviews)
            {
                productReview.IsApproved = true;
                _productService.UpdateProduct(productReview.Product);

                //update product totals
                _productService.UpdateProductReviewTotals(productReview.Product);

                //raise event 
                _eventPublisher.Publish(new ProductReviewApprovedEvent(productReview));
            }

            return Json(new { Result = true });
        }

        [HttpPost]
        public virtual IActionResult DisapproveSelected(ICollection<int> selectedIds)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProductReviews))
                return AccessDeniedView();

            //a vendor does not have access to this functionality
            if (_workContext.CurrentVendor != null)
                return RedirectToAction("List");

            if (selectedIds == null)
                return Json(new { Result = true });

            //filter approved reviews
            var productReviews = _productService.GetProducReviewsByIds(selectedIds.ToArray()).Where(review => review.IsApproved);
            foreach (var productReview in productReviews)
            {
                productReview.IsApproved = false;
                _productService.UpdateProduct(productReview.Product);

                //update product totals
                _productService.UpdateProductReviewTotals(productReview.Product);
            }

            return Json(new { Result = true });
        }

        [HttpPost]
        public virtual IActionResult DeleteSelected(ICollection<int> selectedIds)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProductReviews))
                return AccessDeniedView();

            //a vendor does not have access to this functionality
            if (_workContext.CurrentVendor != null)
                return RedirectToAction("List");

            if (selectedIds == null)
                return Json(new { Result = true });

            var productReviews = _productService.GetProducReviewsByIds(selectedIds.ToArray());
            var products = _productService.GetProductsByIds(productReviews.Select(p => p.ProductId).Distinct().ToArray());

            _productService.DeleteProductReviews(productReviews);

            //update product totals
            foreach (var product in products)
            {
                _productService.UpdateProductReviewTotals(product);
            }

            return Json(new { Result = true });
        }

        public virtual IActionResult ProductSearchAutoComplete(string term)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProductReviews))
                return Content(string.Empty);

            const int searchTermMinimumLength = 3;
            if (string.IsNullOrWhiteSpace(term) || term.Length < searchTermMinimumLength)
                return Content(string.Empty);

            //a vendor should have access only to his products
            var vendorId = 0;
            if (_workContext.CurrentVendor != null)
            {
                vendorId = _workContext.CurrentVendor.Id;
            }

            //products
            const int productNumber = 15;
            var products = _productService.SearchProducts(
                keywords: term,
                vendorId: vendorId,
                pageSize: productNumber,
                showHidden: true);

            var result = (from p in products
                          select new
                          {
                              label = p.Name,
                              productid = p.Id
                          })
                .ToList();
            return Json(result);
        }

        [HttpPost]
        public virtual IActionResult ProductReviewReviewTypeMappingList(ProductReviewReviewTypeMappingSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageProductReviews))
                return AccessDeniedKendoGridJson();
            var productReview = _productService.GetProductReviewById(searchModel.ProductReviewId)
                ?? throw new ArgumentException("No product review found with the specified id");

            //prepare model
            var model = _productReviewModelFactory.PrepareProductReviewReviewTypeMappingListModel(searchModel, productReview);

            return Json(model);
        }

        #endregion
    }
}