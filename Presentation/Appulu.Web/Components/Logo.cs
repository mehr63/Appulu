using Microsoft.AspNetCore.Mvc;
using Appulu.Web.Factories;
using Appulu.Web.Framework.Components;

namespace Appulu.Web.Components
{
    public class LogoViewComponent : AppViewComponent
    {
        private readonly ICommonModelFactory _commonModelFactory;

        public LogoViewComponent(ICommonModelFactory commonModelFactory)
        {
            this._commonModelFactory = commonModelFactory;
        }

        public IViewComponentResult Invoke()
        {
            var model = _commonModelFactory.PrepareLogoModel();
            return View(model);
        }
    }
}
