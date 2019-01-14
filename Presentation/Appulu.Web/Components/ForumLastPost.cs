using Microsoft.AspNetCore.Mvc;
using Appulu.Services.Forums;
using Appulu.Web.Factories;
using Appulu.Web.Framework.Components;

namespace Appulu.Web.Components
{
    public class ForumLastPostViewComponent : AppViewComponent
    {
        private readonly IForumModelFactory _forumModelFactory;
        private readonly IForumService _forumService;

        public ForumLastPostViewComponent(IForumModelFactory forumModelFactory, IForumService forumService)
        {
            this._forumModelFactory = forumModelFactory;
            this._forumService = forumService;
        }

        public IViewComponentResult Invoke(int forumPostId, bool showTopic)
        {
            var forumPost = _forumService.GetPostById(forumPostId);
            var model = _forumModelFactory.PrepareLastPostModel(forumPost, showTopic);

            return View(model);
        }
    }
}
