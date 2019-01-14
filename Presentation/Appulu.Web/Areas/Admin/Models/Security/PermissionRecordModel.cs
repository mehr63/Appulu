using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Security
{
    /// <summary>
    /// Represents a permission record model
    /// </summary>
    public partial class PermissionRecordModel : BaseAppModel
    {
        #region Properties

        public string Name { get; set; }

        public string SystemName { get; set; }

        #endregion
    }
}