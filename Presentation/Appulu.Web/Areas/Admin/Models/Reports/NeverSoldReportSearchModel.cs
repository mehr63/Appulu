﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Reports
{
    /// <summary>
    /// Represents a never sold products report search model
    /// </summary>
    public partial class NeverSoldReportSearchModel : BaseSearchModel
    {
        #region Ctor

        public NeverSoldReportSearchModel()
        {
            AvailableCategories = new List<SelectListItem>();
            AvailableManufacturers = new List<SelectListItem>();
            AvailableStores = new List<SelectListItem>();
            AvailableVendors = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [AppResourceDisplayName("Admin.Reports.Sales.NeverSold.StartDate")]
        [UIHint("DateNullable")]
        public DateTime? StartDate { get; set; }

        [AppResourceDisplayName("Admin.Reports.Sales.NeverSold.EndDate")]
        [UIHint("DateNullable")]
        public DateTime? EndDate { get; set; }

        [AppResourceDisplayName("Admin.Reports.Sales.NeverSold.SearchCategory")]
        public int SearchCategoryId { get; set; }

        public IList<SelectListItem> AvailableCategories { get; set; }

        [AppResourceDisplayName("Admin.Reports.Sales.NeverSold.SearchManufacturer")]
        public int SearchManufacturerId { get; set; }

        public IList<SelectListItem> AvailableManufacturers { get; set; }

        [AppResourceDisplayName("Admin.Reports.Sales.NeverSold.SearchStore")]
        public int SearchStoreId { get; set; }

        public IList<SelectListItem> AvailableStores { get; set; }
        
        [AppResourceDisplayName("Admin.Reports.Sales.NeverSold.SearchVendor")]
        public int SearchVendorId { get; set; }

        public IList<SelectListItem> AvailableVendors { get; set; }

        public bool IsLoggedInAsVendor { get; set; }

        #endregion
    }
}