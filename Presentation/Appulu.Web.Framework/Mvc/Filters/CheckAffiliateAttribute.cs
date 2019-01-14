using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Appulu.Core;
using Appulu.Core.Data;
using Appulu.Core.Domain.Affiliates;
using Appulu.Core.Domain.Users;
using Appulu.Services.Affiliates;
using Appulu.Services.Users;

namespace Appulu.Web.Framework.Mvc.Filters
{
    /// <summary>
    /// Represents filter attribute that checks and updates affiliate of user
    /// </summary>
    public class CheckAffiliateAttribute : TypeFilterAttribute
    {
        #region Ctor

        /// <summary>
        /// Create instance of the filter attribute
        /// </summary>
        public CheckAffiliateAttribute() : base(typeof(CheckAffiliateFilter))
        {
        }

        #endregion

        #region Nested filter

        /// <summary>
        /// Represents a filter that checks and updates affiliate of user
        /// </summary>
        private class CheckAffiliateFilter : IActionFilter
        {
            #region Constants

            private const string AFFILIATE_ID_QUERY_PARAMETER_NAME = "affiliateid";
            private const string AFFILIATE_FRIENDLYURLNAME_QUERY_PARAMETER_NAME = "affiliate";

            #endregion

            #region Fields

            private readonly IAffiliateService _affiliateService;
            private readonly IUserService _userService;
            private readonly IWorkContext _workContext;

            #endregion

            #region Ctor

            public CheckAffiliateFilter(IAffiliateService affiliateService, 
                IUserService userService,
                IWorkContext workContext)
            {
                this._affiliateService = affiliateService;
                this._userService = userService;
                this._workContext = workContext;
            }

            #endregion

            #region Utilities

            /// <summary>
            /// Set the affiliate identifier of current user
            /// </summary>
            /// <param name="affiliate">Affiliate</param>
            protected void SetUserAffiliateId(Affiliate affiliate)
            {
                if (affiliate == null || affiliate.Deleted || !affiliate.Active)
                    return;

                if (affiliate.Id == _workContext.CurrentUser.AffiliateId)
                    return;

                //ignore search engines
                if (_workContext.CurrentUser.IsSearchEngineAccount())
                    return;

                //update affiliate identifier
                _workContext.CurrentUser.AffiliateId = affiliate.Id;
                _userService.UpdateUser(_workContext.CurrentUser);
            }

            #endregion

            #region Methods

            /// <summary>
            /// Called before the action executes, after model binding is complete
            /// </summary>
            /// <param name="context">A context for action filters</param>
            public void OnActionExecuting(ActionExecutingContext context)
            {
                if (context == null)
                    throw new ArgumentNullException(nameof(context));

                //check request query parameters
                var request = context.HttpContext.Request;
                if (request?.Query == null || !request.Query.Any())
                    return;

                if (!DataSettingsManager.DatabaseIsInstalled)
                    return;

                //try to find by ID
                var affiliateIds = request.Query[AFFILIATE_ID_QUERY_PARAMETER_NAME];
                if (affiliateIds.Any() && int.TryParse(affiliateIds.FirstOrDefault(), out int affiliateId) && affiliateId > 0)
                {
                    SetUserAffiliateId(_affiliateService.GetAffiliateById(affiliateId));
                    return;
                }

                //try to find by friendly name
                var affiliateNames = request.Query[AFFILIATE_FRIENDLYURLNAME_QUERY_PARAMETER_NAME];
                if (affiliateNames.Any())
                {
                    var affiliateName = affiliateNames.FirstOrDefault();
                    if (!string.IsNullOrEmpty(affiliateName))
                        SetUserAffiliateId(_affiliateService.GetAffiliateByFriendlyUrlName(affiliateName));
                }
            }

            /// <summary>
            /// Called after the action executes, before the action result
            /// </summary>
            /// <param name="context">A context for action filters</param>
            public void OnActionExecuted(ActionExecutedContext context)
            {
                //do nothing
            }

            #endregion
        }

        #endregion
    }
}