using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Appulu.Core;
using Appulu.Core.Domain.Orders;
using Appulu.Services.Orders;
using Appulu.Web.Factories;
using Appulu.Web.Framework.Components;

namespace Appulu.Web.Components
{
    public class EstimateShippingViewComponent : AppViewComponent
    {
        private readonly IShoppingCartModelFactory _shoppingCartModelFactory;
        private readonly IStoreContext _storeContext;
        private readonly IWorkContext _workContext;

        public EstimateShippingViewComponent(IShoppingCartModelFactory shoppingCartModelFactory,
            IStoreContext storeContext,
            IWorkContext workContext)
        {
            this._shoppingCartModelFactory = shoppingCartModelFactory;
            this._storeContext = storeContext;
            this._workContext = workContext;
        }

        public IViewComponentResult Invoke(bool? prepareAndDisplayOrderReviewData)
        {
            var cart = _workContext.CurrentUser.ShoppingCartItems
                .Where(sci => sci.ShoppingCartType == ShoppingCartType.ShoppingCart)
                .LimitPerStore(_storeContext.CurrentStore.Id)
                .ToList();

            var model = _shoppingCartModelFactory.PrepareEstimateShippingModel(cart);
            if (!model.Enabled)
                return Content("");

            return View(model);
        }
    }
}
