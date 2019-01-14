using System;
using FluentValidation.Attributes;
using Appulu.Web.Areas.Admin.Validators.Orders;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Orders
{
    /// <summary>
    /// Represents a return request model
    /// </summary>
    [Validator(typeof(ReturnRequestValidator))]
    public partial class ReturnRequestModel : BaseAppEntityModel
    {
        #region Properties

        [AppResourceDisplayName("Admin.ReturnRequests.Fields.CustomNumber")]
        public string CustomNumber { get; set; }
        
        public int OrderId { get; set; }

        [AppResourceDisplayName("Admin.ReturnRequests.Fields.CustomOrderNumber")]
        public string CustomOrderNumber { get; set; }

        [AppResourceDisplayName("Admin.ReturnRequests.Fields.User")]
        public int UserId { get; set; }

        [AppResourceDisplayName("Admin.ReturnRequests.Fields.User")]
        public string UserInfo { get; set; }

        public int ProductId { get; set; }

        [AppResourceDisplayName("Admin.ReturnRequests.Fields.Product")]
        public string ProductName { get; set; }

        public string AttributeInfo { get; set; }

        [AppResourceDisplayName("Admin.ReturnRequests.Fields.Quantity")]
        public int Quantity { get; set; }
        
        [AppResourceDisplayName("Admin.ReturnRequests.Fields.ReasonForReturn")]
        public string ReasonForReturn { get; set; }
        
        [AppResourceDisplayName("Admin.ReturnRequests.Fields.RequestedAction")]
        public string RequestedAction { get; set; }
        
        [AppResourceDisplayName("Admin.ReturnRequests.Fields.UserComments")]
        public string UserComments { get; set; }

        [AppResourceDisplayName("Admin.ReturnRequests.Fields.UploadedFile")]
        public Guid UploadedFileGuid { get; set; }
        
        [AppResourceDisplayName("Admin.ReturnRequests.Fields.StaffNotes")]
        public string StaffNotes { get; set; }

        [AppResourceDisplayName("Admin.ReturnRequests.Fields.Status")]
        public int ReturnRequestStatusId { get; set; }

        [AppResourceDisplayName("Admin.ReturnRequests.Fields.Status")]
        public string ReturnRequestStatusStr { get; set; }

        [AppResourceDisplayName("Admin.ReturnRequests.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        #endregion
    }
}