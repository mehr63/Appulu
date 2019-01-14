﻿using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Appulu.Web.Areas.Admin.Factories;
using Appulu.Web.Framework.Components;

namespace Appulu.Web.Areas.Admin.Components
{
    /// <summary>
    /// Represents a view component that displays an admin widgets
    /// </summary>
    public class AdminWidgetViewComponent : AppViewComponent
    {
        #region Fields

        private readonly IWidgetModelFactory _widgetModelFactory;

        #endregion

        #region Ctor

        public AdminWidgetViewComponent(IWidgetModelFactory widgetModelFactory)
        {
            this._widgetModelFactory = widgetModelFactory;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Invoke view component
        /// </summary>
        /// <param name="widgetZone">Widget zone name</param>
        /// <param name="additionalData">Additional data</param>
        /// <returns>View component result</returns>
        public IViewComponentResult Invoke(string widgetZone, object additionalData = null)
        {
            //prepare model
            var models = _widgetModelFactory.PrepareRenderWidgetModels(widgetZone, additionalData);

            //no data?
            if (!models.Any())
                return Content(string.Empty);

            return View(models);
        }

        #endregion
    }
}