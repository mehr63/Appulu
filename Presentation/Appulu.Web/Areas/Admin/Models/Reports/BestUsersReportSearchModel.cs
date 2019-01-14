using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Reports
{
    /// <summary>
    /// Represents a best users report search model
    /// </summary>
    public partial class BestUsersReportSearchModel : BaseSearchModel
    {
        #region Ctor

        public BestUsersReportSearchModel()
        {
            AvailableOrderStatuses = new List<SelectListItem>();
            AvailablePaymentStatuses = new List<SelectListItem>();
            AvailableShippingStatuses = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        //keep it synchronized to UserReportService class, GetBestUsersReport() method, orderBy parameter
        //TODO: move from int to enum
        public int OrderBy { get; set; }

        [AppResourceDisplayName("Admin.Reports.Users.BestBy.StartDate")]
        [UIHint("DateNullable")]
        public DateTime? StartDate { get; set; }

        [AppResourceDisplayName("Admin.Reports.Users.BestBy.EndDate")]
        [UIHint("DateNullable")]
        public DateTime? EndDate { get; set; }

        [AppResourceDisplayName("Admin.Reports.Users.BestBy.OrderStatus")]
        public int OrderStatusId { get; set; }

        [AppResourceDisplayName("Admin.Reports.Users.BestBy.PaymentStatus")]
        public int PaymentStatusId { get; set; }

        [AppResourceDisplayName("Admin.Reports.Users.BestBy.ShippingStatus")]
        public int ShippingStatusId { get; set; }

        public IList<SelectListItem> AvailableOrderStatuses { get; set; }

        public IList<SelectListItem> AvailablePaymentStatuses { get; set; }

        public IList<SelectListItem> AvailableShippingStatuses { get; set; }

        #endregion
    }
}