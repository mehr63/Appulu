﻿using System.Collections.Generic;
using System.Linq;
using Moq;
using Appulu.Core;
using Appulu.Core.Caching;
using Appulu.Core.Data;
using Appulu.Core.Domain.Catalog;
using Appulu.Core.Domain.Orders;
using Appulu.Core.Domain.Shipping;
using Appulu.Core.Domain.Stores;
using Appulu.Services.Catalog;
using Appulu.Services.Common;
using Appulu.Services.Events;
using Appulu.Services.Localization;
using Appulu.Services.Logging;
using Appulu.Services.Orders;
using Appulu.Services.Plugins;
using Appulu.Services.Shipping;
using Appulu.Tests;
using NUnit.Framework;

namespace Appulu.Services.Tests.Shipping
{
    [TestFixture]
    public class ShippingServiceTests : ServiceTest
    {
        private Mock<IRepository<ShippingMethod>> _shippingMethodRepository;
        private Mock<IRepository<Warehouse>> _warehouseRepository;
        private ILogger _logger;
        private Mock<IProductAttributeParser> _productAttributeParser;
        private Mock<ICheckoutAttributeParser> _checkoutAttributeParser;
        private ShippingSettings _shippingSettings;
        private Mock<IEventPublisher> _eventPublisher;
        private Mock<ILocalizationService> _localizationService;
        private Mock<IAddressService> _addressService;
        private Mock<IGenericAttributeService> _genericAttributeService;
        private IShippingService _shippingService;
        private ShoppingCartSettings _shoppingCartSettings;
        private Mock<IProductService> _productService;
        private Store _store;
        private Mock<IStoreContext> _storeContext;
        private Mock<IPriceCalculationService> _priceCalcService;

        [SetUp]
        public new void SetUp()
        {
            _shippingSettings = new ShippingSettings
            {
                ActiveShippingRateComputationMethodSystemNames = new List<string>()
            };
            _shippingSettings.ActiveShippingRateComputationMethodSystemNames.Add("FixedRateTestShippingRateComputationMethod");

            _shippingMethodRepository = new Mock<IRepository<ShippingMethod>>();
            _warehouseRepository = new Mock<IRepository<Warehouse>>();
            _logger = new NullLogger();
            _productAttributeParser = new Mock<IProductAttributeParser>();
            _checkoutAttributeParser = new Mock<ICheckoutAttributeParser>();

            var cacheManager = new AppNullCache();
            
            _productService = new Mock<IProductService>();

            _eventPublisher = new Mock<IEventPublisher>();
            _eventPublisher.Setup(x => x.Publish(It.IsAny<object>()));

            var pluginFinder = new PluginFinder(_eventPublisher.Object);

            _localizationService = new Mock<ILocalizationService>();
            _addressService = new Mock<IAddressService>();
            _genericAttributeService = new Mock<IGenericAttributeService>();
            _priceCalcService = new Mock<IPriceCalculationService>();

            _store = new Store { Id = 1 };
            _storeContext = new Mock<IStoreContext>();
            _storeContext.Setup(x => x.CurrentStore).Returns(_store);

            _shoppingCartSettings = new ShoppingCartSettings();

            _shippingService = new ShippingService(_addressService.Object,
                cacheManager,
                _checkoutAttributeParser.Object,
                _eventPublisher.Object,
                _genericAttributeService.Object,
                _localizationService.Object,
                _logger,
                pluginFinder,
                _priceCalcService.Object,
                _productAttributeParser.Object,
                _productService.Object,
                _shippingMethodRepository.Object,
                _warehouseRepository.Object,
                _storeContext.Object,
                _shippingSettings,
                _shoppingCartSettings);
        }

        [Test]
        public void Can_load_shippingRateComputationMethods()
        {
            var srcm = _shippingService.LoadAllShippingRateComputationMethods();
            srcm.ShouldNotBeNull();
            srcm.Any().ShouldBeTrue();
        }

        [Test]
        public void Can_load_shippingRateComputationMethod_by_systemKeyword()
        {
            var srcm = _shippingService.LoadShippingRateComputationMethodBySystemName("FixedRateTestShippingRateComputationMethod");
            srcm.ShouldNotBeNull();
        }

        [Test]
        public void Can_load_active_shippingRateComputationMethods()
        {
            var srcm = _shippingService.LoadActiveShippingRateComputationMethods();
            srcm.ShouldNotBeNull();
            srcm.Any().ShouldBeTrue();
        }
        
        [Test]
        public void Can_get_shoppingCart_totalWeight_without_attributes()
        {
            var request = new GetShippingOptionRequest
            {
                Items =
                {
                    new GetShippingOptionRequest.PackageItem(new ShoppingCartItem
                    {
                        AttributesXml = "",
                        Quantity = 3,
                        Product = new Product
                        {
                            Weight = 1.5M,
                            Height = 2.5M,
                            Length = 3.5M,
                            Width = 4.5M
                        }
                    }),
                    new GetShippingOptionRequest.PackageItem(new ShoppingCartItem
                    {
                        AttributesXml = "",
                        Quantity = 4,
                        Product = new Product
                        {
                            Weight = 11.5M,
                            Height = 12.5M,
                            Length = 13.5M,
                            Width = 14.5M
                        }
                    })
                }
            };
            _shippingService.GetTotalWeight(request).ShouldEqual(50.5M);
        }
    }
}