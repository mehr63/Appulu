using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Appulu.Web.Factories;
using Appulu.Web.Framework.Components;

namespace Appulu.Web.Components
{
    public class HomepagePollsViewComponent : AppViewComponent
    {
        private readonly IPollModelFactory _pollModelFactory;

        public HomepagePollsViewComponent(IPollModelFactory pollModelFactory)
        {
            this._pollModelFactory = pollModelFactory;
        }

        public IViewComponentResult Invoke()
        {
            var model = _pollModelFactory.PrepareHomePagePollModels();
            if (!model.Any())
                return Content("");

            return View(model);
        }
    }
}
