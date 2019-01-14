using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Appulu.Core;
using Appulu.Core.Domain.Catalog;
using Appulu.Core.Domain.Users;
using Appulu.Core.Domain.Media;
using Appulu.Core.Domain.Vendors;
using Appulu.Services.Catalog;
using Appulu.Services.Common;
using Appulu.Services.Localization;
using Appulu.Services.Logging;
using Appulu.Services.Security;
using Appulu.Services.Stores;
using Appulu.Services.Vendors;
using Appulu.Web.Factories;
using Appulu.Web.Framework;
using Appulu.Web.Framework.Mvc.Filters;
using Appulu.Web.Framework.Security;
using Appulu.Web.Models.Catalog;

namespace Appulu.Web.Controllers
{
    public partial class CatalogController : BasePublicController
    {
        #region Fields

        private readonly CatalogSettings _catalogSettings;
        private readonly IAclService _aclService;
        private readonly ICatalogModelFactory _catalogModelFactory;
        private readonly ICategoryService _categoryService;
        private readonly IUserActivityService _userActivityService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ILocalizationService _localizationService;
        private readonly IManufacturerService _manufacturerService;
        private readonly IPermissionService _permissionService;
        private readonly IProductModelFactory _productModelFactory;
        private readonly IProductService _productService;
        private readonly IProductTagService _productTagService;
        private readonly IStoreContext _storeContext;
        private readonly IStoreMappingService _storeMappingService;
        private readonly IVendorService _vendorService;
        private readonly IWebHelper _webHelper;
        private readonly IWorkContext _workContext;
        private readonly MediaSettings _mediaSettings;
        private readonly VendorSettings _vendorSettings;

        #endregion

        #region Ctor

        public CatalogController(CatalogSettings catalogSettings,
            IAclService aclService,
            ICatalogModelFactory catalogModelFactory,
            ICategoryService categoryService, 
            IUserActivityService userActivityService,
            IGenericAttributeService genericAttributeService,
            ILocalizationService localizationService,
            IManufacturerService manufacturerService,
            IPermissionService permissionService, 
            IProductModelFactory productModelFactory,
            IProductService productService, 
            IProductTagService productTagService,
            IStoreContext storeContext,
            IStoreMappingService storeMappingService,
            IVendorService vendorService,
            IWebHelper webHelper,
            IWorkContext workContext, 
            MediaSettings mediaSettings,
            VendorSettings vendorSettings)
        {
            this._catalogSettings = catalogSettings;
            this._aclService = aclService;
            this._catalogModelFactory = catalogModelFactory;
            this._categoryService = categoryService;
            this._userActivityService = userActivityService;
            this._genericAttributeService = genericAttributeService;
            this._localizationService = localizationService;
            this._manufacturerService = manufacturerService;
            this._permissionService = permissionService;
            this._productModelFactory = productModelFactory;
            this._productService = productService;
            this._productTagService = productTagService;
            this._storeContext = storeContext;
            this._storeMappingService = storeMappingService;
            this._vendorService = vendorService;
            this._webHelper = webHelper;
            this._workContext = workContext;
            this._mediaSettings = mediaSettings;
            this._vendorSettings = vendorSettings;
        }

        #endregion
        
        #region Categories
        
        [HttpsRequirement(SslRequirement.No)]
        public virtual IActionResult Category(int categoryId, CatalogPagingFilteringModel command)
        {
            var category = _categoryService.GetCategoryById(categoryId);
            if (category == null || category.Deleted)
                return InvokeHttp404();

            var notAvailable =
                //published?
                !category.Published ||
                //ACL (access control list) 
                !_aclService.Authorize(category) ||
                //Store mapping
                !_storeMappingService.Authorize(category);
            //Check whether the current user has a "Manage categories" permission (usually a store owner)
            //We should allows him (her) to use "Preview" functionality
            if (notAvailable && !_permissionService.Authorize(StandardPermissionProvider.ManageCategories))
                return InvokeHttp404();

            //'Continue shopping' URL
            _genericAttributeService.SaveAttribute(_workContext.CurrentUser, 
                AppUserDefaults.LastContinueShoppingPageAttribute, 
                _webHelper.GetThisPageUrl(false),
                _storeContext.CurrentStore.Id);

            //display "edit" (manage) link
            if (_permissionService.Authorize(StandardPermissionProvider.AccessAdminPanel) && _permissionService.Authorize(StandardPermissionProvider.ManageCategories))
                DisplayEditLink(Url.Action("Edit", "Category", new { id = category.Id, area = AreaNames.Admin }));

            //activity log
            _userActivityService.InsertActivity("PublicStore.ViewCategory",
                string.Format(_localizationService.GetResource("ActivityLog.PublicStore.ViewCategory"), category.Name), category);

            //model
            var model = _catalogModelFactory.PrepareCategoryModel(category, command);

            //template
            var templateViewPath = _catalogModelFactory.PrepareCategoryTemplateViewPath(category.CategoryTemplateId);
            return View(templateViewPath, model);
        }

        #endregion

        #region Manufacturers

