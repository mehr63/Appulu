using System.ComponentModel.DataAnnotations;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Orders
{
    /// <summary>
    /// Represents an upload license model
    /// </summary>
    public partial class UploadLicenseModel : BaseAppModel
    {
        #region Properties

        public int OrderId { get; set; }

        public int OrderItemId { get; set; }

        [UIHint("Download")]
        public int LicenseDownloadId { get; set; }

        #endregion
    }
}