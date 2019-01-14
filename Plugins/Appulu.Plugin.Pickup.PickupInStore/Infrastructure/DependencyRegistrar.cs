using Autofac;
using Autofac.Core;
using Appulu.Core.Configuration;
using Appulu.Core.Data;
using Appulu.Core.Infrastructure;
using Appulu.Core.Infrastructure.DependencyManagement;
using Appulu.Data;
using Appulu.Plugin.Pickup.PickupInStore.Data;
using Appulu.Plugin.Pickup.PickupInStore.Domain;
using Appulu.Plugin.Pickup.PickupInStore.Services;
using Appulu.Web.Framework.Infrastructure.Extensions;

namespace Appulu.Plugin.Pickup.PickupInStore.Infrastructure
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
            builder.RegisterType<StorePickupPointService>().As<IStorePickupPointService>().InstancePerLifetimeScope();

            //data context
            builder.RegisterPluginDataContext<StorePickupPointObjectContext>("app_object_context_pickup_in_store-pickup");

            //override required repository with our custom context
            builder.RegisterType<EfRepository<StorePickupPoint>>().As<IRepository<StorePickupPoint>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>("app_object_context_pickup_in_store-pickup"))
                .InstancePerLifetimeScope();
        }

        /// <summary>
        /// Order of this dependency registrar implementation
        /// </summary>
        public int Order => 1;
    }
}