        [HttpsRequirement(SslRequirement.No)]
        public virtual IActionResult Manufacturer(int manufacturerId, CatalogPagingFilteringModel command)
        {
            var manufacturer = _manufacturerService.GetManufacturerById(manufacturerId);
            if (manufacturer == null || manufacturer.Deleted)
                return InvokeHttp404();

            var notAvailable =
                //published?
                !manufacturer.Published ||
                //ACL (access control list) 
                !_aclService.Authorize(manufacturer) ||
                //Store mapping
                !_storeMappingService.Authorize(manufacturer);
            //Check whether the current user has a "Manage categories" permission (usually a store owner)
            //We should allows him (her) to use "Preview" functionality
            if (notAvailable && !_permissionService.Authorize(StandardPermissionProvider.ManageManufacturers))
                return InvokeHttp404();

            //'Continue shopping' URL
            _genericAttributeService.SaveAttribute(_workContext.CurrentUser, 
                AppUserDefaults.LastContinueShoppingPageAttribute, 
                _webHelper.GetThisPageUrl(false),
                _storeContext.CurrentStore.Id);
            
            //display "edit" (manage) link
            if (_permissionService.Authorize(StandardPermissionProvider.AccessAdminPanel) && _permissionService.Authorize(StandardPermissionProvider.ManageManufacturers))
                DisplayEditLink(Url.Action("Edit", "Manufacturer", new { id = manufacturer.Id, area = AreaNames.Admin }));

            //activity log
            _userActivityService.InsertActivity("PublicStore.ViewManufacturer",
                string.Format(_localizationService.GetResource("ActivityLog.PublicStore.ViewManufacturer"), manufacturer.Name), manufacturer);

            //model
            var model = _catalogModelFactory.PrepareManufacturerModel(manufacturer, command);
            
            //template
            var templateViewPath = _catalogModelFactory.PrepareManufacturerTemplateViewPath(manufacturer.ManufacturerTemplateId);
            return View(templateViewPath, model);
        }

        [HttpsRequirement(SslRequirement.No)]
        public virtual IActionResult ManufacturerAll()
        {
            var model = _catalogModelFactory.PrepareManufacturerAllModels();
            return View(model);
        }
        
        #endregion

        #region Vendors

        [HttpsRequirement(SslRequirement.No)]
        public virtual IActionResult Vendor(int vendorId, CatalogPagingFilteringModel command)
        {
            var vendor = _vendorService.GetVendorById(vendorId);
            if (vendor == null || vendor.Deleted || !vendor.Active)
                return InvokeHttp404();

            //'Continue shopping' URL
            _genericAttributeService.SaveAttribute(_workContext.CurrentUser,
                AppUserDefaults.LastContinueShoppingPageAttribute,
                _webHelper.GetThisPageUrl(false),
                _storeContext.CurrentStore.Id);
            
            //display "edit" (manage) link
            if (_permissionService.Authorize(StandardPermissionProvider.AccessAdminPanel) && _permissionService.Authorize(StandardPermissionProvider.ManageVendors))
                DisplayEditLink(Url.Action("Edit", "Vendor", new { id = vendor.Id, area = AreaNames.Admin }));

            //model
            var model = _catalogModelFactory.PrepareVendorModel(vendor, command);

            return View(model);
        }

        [HttpsRequirement(SslRequirement.No)]
        public virtual IActionResult VendorAll()
        {
            //we don't allow viewing of vendors if "vendors" block is hidden
            if (_vendorSettings.VendorsBlockItemsToDisplay == 0)
                return RedirectToRoute("HomePage");

            var model = _catalogModelFactory.PrepareVendorAllModels();
            return View(model);
        }

        #endregion

        #region Product tags
        
        [HttpsRequirement(SslRequirement.No)]
        public virtual IActionResult ProductsByTag(int productTagId, CatalogPagingFilteringModel command)
        {
            var productTag = _productTagService.GetProductTagById(productTagId);
            if (productTag == null)
                return InvokeHttp404();

            var model = _catalogModelFactory.PrepareProductsByTagModel(productTag, command);
            return View(model);
        }

        [HttpsRequirement(SslRequirement.No)]
        public virtual IActionResult ProductTagsAll()
        {
            var model = _catalogModelFactory.PrepareProductTagsAllModel();
            return View(model);
        }

        #endregion

        #region Searching

        [HttpsRequirement(SslRequirement.No)]
        public virtual IActionResult Search(SearchModel model, CatalogPagingFilteringModel command)
        {
            //'Continue shopping' URL
            _genericAttributeService.SaveAttribute(_workContext.CurrentUser,
                AppUserDefaults.LastContinueShoppingPageAttribute,
                _webHelper.GetThisPageUrl(false),
                _storeContext.CurrentStore.Id);

            if (model == null)
                model = new SearchModel();

            model = _catalogModelFactory.PrepareSearchModel(model, command);
            return View(model);
        }

        public virtual IActionResult SearchTermAutoComplete(string term)
        {
            if (string.IsNullOrWhiteSpace(term) || term.Length < _catalogSettings.ProductSearchTermMinimumLength)
                return Content("");

            //products
            var productNumber = _catalogSettings.ProductSearchAutoCompleteNumberOfProducts > 0 ?
                _catalogSettings.ProductSearchAutoCompleteNumberOfProducts : 10;            

            var products = _productService.SearchProducts(
                storeId: _storeContext.CurrentStore.Id,
                keywords: term,
                languageId: _workContext.WorkingLanguage.Id,
                visibleIndividuallyOnly: true,
                pageSize: productNumber);

            var showLinkToResultSearch = _catalogSettings.ShowLinkToAllResultInSearchAutoComplete && (products.TotalCount > productNumber);

            var models =  _productModelFactory.PrepareProductOverviewModels(products, false, _catalogSettings.ShowProductImagesInSearchAutoComplete, _mediaSettings.AutoCompleteSearchThumbPictureSize).ToList();
            var result = (from p in models
                    select new
                    {
                        label = p.Name,
                        producturl = Url.RouteUrl("Product", new {SeName = p.SeName}),
                        productpictureurl = p.DefaultPictureModel.ImageUrl,
                        showlinktoresultsearch = showLinkToResultSearch
                    })
                .ToList();
            return Json(result);
        }
        
        #endregion
    }
}