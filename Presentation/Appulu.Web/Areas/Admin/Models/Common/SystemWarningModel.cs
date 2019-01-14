using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Common
{
    public partial class SystemWarningModel : BaseAppModel
    {
        public SystemWarningLevel Level { get; set; }

        public string Text { get; set; }

        public bool DontEncode { get; set; }
    }

    public enum SystemWarningLevel
    {
        Pass,
        Recommendation,
        CopyrightRemovalKey,
        Warning,
        Fail
    }
}