using System.Collections.Generic;
using Appulu.Web.Models.Cms;

namespace Appulu.Web.Factories
{
    /// <summary>
    /// Represents the interface of the widget model factory
    /// </summary>
    public partial interface IWidgetModelFactory
    {
        /// <summary>
        /// Get render the widget models
        /// </summary>
        /// <param name="widgetZone">Name of widget zone</param>
        /// <param name="additionalData">Additional data object</param>
        /// <returns>List of the render widget models</returns>
        List<RenderWidgetModel> PrepareRenderWidgetModel(string widgetZone, object additionalData = null);
    }
}