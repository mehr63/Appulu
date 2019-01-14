using System;
using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Orders
{
    /// <summary>
    /// Represents an order note model
    /// </summary>
    public partial class OrderNoteModel : BaseAppEntityModel
    {
        #region Properties

        public int OrderId { get; set; }

        [AppResourceDisplayName("Admin.Orders.OrderNotes.Fields.DisplayToUser")]
        public bool DisplayToUser { get; set; }

        [AppResourceDisplayName("Admin.Orders.OrderNotes.Fields.Note")]
        public string Note { get; set; }

        [AppResourceDisplayName("Admin.Orders.OrderNotes.Fields.Download")]
        public int DownloadId { get; set; }

        [AppResourceDisplayName("Admin.Orders.OrderNotes.Fields.Download")]
        public Guid DownloadGuid { get; set; }

        [AppResourceDisplayName("Admin.Orders.OrderNotes.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        #endregion
    }
}