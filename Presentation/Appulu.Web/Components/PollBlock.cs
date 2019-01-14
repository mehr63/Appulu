using Microsoft.AspNetCore.Mvc;
using Appulu.Web.Factories;
using Appulu.Web.Framework.Components;

namespace Appulu.Web.Components
{
    public class PollBlockViewComponent : AppViewComponent
    {
        private readonly IPollModelFactory _pollModelFactory;

        public PollBlockViewComponent(IPollModelFactory pollModelFactory)
        {
            this._pollModelFactory = pollModelFactory;
        }

        public IViewComponentResult Invoke(string systemKeyword)
        {

            if (string.IsNullOrWhiteSpace(systemKeyword))
                return Content("");

            var model = _pollModelFactory.PreparePollModelBySystemName(systemKeyword);
            if (model == null)
                return Content("");

            return View(model);
        }
    }
}
