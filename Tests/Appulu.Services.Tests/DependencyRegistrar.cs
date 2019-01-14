using Autofac;
using Appulu.Core.Caching;
using Appulu.Core.Configuration;
using Appulu.Core.Infrastructure;
using Appulu.Core.Infrastructure.DependencyManagement;

namespace Appulu.Services.Tests
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
            //cache managers
            builder.RegisterType<AppNullCache>().As<ICacheManager>().Named<ICacheManager>("app_cache_static").SingleInstance();

        }

        /// <summary>
        /// Order of this dependency registrar implementation
        /// </summary>
        public int Order
        {
            get { return 0; }
        }
    }

}
