using Moq;
using Appulu.Core;
using Appulu.Core.Infrastructure;
using Appulu.Services.Localization;
using Appulu.Tests;
using Appulu.Web.Areas.Admin.Validators.Common;
using NUnit.Framework;

namespace Appulu.Web.MVC.Tests.Public.Validators
{
    [TestFixture]
    public abstract class BaseValidatorTests
    {
        protected ILocalizationService _localizationService;
        protected Mock<IWorkContext> _workContext;
        
        [SetUp]
        public void Setup()
        {
            var appEngine = new Mock<AppEngine>();
            var serviceProvider = new TestServiceProvider();
            appEngine.Setup(x => x.ServiceProvider).Returns(serviceProvider);
            appEngine.Setup(x => x.ResolveUnregistered(typeof(AddressValidator))).Returns(new AddressValidator(serviceProvider.LocalizationService.Object));
            EngineContext.Replace(appEngine.Object);
            _localizationService = serviceProvider.LocalizationService.Object;
        }
    }
}
