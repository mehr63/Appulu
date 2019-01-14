using Appulu.Web.Framework.Models;

namespace Appulu.Web.Models.Common
{
    public partial class AdminHeaderLinksModel : BaseAppModel
    {
        public string ImpersonatedUserName { get; set; }
        public bool IsUserImpersonated { get; set; }
        public bool DisplayAdminLink { get; set; }
        public string EditPageUrl { get; set; }
    }
}