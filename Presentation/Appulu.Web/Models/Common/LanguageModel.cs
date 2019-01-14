using Appulu.Web.Framework.Models;

namespace Appulu.Web.Models.Common
{
    public partial class LanguageModel : BaseAppEntityModel
    {
        public string Name { get; set; }

        public string FlagImageFileName { get; set; }
    }
}