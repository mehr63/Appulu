using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Moq;
using Appulu.Core;
using Appulu.Core.Infrastructure;
using Appulu.Core.Plugins;
using Appulu.Services.Tests.Directory;
using Appulu.Services.Tests.Discounts;
using Appulu.Services.Tests.Payments;
using Appulu.Services.Tests.Shipping;
using Appulu.Services.Tests.Tax;
using NUnit.Framework;

namespace Appulu.Services.Tests
{
    [TestFixture]
    public abstract class ServiceTest
    {
        [SetUp]
        public void SetUp()
        {
            //init plugins
            InitPlugins();
        }

        private void InitPlugins()
        {
            var hostingEnvironment = new Mock<IHostingEnvironment>();
            hostingEnvironment.Setup(x => x.ContentRootPath).Returns(System.Reflection.Assembly.GetExecutingAssembly().Location);
            hostingEnvironment.Setup(x => x.WebRootPath).Returns(System.IO.Directory.GetCurrentDirectory());
            CommonHelper.DefaultFileProvider = new AppFileProvider(hostingEnvironment.Object);

            PluginManager.ReferencedPlugins = new List<PluginDescriptor>
            {
                new PluginDescriptor(typeof(FixedRateTestTaxProvider).Assembly)
                {
                    PluginType = typeof(FixedRateTestTaxProvider),
                    SystemName = "FixedTaxRateTest",
                    FriendlyName = "Fixed tax test rate provider",
                    Installed = true
                },
                new PluginDescriptor(typeof(FixedRateTestShippingRateComputationMethod).Assembly)
                {
                    PluginType = typeof(FixedRateTestShippingRateComputationMethod),
                    SystemName = "FixedRateTestShippingRateComputationMethod",
                    FriendlyName = "Fixed rate test shipping computation method",
                    Installed = true
                },
                new PluginDescriptor(typeof(TestPaymentMethod).Assembly)
                {
                    PluginType = typeof(TestPaymentMethod),
                    SystemName = "Payments.TestMethod",
                    FriendlyName = "Test payment method",
                    Installed = true
                },
                new PluginDescriptor(typeof(TestDiscountRequirementRule).Assembly)
                {
                    PluginType = typeof(TestDiscountRequirementRule),
                    SystemName = "TestDiscountRequirementRule",
                    FriendlyName = "Test discount requirement rule",
                    Installed = true
                },
                new PluginDescriptor(typeof(TestExchangeRateProvider).Assembly)
                {
                    PluginType = typeof(TestExchangeRateProvider),
                    SystemName = "CurrencyExchange.TestProvider",
                    FriendlyName = "Test exchange rate provider",
                    Installed = true
                }
            };
        }
    }
}
