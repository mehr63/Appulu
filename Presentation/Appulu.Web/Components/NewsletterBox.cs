using Microsoft.AspNetCore.Mvc;
using Appulu.Core.Domain.Users;
using Appulu.Web.Factories;
using Appulu.Web.Framework.Components;

namespace Appulu.Web.Components
{
    public class NewsletterBoxViewComponent : AppViewComponent
    {
        private readonly UserSettings _userSettings;
        private readonly INewsletterModelFactory _newsletterModelFactory;

        public NewsletterBoxViewComponent(UserSettings userSettings, INewsletterModelFactory newsletterModelFactory)
        {
            this._userSettings = userSettings;
            this._newsletterModelFactory = newsletterModelFactory;
        }

        public IViewComponentResult Invoke()
        {
            if (_userSettings.HideNewsletterBlock)
                return Content("");

            var model = _newsletterModelFactory.PrepareNewsletterBoxModel();
            return View(model);
        }
    }
}
