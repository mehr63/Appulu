using Microsoft.AspNetCore.Mvc;
using Appulu.Web.Factories;
using Appulu.Web.Framework.Components;

namespace Appulu.Web.Components
{
    public class UserNavigationViewComponent : AppViewComponent
    {
        private readonly IUserModelFactory _userModelFactory;

        public UserNavigationViewComponent(IUserModelFactory userModelFactory)
        {
            this._userModelFactory = userModelFactory;
        }

        public IViewComponentResult Invoke(int selectedTabId = 0)
        {
            var model = _userModelFactory.PrepareUserNavigationModel(selectedTabId);
            return View(model);
        }
    }
}
