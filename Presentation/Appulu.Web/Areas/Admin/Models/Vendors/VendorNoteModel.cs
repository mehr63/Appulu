using System;
using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Vendors
{
    /// <summary>
    /// Represents a vendor note model
    /// </summary>
    public partial class VendorNoteModel : BaseAppEntityModel
    {
        #region Properties

        public int VendorId { get; set; }

        [AppResourceDisplayName("Admin.Vendors.VendorNotes.Fields.Note")]
        public string Note { get; set; }

        [AppResourceDisplayName("Admin.Vendors.VendorNotes.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        #endregion
    }
}