using Microsoft.AspNetCore.Mvc;
using Appulu.Web.Areas.Admin.Factories;
using Appulu.Web.Framework.Components;

namespace Appulu.Web.Areas.Admin.Components
{
    /// <summary>
    /// Represents a view component that displays the Appulu news
    /// </summary>
    public class AppuluNewsViewComponent : AppViewComponent
    {
        #region Fields

        private readonly IHomeModelFactory _homeModelFactory;

        #endregion

        #region Ctor

        public AppuluNewsViewComponent(IHomeModelFactory homeModelFactory)
        {
            this._homeModelFactory = homeModelFactory;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Invoke view component
        /// </summary>
        /// <returns>View component result</returns>
        public IViewComponentResult Invoke()
        {
            try
            {
                //prepare model
                var model = _homeModelFactory.PrepareAppuluNewsModel();

                return View(model);
            }
            catch
            {
                return Content(string.Empty);
            }
        }

        #endregion
    }
}