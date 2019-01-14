using Microsoft.AspNetCore.Mvc;
using Appulu.Web.Framework.Components;

namespace Appulu.Plugin.ExternalAuth.Facebook.Components
{
    [ViewComponent(Name = FacebookAuthenticationDefaults.ViewComponentName)]
    public class FacebookAuthenticationViewComponent : AppViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("~/Plugins/ExternalAuth.Facebook/Views/PublicInfo.cshtml");
        }
    }
}