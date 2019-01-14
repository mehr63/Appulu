using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Appulu.Core;
using Appulu.Core.Caching;
using Appulu.Core.Domain.Catalog;
using Appulu.Services.Catalog;
using Appulu.Services.Orders;
using Appulu.Services.Security;
using Appulu.Services.Stores;
using Appulu.Web.Factories;
using Appulu.Web.Framework.Components;
using Appulu.Web.Infrastructure.Cache;

namespace Appulu.Web.Components
{
    public class ProductsAlsoPurchasedViewComponent : AppViewComponent
    {
        private readonly CatalogSettings _catalogSettings;
        private readonly IAclService _aclService;
        private readonly IOrderReportService _orderReportService;
        private readonly IProductModelFactory _productModelFactory;
        private readonly IProductService _productService;
        private readonly IStaticCacheManager _cacheManager;
        private readonly IStoreContext _storeContext;
        private readonly IStoreMappingService _storeMappingService;

        public ProductsAlsoPurchasedViewComponent(CatalogSettings catalogSettings,
            IAclService aclService,
            IOrderReportService orderReportService,
            IProductModelFactory productModelFactory,
            IProductService productService,
            IStaticCacheManager cacheManager,
            IStoreContext storeContext,
            IStoreMappingService storeMappingService)
        {
            this._catalogSettings = catalogSettings;
            this._aclService = aclService;
            this._orderReportService = orderReportService;
            this._productModelFactory = productModelFactory;
            this._productService = productService;
            this._cacheManager = cacheManager;
            this._storeContext = storeContext;
            this._storeMappingService = storeMappingService;
        }

        public IViewComponentResult Invoke(int productId, int? productThumbPictureSize)
        {
            if (!_catalogSettings.ProductsAlsoPurchasedEnabled)
                return Content("");

            //load and cache report
            var productIds = _cacheManager.Get(string.Format(ModelCacheEventConsumer.PRODUCTS_ALSO_PURCHASED_IDS_KEY, productId, _storeContext.CurrentStore.Id),
                () => _orderReportService.GetAlsoPurchasedProductsIds(_storeContext.CurrentStore.Id, productId, _catalogSettings.ProductsAlsoPurchasedNumber)
            );

            //load products
            var products = _productService.GetProductsByIds(productIds);
            //ACL and store mapping
            products = products.Where(p => _aclService.Authorize(p) && _storeMappingService.Authorize(p)).ToList();
            //availability dates
            products = products.Where(p => _productService.ProductIsAvailable(p)).ToList();

            if (!products.Any())
                return Content("");

            var model = _productModelFactory.PrepareProductOverviewModels(products, true, true, productThumbPictureSize).ToList();
            return View(model);
        }
    }
}