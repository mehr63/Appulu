using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Settings
{
    /// <summary>
    /// Represents a setting mode model
    /// </summary>
    public partial class SettingModeModel : BaseAppModel
    {
        #region Properties

        public string ModeName { get; set; }

        public bool Enabled { get; set; }

        #endregion
    }
}