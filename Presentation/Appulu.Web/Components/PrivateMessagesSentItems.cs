using Microsoft.AspNetCore.Mvc;
using Appulu.Web.Factories;
using Appulu.Web.Framework.Components;

namespace Appulu.Web.Components
{
    public class PrivateMessagesSentItemsViewComponent : AppViewComponent
    {
        private readonly IPrivateMessagesModelFactory _privateMessagesModelFactory;

        public PrivateMessagesSentItemsViewComponent(IPrivateMessagesModelFactory privateMessagesModelFactory)
        {
            this._privateMessagesModelFactory = privateMessagesModelFactory;
        }

        public IViewComponentResult Invoke(int pageNumber, string tab)
        {
            var model = _privateMessagesModelFactory.PrepareSentModel(pageNumber, tab);
            return View(model);
        }
    }
}
