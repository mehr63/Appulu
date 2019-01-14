using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Appulu.Core;
using Appulu.Core.Domain.Orders;
using Appulu.Services.Orders;
using Appulu.Web.Factories;
using Appulu.Web.Framework.Components;

namespace Appulu.Web.Components
{
    public class OrderTotalsViewComponent : AppViewComponent
    {
        private readonly IShoppingCartModelFactory _shoppingCartModelFactory;
        private readonly IStoreContext _storeContext;
        private readonly IWorkContext _workContext;

        public OrderTotalsViewComponent(IShoppingCartModelFactory shoppingCartModelFactory,
            IStoreContext storeContext,
            IWorkContext workContext)
        {
            this._shoppingCartModelFactory = shoppingCartModelFactory;
            this._storeContext = storeContext;
            this._workContext = workContext;
        }

        public IViewComponentResult Invoke(bool isEditable)
        {
            var cart = _workContext.CurrentUser.ShoppingCartItems
                .Where(sci => sci.ShoppingCartType == ShoppingCartType.ShoppingCart)
                .LimitPerStore(_storeContext.CurrentStore.Id)
                .ToList();

            var model = _shoppingCartModelFactory.PrepareOrderTotalsModel(cart, isEditable);
            return View(model);
        }
    }
}
