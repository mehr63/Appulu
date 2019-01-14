using System;
using System.Linq;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using Appulu.Core;
using Appulu.Core.Caching;
using Appulu.Core.Configuration;
using Appulu.Core.Data;
using Appulu.Core.Domain;
using Appulu.Core.Domain.Security;
using Appulu.Core.Http;
using Appulu.Core.Infrastructure;
using Appulu.Core.Plugins;
using Appulu.Data;
using Appulu.Services.Authentication;
using Appulu.Services.Authentication.External;
using Appulu.Services.Logging;
using Appulu.Services.Security;
using Appulu.Services.Tasks;
using Appulu.Web.Framework.FluentValidation;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Mvc.Routing;
using Appulu.Web.Framework.Themes;
using StackExchange.Profiling.Storage;

namespace Appulu.Web.Framework.Infrastructure.Extensions
{
    /// <summary>
    /// Represents extensions of IServiceCollection
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add services to the application and configure service provider
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration of the application</param>
        /// <returns>Configured service provider</returns>
        public static IServiceProvider ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            //add AppConfig configuration parameters
            services.ConfigureStartupConfig<AppConfig>(configuration.GetSection("App"));
            //add hosting configuration parameters
            services.ConfigureStartupConfig<HostingConfig>(configuration.GetSection("Hosting"));
            //add accessor to HttpContext
            services.AddHttpContextAccessor();

            //create, initialize and configure the engine
            var engine = EngineContext.Create();
            engine.Initialize(services);
            var serviceProvider = engine.ConfigureServices(services, configuration);

            if (DataSettingsManager.DatabaseIsInstalled)
            {
                //implement schedule tasks
                //database is already installed, so start scheduled tasks
                TaskManager.Instance.Initialize();
                TaskManager.Instance.Start();

                //log application start
                EngineContext.Current.Resolve<ILogger>().Information("Application started", null, null);
            }

