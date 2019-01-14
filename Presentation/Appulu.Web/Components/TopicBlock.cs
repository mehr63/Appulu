using Microsoft.AspNetCore.Mvc;
using Appulu.Web.Factories;
using Appulu.Web.Framework.Components;

namespace Appulu.Web.Components
{
    public class TopicBlockViewComponent : AppViewComponent
    {
        private readonly ITopicModelFactory _topicModelFactory;

        public TopicBlockViewComponent(ITopicModelFactory topicModelFactory)
        {
            this._topicModelFactory = topicModelFactory;
        }

        public IViewComponentResult Invoke(string systemName)
        {
            var model = _topicModelFactory.PrepareTopicModelBySystemName(systemName);
            if (model == null)
                return Content("");
            return View(model);
        }
    }
}
