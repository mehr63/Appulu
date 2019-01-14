using Microsoft.AspNetCore.Mvc;
using Appulu.Web.Factories;
using Appulu.Web.Framework.Components;

namespace Appulu.Web.Components
{
    public class SocialButtonsViewComponent : AppViewComponent
    {
        private readonly ICommonModelFactory _commonModelFactory;

        public SocialButtonsViewComponent(ICommonModelFactory commonModelFactory)
        {
            this._commonModelFactory = commonModelFactory;
        }

        public IViewComponentResult Invoke()
        {
            var model = _commonModelFactory.PrepareSocialModel();
            return View(model);
        }
    }
}