            return serviceProvider;
        }

        /// <summary>
        /// Create, bind and register as service the specified configuration parameters 
        /// </summary>
        /// <typeparam name="TConfig">Configuration parameters</typeparam>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Set of key/value application configuration properties</param>
        /// <returns>Instance of configuration parameters</returns>
        public static TConfig ConfigureStartupConfig<TConfig>(this IServiceCollection services, IConfiguration configuration) where TConfig : class, new()
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            //create instance of config
            var config = new TConfig();

            //bind it to the appropriate section of configuration
            configuration.Bind(config);

            //and register it as a service
            services.AddSingleton(config);

            return config;
        }

        /// <summary>
        /// Register HttpContextAccessor
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddHttpContextAccessor(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        /// <summary>
        /// Adds services required for anti-forgery support
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddAntiForgery(this IServiceCollection services)
        {
            //override cookie name
            services.AddAntiforgery(options =>
            {
                options.Cookie.Name = $"{AppCookieDefaults.Prefix}{AppCookieDefaults.AntiforgeryCookie}";

                //whether to allow the use of anti-forgery cookies from SSL protected page on the other store pages which are not
                options.Cookie.SecurePolicy = DataSettingsManager.DatabaseIsInstalled && EngineContext.Current.Resolve<SecuritySettings>().ForceSslForAllPages
                    ? CookieSecurePolicy.SameAsRequest : CookieSecurePolicy.None;
            });
        }

        /// <summary>
        /// Adds services required for application session state
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddHttpSession(this IServiceCollection services)
        {
            services.AddSession(options =>
            {
                options.Cookie.Name = $"{AppCookieDefaults.Prefix}{AppCookieDefaults.SessionCookie}";
                options.Cookie.HttpOnly = true;

                //whether to allow the use of session values from SSL protected page on the other store pages which are not
                options.Cookie.SecurePolicy = DataSettingsManager.DatabaseIsInstalled && EngineContext.Current.Resolve<SecuritySettings>().ForceSslForAllPages
                    ? CookieSecurePolicy.SameAsRequest : CookieSecurePolicy.None;
            });
        }

        /// <summary>
        /// Adds services required for themes support
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddThemes(this IServiceCollection services)
        {
            if (!DataSettingsManager.DatabaseIsInstalled)
                return;

            //themes support
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new ThemeableViewLocationExpander());
            });
        }

        /// <summary>
        /// Adds data protection services
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddAppDataProtection(this IServiceCollection services)
        {
            //check whether to persist data protection in Redis
            var appConfig = services.BuildServiceProvider().GetRequiredService<AppConfig>();
            if (appConfig.RedisCachingEnabled && appConfig.PersistDataProtectionKeysToRedis)
            {
                //store keys in Redis
                services.AddDataProtection().PersistKeysToRedis(() =>
                {
                    var redisConnectionWrapper = EngineContext.Current.Resolve<IRedisConnectionWrapper>();
                    return redisConnectionWrapper.GetDatabase();
                }, AppCachingDefaults.RedisDataProtectionKey);
            }
            else
            {
                var dataProtectionKeysPath = CommonHelper.DefaultFileProvider.MapPath("~/App_Data/DataProtectionKeys");
                var dataProtectionKeysFolder = new System.IO.DirectoryInfo(dataProtectionKeysPath);

                //configure the data protection system to persist keys to the specified directory
                services.AddDataProtection().PersistKeysToFileSystem(dataProtectionKeysFolder);
            }
        }

        /// <summary>
        /// Adds authentication service
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddAppAuthentication(this IServiceCollection services)
        {
            //set default authentication schemes
            var authenticationBuilder = services.AddAuthentication(options =>
            {
                options.DefaultScheme = AppAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = AppAuthenticationDefaults.ExternalAuthenticationScheme;
            });

            //add main cookie authentication
            authenticationBuilder.AddCookie(AppAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.Cookie.Name = $"{AppCookieDefaults.Prefix}{AppCookieDefaults.AuthenticationCookie}";
                options.Cookie.HttpOnly = true;
                options.LoginPath = AppAuthenticationDefaults.LoginPath;
                options.AccessDeniedPath = AppAuthenticationDefaults.AccessDeniedPath;

                //whether to allow the use of authentication cookies from SSL protected page on the other store pages which are not
                options.Cookie.SecurePolicy = DataSettingsManager.DatabaseIsInstalled && EngineContext.Current.Resolve<SecuritySettings>().ForceSslForAllPages
                    ? CookieSecurePolicy.SameAsRequest : CookieSecurePolicy.None;
            });

            //add external authentication
            authenticationBuilder.AddCookie(AppAuthenticationDefaults.ExternalAuthenticationScheme, options =>
            {
                options.Cookie.Name = $"{AppCookieDefaults.Prefix}{AppCookieDefaults.ExternalAuthenticationCookie}";
                options.Cookie.HttpOnly = true;
                options.LoginPath = AppAuthenticationDefaults.LoginPath;
                options.AccessDeniedPath = AppAuthenticationDefaults.AccessDeniedPath;

                //whether to allow the use of authentication cookies from SSL protected page on the other store pages which are not
                options.Cookie.SecurePolicy = DataSettingsManager.DatabaseIsInstalled && EngineContext.Current.Resolve<SecuritySettings>().ForceSslForAllPages
                    ? CookieSecurePolicy.SameAsRequest : CookieSecurePolicy.None;
            });

            //register and configure external authentication plugins now
            var typeFinder = new WebAppTypeFinder();
            var externalAuthConfigurations = typeFinder.FindClassesOfType<IExternalAuthenticationRegistrar>();
            var externalAuthInstances = externalAuthConfigurations
                .Where(x => PluginManager.FindPlugin(x)?.Installed ?? true) //ignore not installed plugins
                .Select(x => (IExternalAuthenticationRegistrar)Activator.CreateInstance(x));

            foreach (var instance in externalAuthInstances)
                instance.Configure(authenticationBuilder);
        }

        /// <summary>
        /// Add and configure MVC for the application
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <returns>A builder for configuring MVC services</returns>
        public static IMvcBuilder AddAppMvc(this IServiceCollection services)
        {
            //add basic MVC feature
            var mvcBuilder = services.AddMvc();

            //sets the default value of settings on MvcOptions to match the behavior of asp.net core mvc 2.1
            mvcBuilder.SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var appConfig = services.BuildServiceProvider().GetRequiredService<AppConfig>();
            if (appConfig.UseSessionStateTempDataProvider)
            {
                //use session-based temp data provider
                mvcBuilder.AddSessionStateTempDataProvider();
            }
            else
            {
                //use cookie-based temp data provider
                mvcBuilder.AddCookieTempDataProvider(options =>
                {
                    options.Cookie.Name = $"{AppCookieDefaults.Prefix}{AppCookieDefaults.TempDataCookie}";

                    //whether to allow the use of cookies from SSL protected page on the other store pages which are not
                    options.Cookie.SecurePolicy = DataSettingsManager.DatabaseIsInstalled && EngineContext.Current.Resolve<SecuritySettings>().ForceSslForAllPages
                        ? CookieSecurePolicy.SameAsRequest : CookieSecurePolicy.None;
                });
            }

            //MVC now serializes JSON with camel case names by default, use this code to avoid it
            mvcBuilder.AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            //add custom display metadata provider
            mvcBuilder.AddMvcOptions(options => options.ModelMetadataDetailsProviders.Add(new AppMetadataProvider()));

            //add custom model binder provider (to the top of the provider list)
            mvcBuilder.AddMvcOptions(options => options.ModelBinderProviders.Insert(0, new AppModelBinderProvider()));

            //add fluent validation
            mvcBuilder.AddFluentValidation(configuration =>
            {
                configuration.ValidatorFactoryType = typeof(AppValidatorFactory);
                //implicit/automatic validation of child properties
                configuration.ImplicitlyValidateChildProperties = true;
            });

            return mvcBuilder;
        }

        /// <summary>
        /// Register custom RedirectResultExecutor
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddAppRedirectResultExecutor(this IServiceCollection services)
        {
            //we use custom redirect executor as a workaround to allow using non-ASCII characters in redirect URLs
            services.AddSingleton<IActionResultExecutor<RedirectResult>, AppRedirectResultExecutor>();
        }

        /// <summary>
        /// Register base object context
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddAppObjectContext(this IServiceCollection services)
        {
            services.AddDbContext<AppObjectContext>(optionsBuilder =>
            {
                optionsBuilder.UseSqlServerWithLazyLoading(services);
            });
        }

        /// <summary>
        /// Add and configure MiniProfiler service
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddAppMiniProfiler(this IServiceCollection services)
        {
            //whether database is already installed
            if (!DataSettingsManager.DatabaseIsInstalled)
                return;

            services.AddMiniProfiler(miniProfilerOptions =>
            {
                //use memory cache provider for storing each result
                (miniProfilerOptions.Storage as MemoryCacheStorage).CacheDuration = TimeSpan.FromMinutes(60);

                //whether MiniProfiler should be displayed
                miniProfilerOptions.ShouldProfile = request =>
                    EngineContext.Current.Resolve<StoreInformationSettings>().DisplayMiniProfilerInPublicStore;

                //determine who can access the MiniProfiler results
                miniProfilerOptions.ResultsAuthorize = request =>
                    !EngineContext.Current.Resolve<StoreInformationSettings>().DisplayMiniProfilerForAdminOnly ||
                    EngineContext.Current.Resolve<IPermissionService>().Authorize(StandardPermissionProvider.AccessAdminPanel);
            }).AddEntityFramework();
        }
    }
}