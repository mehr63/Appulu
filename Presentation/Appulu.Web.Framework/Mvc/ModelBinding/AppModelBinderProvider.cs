using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Appulu.Core.Infrastructure;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Framework.Mvc.ModelBinding
{
    /// <summary>
    /// Represents model binder provider for the creating AppModelBinder
    /// </summary>
    public class AppModelBinderProvider : IModelBinderProvider
    {
        /// <summary>
        /// Creates a app model binder based on passed context
        /// </summary>
        /// <param name="context">Model binder provider context</param>
        /// <returns>Model binder</returns>
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));


            var modelType = context.Metadata.ModelType;
            if (!typeof(BaseAppModel).IsAssignableFrom(modelType))
                return null;

            //use AppModelBinder as a ComplexTypeModelBinder for BaseAppModel
            if (context.Metadata.IsComplexType && !context.Metadata.IsCollectionType)
            {
                //create binders for all model properties
                var propertyBinders = context.Metadata.Properties
                    .ToDictionary(modelProperty => modelProperty, modelProperty => context.CreateBinder(modelProperty));
                
                return new AppModelBinder(propertyBinders, EngineContext.Current.Resolve<ILoggerFactory>());
            }

            //or return null to further search for a suitable binder
            return null;
        }
    }
}
