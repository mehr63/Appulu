using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Orders
{
    /// <summary>
    /// Represents a return request search model
    /// </summary>
    public class ReturnRequestSearchModel: BaseSearchModel
    {
        #region Ctor

        public ReturnRequestSearchModel()
        {
            ReturnRequestStatusList = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [AppResourceDisplayName("Admin.ReturnRequests.SearchStartDate")]
        [UIHint("DateNullable")]
        public DateTime? StartDate { get; set; }

        [AppResourceDisplayName("Admin.ReturnRequests.SearchEndDate")]
        [UIHint("DateNullable")]
        public DateTime? EndDate { get; set; }

        [AppResourceDisplayName("Admin.ReturnRequests.SearchCustomNumber")]
        public string CustomNumber { get; set; }

        [AppResourceDisplayName("Admin.ReturnRequests.SearchReturnRequestStatus")]
        public int ReturnRequestStatusId { get; set; }

        public IList<SelectListItem> ReturnRequestStatusList { get; set; }

        #endregion
    }
}