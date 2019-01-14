using Appulu.Web.Framework.Models;

namespace Appulu.Web.Models.User
{
    public partial class GdprConsentModel : BaseAppEntityModel
    {
        public string Message { get; set; }

        public bool IsRequired { get; set; }

        public string RequiredMessage { get; set; }

        public bool Accepted { get; set; }
    }
}