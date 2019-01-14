using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Appulu.Core;
using Appulu.Core.Caching;
using Appulu.Core.Data;
using Appulu.Core.Domain.Catalog;
using Appulu.Core.Domain.Users;
using Appulu.Core.Domain.Discounts;
using Appulu.Core.Domain.Stores;
using Appulu.Services.Catalog;
using Appulu.Services.Users;
using Appulu.Services.Discounts;
using Appulu.Services.Events;
using Appulu.Services.Localization;
using Appulu.Services.Plugins;

namespace Appulu.Services.Tests
{
    public class TestDiscountService : DiscountService
    {
        private readonly List<DiscountForCaching> _discountForCaching;

        public TestDiscountService(ICategoryService categoryService, IUserService userService, IEventPublisher eventPublisher, ILocalizationService localizationService, IPluginFinder pluginFinder, IRepository<Category> categoryRepository, IRepository<Discount> discountRepository, IRepository<DiscountRequirement> discountRequirementRepository, IRepository<DiscountUsageHistory> discountUsageHistoryRepository, IRepository<Manufacturer> manufacturerRepository, IRepository<Product> productRepository, IStaticCacheManager cacheManager, IStoreContext storeContext) : base(categoryService, userService, eventPublisher, localizationService, pluginFinder, categoryRepository, discountRepository, discountRequirementRepository, discountUsageHistoryRepository, manufacturerRepository, productRepository, cacheManager, storeContext)
        {
            _discountForCaching = new List<DiscountForCaching>();
        }

        public override DiscountValidationResult ValidateDiscount(Discount discount, User user)
        {
            return new DiscountValidationResult { IsValid = true };
        }

        public override DiscountValidationResult ValidateDiscount(DiscountForCaching discount, User user)
        {
            return new DiscountValidationResult { IsValid = true };
        }

        public override IList<DiscountForCaching> GetAllDiscountsForCaching(DiscountType? discountType = null,
            string couponCode = null, string discountName = null,
            bool showHidden = false)
        {
            
            return _discountForCaching
                .Where(x=> !discountType.HasValue || x.DiscountType == discountType.Value)
                .Where(x => string.IsNullOrEmpty(couponCode) || x.CouponCode == couponCode)
                //UNDONE other filtering such as discountName, showHidden (not actually required in unit tests)
                .ToList();
        }

        public void AddDiscount(DiscountType discountType)
        {
            _discountForCaching.Clear();

            //discounts
            var discount = new DiscountForCaching
            {
                Id = 1,
                Name = "Discount 1",
                DiscountType = discountType,
                DiscountAmount = 3,
                DiscountLimitation = DiscountLimitationType.Unlimited
            };

            _discountForCaching.Add(discount);
        }

        public void ClearDiscount()
        {
            _discountForCaching.Clear();
        }

        public static IDiscountService Init()
        {
            var _cacheManager = new TestMemoryCacheManager(new Mock<IMemoryCache>().Object);
            var _discountRepo = new Mock<IRepository<Discount>>();
            var _discountRequirementRepo = new Mock<IRepository<DiscountRequirement>>();
            _discountRequirementRepo.Setup(x => x.Table).Returns(new List<DiscountRequirement>().AsQueryable());
            var _discountUsageHistoryRepo = new Mock<IRepository<DiscountUsageHistory>>();
            var _categoryRepo = new Mock<IRepository<Category>>();
            _categoryRepo.Setup(x => x.Table).Returns(new List<Category>().AsQueryable());
            var _manufacturerRepo = new Mock<IRepository<Manufacturer>>();
            _manufacturerRepo.Setup(x => x.Table).Returns(new List<Manufacturer>().AsQueryable());
            var _productRepo = new Mock<IRepository<Product>>();
            _productRepo.Setup(x => x.Table).Returns(new List<Product>().AsQueryable());
            var _userService = new Mock<IUserService>();
            var _localizationService = new Mock<ILocalizationService>();
            var _eventPublisher = new Mock<IEventPublisher>();
            var pluginFinder = new PluginFinder(_eventPublisher.Object);
            var _categoryService = new Mock<ICategoryService>();

            var _store = new Store {Id = 1};
            var _storeContext = new Mock<IStoreContext>();
            _storeContext.Setup(x => x.CurrentStore).Returns(_store);

            var discountService = new TestDiscountService(_categoryService.Object,
                _userService.Object,
                _eventPublisher.Object,
                _localizationService.Object,
                pluginFinder,
                _categoryRepo.Object,
                _discountRepo.Object,
                _discountRequirementRepo.Object,
                _discountUsageHistoryRepo.Object,
                _manufacturerRepo.Object,
                _productRepo.Object,
                _cacheManager,
                _storeContext.Object);

            return discountService;
        }
    }
}
