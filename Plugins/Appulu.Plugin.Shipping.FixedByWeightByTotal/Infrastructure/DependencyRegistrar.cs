using Autofac;
using Autofac.Core;
using Appulu.Core.Configuration;
using Appulu.Core.Data;
using Appulu.Core.Infrastructure;
using Appulu.Core.Infrastructure.DependencyManagement;
using Appulu.Data;
using Appulu.Plugin.Shipping.FixedByWeightByTotal.Data;
using Appulu.Plugin.Shipping.FixedByWeightByTotal.Domain;
using Appulu.Plugin.Shipping.FixedByWeightByTotal.Services;
using Appulu.Web.Framework.Infrastructure.Extensions;

namespace Appulu.Plugin.Shipping.FixedByWeightByTotal.Infrastructure
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
            builder.RegisterType<ShippingByWeightByTotalService>().As<IShippingByWeightByTotalService>().InstancePerLifetimeScope();

            //data context
            builder.RegisterPluginDataContext<ShippingByWeightByTotalObjectContext>("app_object_context_shipping_weight_total_zip");

            //override required repository with our custom context
            builder.RegisterType<EfRepository<ShippingByWeightByTotalRecord>>().As<IRepository<ShippingByWeightByTotalRecord>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>("app_object_context_shipping_weight_total_zip"))
                .InstancePerLifetimeScope();
        }

        /// <summary>
        /// Order of this dependency registrar implementation
        /// </summary>
        public int Order => 1;
    }
}