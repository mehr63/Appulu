﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Appulu.Web.Framework.Mvc.Routing;

namespace Appulu.Plugin.Payments.Square.Infrastructure
{
    public partial class RouteProvider : IRouteProvider
    {
        /// <summary>
        /// Register routes
        /// </summary>
        /// <param name="routeBuilder">Route builder</param>
        public void RegisterRoutes(IRouteBuilder routeBuilder)
        {
            //add route for the access token callback
            routeBuilder.MapRoute(SquarePaymentDefaults.AccessTokenRoute, 
                "Plugins/PaymentSquare/AccessToken/", new { controller = "PaymentSquare", action = "AccessTokenCallback" });
        }

        /// <summary>
        /// Gets a priority of route provider
        /// </summary>
        public int Priority
        {
            get { return 0; }
        }
    }
}