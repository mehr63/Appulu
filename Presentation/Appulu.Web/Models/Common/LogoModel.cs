using Appulu.Web.Framework.Models;

namespace Appulu.Web.Models.Common
{
    public partial class LogoModel : BaseAppModel
    {
        public string StoreName { get; set; }

        public string LogoPath { get; set; }
    }
}