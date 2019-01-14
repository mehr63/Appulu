using System;
using Microsoft.AspNetCore.Mvc;
using Appulu.Services.Users;
using Appulu.Services.Orders;
using Appulu.Services.Security;
using Appulu.Web.Areas.Admin.Factories;
using Appulu.Web.Areas.Admin.Models.ShoppingCart;
using Appulu.Web.Framework.Mvc;

namespace Appulu.Web.Areas.Admin.Controllers
{
    public partial class ShoppingCartController : BaseAdminController
    {
        #region Fields

        private readonly IUserService _userService;
        private readonly IPermissionService _permissionService;
        private readonly IShoppingCartModelFactory _shoppingCartModelFactory;
        private readonly IShoppingCartService _shoppingCartService;

        #endregion

        #region Ctor

        public ShoppingCartController(IUserService userService,
            IPermissionService permissionService, 
            IShoppingCartService shoppingCartService,
            IShoppingCartModelFactory shoppingCartModelFactory)
        {
            this._userService = userService;
            this._permissionService = permissionService;
            this._shoppingCartModelFactory = shoppingCartModelFactory;
            this._shoppingCartService = shoppingCartService;
        }

        #endregion
        
        #region Methods
        
        public virtual IActionResult CurrentCarts()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCurrentCarts))
                return AccessDeniedView();

            //prepare model
            var model = _shoppingCartModelFactory.PrepareShoppingCartSearchModel(new ShoppingCartSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult CurrentCarts(ShoppingCartSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCurrentCarts))
                return AccessDeniedKendoGridJson();

            //prepare model
            var model = _shoppingCartModelFactory.PrepareShoppingCartListModel(searchModel);

            return Json(model);
        }

        [HttpPost]
        public virtual IActionResult GetCartDetails(ShoppingCartItemSearchModel searchModel)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCurrentCarts))
                return AccessDeniedKendoGridJson();

            //try to get a user with the specified id
            var user = _userService.GetUserById(searchModel.UserId)
                ?? throw new ArgumentException("No user found with the specified id");

            //prepare model
            var model = _shoppingCartModelFactory.PrepareShoppingCartItemListModel(searchModel, user);

            return Json(model);
        }
        
        [HttpPost]
        public virtual IActionResult DeleteItem(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCurrentCarts))
                return AccessDeniedKendoGridJson();
            
            _shoppingCartService.DeleteShoppingCartItem(id);

            return new NullJsonResult();
        }

        #endregion
    }
}