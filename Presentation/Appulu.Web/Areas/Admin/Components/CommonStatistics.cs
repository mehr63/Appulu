using Microsoft.AspNetCore.Mvc;
using Appulu.Core;
using Appulu.Services.Security;
using Appulu.Web.Areas.Admin.Factories;
using Appulu.Web.Framework.Components;

namespace Appulu.Web.Areas.Admin.Components
{
    /// <summary>
    /// Represents a view component that displays common statistics
    /// </summary>
    public class CommonStatisticsViewComponent : AppViewComponent
    {
        #region Fields

        private readonly ICommonModelFactory _commonModelFactory;
        private readonly IPermissionService _permissionService;
        private readonly IWorkContext _workContext;

        #endregion

        #region Ctor

        public CommonStatisticsViewComponent(ICommonModelFactory commonModelFactory,
            IPermissionService permissionService,
            IWorkContext workContext)
        {
            this._commonModelFactory = commonModelFactory;
            this._permissionService = permissionService;
            this._workContext = workContext;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Invoke view component
        /// </summary>
        /// <returns>View component result</returns>
        public IViewComponentResult Invoke()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageUsers) ||
                !_permissionService.Authorize(StandardPermissionProvider.ManageOrders) ||
                !_permissionService.Authorize(StandardPermissionProvider.ManageReturnRequests) ||
                !_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
            {
                return Content(string.Empty);
            }

            //a vendor doesn't have access to this report
            if (_workContext.CurrentVendor != null)
                return Content(string.Empty);

            //prepare model
            var model = _commonModelFactory.PrepareCommonStatisticsModel();

            return View(model);
        }

        #endregion
    }
}