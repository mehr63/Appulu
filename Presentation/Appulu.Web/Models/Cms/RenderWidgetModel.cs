using Appulu.Web.Framework.Models;

namespace Appulu.Web.Models.Cms
{
    public partial class RenderWidgetModel : BaseAppModel
    {
        public string WidgetViewComponentName { get; set; }
        public object WidgetViewComponentArguments { get; set; }
    }
}