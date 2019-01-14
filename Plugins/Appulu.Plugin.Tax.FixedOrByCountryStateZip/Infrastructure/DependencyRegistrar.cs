using Autofac;
using Autofac.Core;
using Appulu.Core.Configuration;
using Appulu.Core.Data;
using Appulu.Core.Infrastructure;
using Appulu.Core.Infrastructure.DependencyManagement;
using Appulu.Data;
using Appulu.Plugin.Tax.FixedOrByCountryStateZip.Data;
using Appulu.Plugin.Tax.FixedOrByCountryStateZip.Domain;
using Appulu.Plugin.Tax.FixedOrByCountryStateZip.Services;
using Appulu.Services.Tax;
using Appulu.Web.Framework.Infrastructure.Extensions;

namespace Appulu.Plugin.Tax.FixedOrByCountryStateZip.Infrastructure
{
    /// <summary>
    /// Dependency registrar
    /// </summary>
    public class DependencyRegistrar : IDependencyRegistrar
    {
        /// <summary>
        /// Register services and interfaces
        /// </summary>
        /// <param name="builder">Container builder</param>
        /// <param name="typeFinder">Type finder</param>
        /// <param name="config">Config</param>
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, AppConfig config)
        {
            builder.RegisterType<FixedOrByCountryStateZipTaxProvider>().As<ITaxProvider>().InstancePerLifetimeScope();
            builder.RegisterType<CountryStateZipService>().As<ICountryStateZipService>().InstancePerLifetimeScope();

            //data context
            builder.RegisterPluginDataContext<CountryStateZipObjectContext>("app_object_context_tax_country_state_zip");

            //override required repository with our custom context
            builder.RegisterType<EfRepository<TaxRate>>().As<IRepository<TaxRate>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>("app_object_context_tax_country_state_zip"))
                .InstancePerLifetimeScope();
        }

        /// <summary>
        /// Order of this dependency registrar implementation
        /// </summary>
        public int Order => 1;
    }
}