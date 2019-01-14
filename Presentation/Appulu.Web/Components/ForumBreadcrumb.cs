using Microsoft.AspNetCore.Mvc;
using Appulu.Web.Factories;
using Appulu.Web.Framework.Components;

namespace Appulu.Web.Components
{
    public class ForumBreadcrumbViewComponent : AppViewComponent
    {
        private readonly IForumModelFactory _forumModelFactory;

        public ForumBreadcrumbViewComponent(IForumModelFactory forumModelFactory)
        {
            this._forumModelFactory = forumModelFactory;
        }

        public IViewComponentResult Invoke(int? forumGroupId, int? forumId, int? forumTopicId)
        {
            var model = _forumModelFactory.PrepareForumBreadcrumbModel(forumGroupId, forumId, forumTopicId);
            return View(model);
        }
    }
}
