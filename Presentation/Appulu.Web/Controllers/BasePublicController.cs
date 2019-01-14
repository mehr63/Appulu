using Microsoft.AspNetCore.Mvc;
using Appulu.Web.Framework.Controllers;
using Appulu.Web.Framework.Mvc.Filters;
using Appulu.Web.Framework.Security;

namespace Appulu.Web.Controllers
{
    [HttpsRequirement(SslRequirement.NoMatter)]
    [WwwRequirement]
    [CheckAccessPublicStore]
    [CheckAccessClosedStore]
    [CheckLanguageSeoCode]
    [CheckDiscountCoupon]
    [CheckAffiliate]
    public abstract partial class BasePublicController : BaseController
    {
        protected virtual IActionResult InvokeHttp404()
        {
            Response.StatusCode = 404;
            return new EmptyResult();
        }
    }
}