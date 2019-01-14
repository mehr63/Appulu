using System;
using Microsoft.AspNetCore.Mvc;
using Appulu.Core;
using Appulu.Core.Domain;
using Appulu.Core.Domain.Users;
using Appulu.Services.Common;
using Appulu.Web.Framework.Components;

namespace Appulu.Web.Components
{
    public class EuCookieLawViewComponent : AppViewComponent
    {
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IStoreContext _storeContext;
        private readonly IWorkContext _workContext;
        private readonly StoreInformationSettings _storeInformationSettings;

        public EuCookieLawViewComponent(IGenericAttributeService genericAttributeService,
            IStoreContext storeContext,
            IWorkContext workContext,
            StoreInformationSettings storeInformationSettings)
        {
            this._genericAttributeService = genericAttributeService;
            this._storeContext = storeContext;
            this._workContext = workContext;
            this._storeInformationSettings = storeInformationSettings;
        }

        public IViewComponentResult Invoke()
        {
            if (!_storeInformationSettings.DisplayEuCookieLawWarning)
                //disabled
                return Content("");

            //ignore search engines because some pages could be indexed with the EU cookie as description
            if (_workContext.CurrentUser.IsSearchEngineAccount())
                return Content("");

            if (_genericAttributeService.GetAttribute<bool>(_workContext.CurrentUser, AppUserDefaults.EuCookieLawAcceptedAttribute, _storeContext.CurrentStore.Id))
                //already accepted
                return Content("");

            //ignore notification?
            //right now it's used during logout so popup window is not displayed twice
            if (TempData["app.IgnoreEuCookieLawWarning"] != null && Convert.ToBoolean(TempData["app.IgnoreEuCookieLawWarning"]))
                return Content("");

            return View();
        }
    }
}