﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Reports
{
    /// <summary>
    /// Represents a country report search model
    /// </summary>
    public partial class CountryReportSearchModel : BaseSearchModel
    {
        #region Ctor

        public CountryReportSearchModel()
        {
            AvailableOrderStatuses = new List<SelectListItem>();
            AvailablePaymentStatuses = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [AppResourceDisplayName("Admin.Reports.Sales.Country.StartDate")]
        [UIHint("DateNullable")]
        public DateTime? StartDate { get; set; }

        [AppResourceDisplayName("Admin.Reports.Sales.Country.EndDate")]
        [UIHint("DateNullable")]
        public DateTime? EndDate { get; set; }

        [AppResourceDisplayName("Admin.Reports.Sales.Country.OrderStatus")]
        public int OrderStatusId { get; set; }

        [AppResourceDisplayName("Admin.Reports.Sales.Country.PaymentStatus")]
        public int PaymentStatusId { get; set; }

        public IList<SelectListItem> AvailableOrderStatuses { get; set; }

        public IList<SelectListItem> AvailablePaymentStatuses { get; set; }

        #endregion
    }
}