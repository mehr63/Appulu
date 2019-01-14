using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Common
{
    /// <summary>
    /// Represents a backup file model
    /// </summary>
    public partial class BackupFileModel : BaseAppModel
    {
        #region Properties
        
        public string Name { get; set; }
        
        public string Length { get; set; }
        
        public string Link { get; set; }
        
        #endregion
    }
}