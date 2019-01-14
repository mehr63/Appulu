using System;
using System.Linq;
using Appulu.Core.Domain.Users;
using Appulu.Core.Domain.Orders;
using Appulu.Services.Catalog;
using Appulu.Services.Users;
using Appulu.Services.Helpers;
using Appulu.Services.Localization;
using Appulu.Services.Stores;
using Appulu.Services.Tax;
using Appulu.Web.Areas.Admin.Models.ShoppingCart;
using Appulu.Web.Framework.Extensions;

namespace Appulu.Web.Areas.Admin.Factories
{
    /// <summary>
    /// Represents the shopping cart model factory implementation
    /// </summary>
    public partial class ShoppingCartModelFactory : IShoppingCartModelFactory
    {
        #region Fields

        private readonly IBaseAdminModelFactory _baseAdminModelFactory;
        private readonly IUserService _userService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly ILocalizationService _localizationService;
        private readonly IPriceCalculationService _priceCalculationService;
        private readonly IPriceFormatter _priceFormatter;
        private readonly IProductAttributeFormatter _productAttributeFormatter;
        private readonly IStoreService _storeService;
        private readonly ITaxService _taxService;

        #endregion

        #region Ctor

        public ShoppingCartModelFactory(IBaseAdminModelFactory baseAdminModelFactory,
            IUserService userService,
            IDateTimeHelper dateTimeHelper,
            ILocalizationService localizationService,
            IPriceCalculationService priceCalculationService,
            IPriceFormatter priceFormatter,
            IProductAttributeFormatter productAttributeFormatter,
            IStoreService storeService,
            ITaxService taxService)
        {
            this._baseAdminModelFactory = baseAdminModelFactory;
            this._userService = userService;
            this._dateTimeHelper = dateTimeHelper;
            this._localizationService = localizationService;
            this._priceCalculationService = priceCalculationService;
            this._priceFormatter = priceFormatter;
            this._productAttributeFormatter = productAttributeFormatter;
            this._storeService = storeService;
            this._taxService = taxService;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Prepare shopping cart item search model
        /// </summary>
        /// <param name="searchModel">Shopping cart item search model</param>
        /// <returns>Shopping cart item search model</returns>
        protected virtual ShoppingCartItemSearchModel PrepareShoppingCartItemSearchModel(ShoppingCartItemSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare shopping cart search model
        /// </summary>
        /// <param name="searchModel">Shopping cart search model</param>
        /// <returns>Shopping cart search model</returns>
        public virtual ShoppingCartSearchModel PrepareShoppingCartSearchModel(ShoppingCartSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare available shopping cart types
            _baseAdminModelFactory.PrepareShoppingCartTypes(searchModel.AvailableShoppingCartTypes, false);

            //set default search values
            searchModel.ShoppingCartType = ShoppingCartType.ShoppingCart;

            //prepare nested search model
            PrepareShoppingCartItemSearchModel(searchModel.ShoppingCartItemSearchModel);

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        /// <summary>
        /// Prepare paged shopping cart list model
        /// </summary>
        /// <param name="searchModel">Shopping cart search model</param>
        /// <returns>Shopping cart list model</returns>
        public virtual ShoppingCartListModel PrepareShoppingCartListModel(ShoppingCartSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get users with shopping carts
            var users = _userService.GetAllUsers(loadOnlyWithShoppingCart: true,
                sct: searchModel.ShoppingCartType,
                pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize);

            //prepare list model
            var model = new ShoppingCartListModel
            {
                Data = users.Select(user =>
                {
                    //fill in model values from the entity
                    var shoppingCartModel = new ShoppingCartModel
                    {
                        UserId = user.Id
                    };

                    //fill in additional values (not existing in the entity)
                    shoppingCartModel.UserEmail = user.IsRegistered()
                        ? user.Email : _localizationService.GetResource("Admin.Users.Guest");
                    shoppingCartModel.TotalItems = user.ShoppingCartItems
                        .Where(item => item.ShoppingCartType == searchModel.ShoppingCartType).Sum(item => item.Quantity);

                    return shoppingCartModel;
                }),
                Total = users.TotalCount
            };

            return model;
        }

        /// <summary>
        /// Prepare paged shopping cart item list model
        /// </summary>
        /// <param name="searchModel">Shopping cart item search model</param>
        /// <param name="user">User</param>
        /// <returns>Shopping cart item list model</returns>
        public virtual ShoppingCartItemListModel PrepareShoppingCartItemListModel(ShoppingCartItemSearchModel searchModel, User user)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            //get shopping cart items
            var items = user.ShoppingCartItems.Where(item => item.ShoppingCartType == searchModel.ShoppingCartType).ToList();

            //prepare list model
            var model = new ShoppingCartItemListModel
            {
                Data = items.PaginationByRequestModel(searchModel).Select(item =>
                {
                    //fill in model values from the entity
                    var itemModel = new ShoppingCartItemModel
                    {
                        Id = item.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        ProductName = item.Product?.Name
                    };

                    //convert dates to the user time
                    itemModel.UpdatedOn = _dateTimeHelper.ConvertToUserTime(item.UpdatedOnUtc, DateTimeKind.Utc);

                    //fill in additional values (not existing in the entity)
                    itemModel.Store = _storeService.GetStoreById(item.StoreId)?.Name ?? "Deleted";
                    itemModel.AttributeInfo = _productAttributeFormatter.FormatAttributes(item.Product, item.AttributesXml, item.User);
                    var unitPrice = _priceCalculationService.GetUnitPrice(item);
                    itemModel.UnitPrice = _priceFormatter.FormatPrice(_taxService.GetProductPrice(item.Product, unitPrice, out var _));
                    var subTotal = _priceCalculationService.GetSubTotal(item);
                    itemModel.Total = _priceFormatter.FormatPrice(_taxService.GetProductPrice(item.Product, subTotal, out _));

                    return itemModel;
                }),
                Total = items.Count
            };

            return model;
        }

        #endregion
    }
}