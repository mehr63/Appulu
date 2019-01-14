﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Appulu.Core.Infrastructure;
using Appulu.Web.Framework.Infrastructure.Extensions;

namespace Appulu.Web.Framework.Infrastructure
{
    /// <summary>
    /// Represents object for the configuring authentication middleware on application startup
    /// </summary>
    public class AuthenticationStartup : IAppStartup
    {
        /// <summary>
        /// Add and configure any of the middleware
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration of the application</param>
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            //add data protection
            services.AddAppDataProtection();

            //add authentication
            services.AddAppAuthentication();
        }

        /// <summary>
        /// Configure the using of added middleware
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public void Configure(IApplicationBuilder application)
        {
            //configure authentication
            application.UseAppAuthentication();

            //set request culture
            application.UseCulture();
        }

        /// <summary>
        /// Gets order of this startup configuration implementation
        /// </summary>
        public int Order
        {
            //authentication should be loaded before MVC
            get { return 500; }
        }
    }
}
