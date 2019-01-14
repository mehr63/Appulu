using Microsoft.AspNetCore.Mvc;
using Appulu.Web.Framework.Mvc.Filters;
using Appulu.Web.Framework.Security;

namespace Appulu.Web.Controllers
{
    public partial class HomeController : BasePublicController
    {
        [HttpsRequirement(SslRequirement.No)]
        public virtual IActionResult Index()
        {
            return View();
        }
    }
}