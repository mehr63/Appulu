using System;
using System.Linq;
using Moq;
using Appulu.Core;
using Appulu.Core.Domain.Catalog;
using Appulu.Core.Domain.Common;
using Appulu.Core.Domain.Users;
using Appulu.Core.Domain.Shipping;
using Appulu.Core.Domain.Stores;
using Appulu.Core.Domain.Tax;
using Appulu.Services.Common;
using Appulu.Services.Directory;
using Appulu.Services.Events;
using Appulu.Services.Logging;
using Appulu.Services.Plugins;
using Appulu.Services.Tax;
using Appulu.Tests;
using NUnit.Framework;

namespace Appulu.Services.Tests.Tax
{
    [TestFixture]
    public class TaxServiceTests : ServiceTest
    {
        private Mock<IAddressService> _addressService;
        private IWorkContext _workContext;
        private Mock<IStoreContext> _storeContext;
        private TaxSettings _taxSettings;
        private Mock<IEventPublisher> _eventPublisher;
        private ITaxService _taxService;
        private Mock<IGeoLookupService> _geoLookupService;
        private Mock<ICountryService> _countryService;
        private Mock<IStateProvinceService> _stateProvinceService;
        private Mock<ILogger> _logger;
        private Mock<IWebHelper> _webHelper;
        private UserSettings _userSettings;
        private ShippingSettings _shippingSettings;
        private AddressSettings _addressSettings;
        private Mock<IGenericAttributeService> _genericAttributeService;

        [SetUp]
        public new void SetUp()
        {
            _taxSettings = new TaxSettings
            {
                DefaultTaxAddressId = 10
            };

            _workContext = null;
            _storeContext = new Mock<IStoreContext>();
            _storeContext.Setup(x => x.CurrentStore).Returns(new Store { Id = 1 });

            _addressService = new Mock<IAddressService>();
            //default tax address
            _addressService.Setup(x => x.GetAddressById(_taxSettings.DefaultTaxAddressId)).Returns(new Address { Id = _taxSettings.DefaultTaxAddressId });

            _eventPublisher = new Mock<IEventPublisher>();
            _eventPublisher.Setup(x => x.Publish(It.IsAny<object>()));

            var pluginFinder = new PluginFinder(_eventPublisher.Object);

            _geoLookupService = new Mock<IGeoLookupService>();
            _countryService = new Mock<ICountryService>();
            _stateProvinceService = new Mock<IStateProvinceService>();
            _logger = new Mock<ILogger>();
            _webHelper = new Mock<IWebHelper>();
            _genericAttributeService = new Mock<IGenericAttributeService>();

            _userSettings = new UserSettings();
            _shippingSettings = new ShippingSettings();
            _addressSettings = new AddressSettings();

            _taxService = new TaxService(_addressSettings,
                _userSettings,
                _addressService.Object,
                _countryService.Object,
                _genericAttributeService.Object,
                _geoLookupService.Object,
                _logger.Object,
                pluginFinder,
                _stateProvinceService.Object,
                _storeContext.Object,
                _webHelper.Object,
                _workContext,
                _shippingSettings,
                _taxSettings);
        }

        [Test]
        public void Can_load_taxProviders()
        {
            var providers = _taxService.LoadAllTaxProviders();
            providers.ShouldNotBeNull();
            providers.Any().ShouldBeTrue();
        }

        [Test]
        public void Can_load_taxProvider_by_systemKeyword()
        {
            var provider = _taxService.LoadTaxProviderBySystemName("FixedTaxRateTest");
            provider.ShouldNotBeNull();
        }

        [Test]
        public void Can_load_active_taxProvider()
        {
            var provider = _taxService.LoadActiveTaxProvider();
            provider.ShouldNotBeNull();
        }

        [Test]
        public void Can_check_taxExempt_product()
        {
            var product = new Product
            {
                IsTaxExempt = true
            };
            _taxService.IsTaxExempt(product, null).ShouldEqual(true);
            product.IsTaxExempt = false;
            _taxService.IsTaxExempt(product, null).ShouldEqual(false);
        }

        [Test]
        public void Can_check_taxExempt_user()
        {
            var user = new User
            {
                IsTaxExempt = true
            };
            _taxService.IsTaxExempt(null, user).ShouldEqual(true);
            user.IsTaxExempt = false;
            _taxService.IsTaxExempt(null, user).ShouldEqual(false);
        }

        [Test]
        public void Can_check_taxExempt_user_in_taxExemptUserRole()
        {
            var user = new User
            {
                IsTaxExempt = false
            };
            _taxService.IsTaxExempt(null, user).ShouldEqual(false);

            var userRole = new UserRole
            {
                TaxExempt = true,
                Active = true
            };
            user.UserRoles.Add(userRole);
            _taxService.IsTaxExempt(null, user).ShouldEqual(true);
            userRole.TaxExempt = false;
            _taxService.IsTaxExempt(null, user).ShouldEqual(false);

            //if role is not active, we should ignore 'TaxExempt' property
            userRole.Active = false;
            _taxService.IsTaxExempt(null, user).ShouldEqual(false);
        }

        [Test]
        public void Can_get_productPrice_priceIncludesTax_includingTax_taxable()
        {
            var user = new User();
            var product = new Product();

            _taxService.GetProductPrice(product, 0, 1000M, true, user, true, out _).ShouldEqual(1000);
            _taxService.GetProductPrice(product, 0, 1000M, true, user, false, out _).ShouldEqual(1100);
            _taxService.GetProductPrice(product, 0, 1000M, false, user, true, out _).ShouldEqual(909.0909090909090909090909091M);
            _taxService.GetProductPrice(product, 0, 1000M, false, user, false, out _).ShouldEqual(1000);
        }

        [Test]
        public void Can_get_productPrice_priceIncludesTax_includingTax_non_taxable()
        {
            var user = new User();
            var product = new Product();

            //not taxable
            user.IsTaxExempt = true;

            _taxService.GetProductPrice(product, 0, 1000M, true, user, true, out _).ShouldEqual(909.0909090909090909090909091M);
            _taxService.GetProductPrice(product, 0, 1000M, true, user, false, out _).ShouldEqual(1000);
            _taxService.GetProductPrice(product, 0, 1000M, false, user, true, out _).ShouldEqual(909.0909090909090909090909091M);
            _taxService.GetProductPrice(product, 0, 1000M, false, user, false, out _).ShouldEqual(1000);
        }

        [Test]
        public void Can_do_VAT_check()
        {
            //remove? this method requires Internet access
            
            var vatNumberStatus1 = _taxService.DoVatCheck("GB", "523 2392 69",
                out _, out _, out Exception exception);
            vatNumberStatus1.ShouldEqual(VatNumberStatus.Valid);
            exception.ShouldBeNull();

            var vatNumberStatus2 = _taxService.DoVatCheck("GB", "000 0000 00",
                out _, out _, out exception);
            vatNumberStatus2.ShouldEqual(VatNumberStatus.Invalid);
            exception.ShouldBeNull();
        }

        [Test]
        public void Should_assume_valid_VAT_number_if_EuVatAssumeValid_setting_is_true()
        {
            _taxSettings.EuVatAssumeValid = true;

            var vatNumberStatus = _taxService.GetVatNumberStatus("GB", "000 0000 00", out _, out _);
            vatNumberStatus.ShouldEqual(VatNumberStatus.Valid);
        }
    }
}
