using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Appulu.Web.Factories;
using Appulu.Web.Framework.Components;

namespace Appulu.Web.Components
{
    public class ForumActiveDiscussionsSmallViewComponent : AppViewComponent
    {
        private readonly IForumModelFactory _forumModelFactory;

        public ForumActiveDiscussionsSmallViewComponent(IForumModelFactory forumModelFactory)
        {
            this._forumModelFactory = forumModelFactory;
        }

        public IViewComponentResult Invoke()
        {
            var model = _forumModelFactory.PrepareActiveDiscussionsModel();
            if (!model.ForumTopics.Any())
                return Content("");

            return View(model);
        }
    }
}
