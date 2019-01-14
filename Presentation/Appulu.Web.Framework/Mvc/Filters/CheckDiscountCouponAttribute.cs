using System;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Appulu.Core;
using Appulu.Core.Data;
using Appulu.Core.Domain.Users;
using Appulu.Services.Users;
using Appulu.Services.Discounts;

namespace Appulu.Web.Framework.Mvc.Filters
{
    /// <summary>
    /// Represents filter attribute that checks and applied discount coupon code to user
    /// </summary>
    public class CheckDiscountCouponAttribute : TypeFilterAttribute
    {
        #region Ctor

        /// <summary>
        /// Create instance of the filter attribute
        /// </summary>
        public CheckDiscountCouponAttribute() : base(typeof(CheckDiscountCouponFilter))
        {
        }

        #endregion

        #region Nested filter

        /// <summary>
        /// Represents a filter that checks and applied discount coupon code to user
        /// </summary>
        private class CheckDiscountCouponFilter : IActionFilter
        {
            #region Fields

            private readonly IUserService _userService;
            private readonly IDiscountService _discountService;
            private readonly IWorkContext _workContext;

            #endregion

            #region Ctor

            public CheckDiscountCouponFilter(IUserService userService,
                IDiscountService discountService,
                IWorkContext workContext)
            {
                this._userService = userService;
                this._discountService = discountService;
                this._workContext = workContext;
            }

            #endregion

            #region Methods

            /// <summary>
            /// Called before the action executes, after model binding is complete
            /// </summary>
            /// <param name="context">A context for action filters</param>
            public void OnActionExecuting(ActionExecutingContext context)
            {
                if (context == null)
                    throw new ArgumentNullException(nameof(context));

                //check request query parameters
                if (!context.HttpContext.Request?.Query?.Any() ?? true)
                    return;

                //only in GET requests
                if (!context.HttpContext.Request.Method.Equals(WebRequestMethods.Http.Get, StringComparison.InvariantCultureIgnoreCase))
                    return;

                if (!DataSettingsManager.DatabaseIsInstalled)
                    return;

                //ignore search engines
                if (_workContext.CurrentUser.IsSearchEngineAccount())
                    return;

                //try to get discount coupon code
                var queryKey = AppDiscountDefaults.DiscountCouponQueryParameter;
                if (!context.HttpContext.Request.Query.TryGetValue(queryKey, out StringValues couponCodes) || StringValues.IsNullOrEmpty(couponCodes))
                    return;

                //get validated discounts with passed coupon codes
                var discounts = couponCodes
                    .SelectMany(couponCode => _discountService.GetAllDiscountsForCaching(couponCode: couponCode))
                    .Distinct()
                    .Where(discount => _discountService.ValidateDiscount(discount, _workContext.CurrentUser, couponCodes.ToArray()).IsValid)
                    .ToList();

                //apply discount coupon codes to user
                discounts.ForEach(discount => _userService.ApplyDiscountCouponCode(_workContext.CurrentUser, discount.CouponCode));
            }

            /// <summary>
            /// Called after the action executes, before the action result
            /// </summary>
            /// <param name="context">A context for action filters</param>
            public void OnActionExecuted(ActionExecutedContext context)
            {
                //do nothing
            }

            #endregion
        }

        #endregion
    }
}