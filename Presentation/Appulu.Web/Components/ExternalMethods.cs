using Microsoft.AspNetCore.Mvc;
using Appulu.Web.Factories;
using Appulu.Web.Framework.Components;

namespace Appulu.Web.Components
{
    public class ExternalMethodsViewComponent : AppViewComponent
    {
        #region Fields

        private readonly IExternalAuthenticationModelFactory _externalAuthenticationModelFactory;

        #endregion

        #region Ctor

        public ExternalMethodsViewComponent(IExternalAuthenticationModelFactory externalAuthenticationModelFactory)
        {
            this._externalAuthenticationModelFactory = externalAuthenticationModelFactory;
        }

        #endregion

        #region Methods

        public IViewComponentResult Invoke()
        {
            var model = _externalAuthenticationModelFactory.PrepareExternalMethodsModel();

            return View(model);
        }

        #endregion
    }
}
