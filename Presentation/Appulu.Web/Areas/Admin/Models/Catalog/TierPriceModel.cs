using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Catalog
{
    /// <summary>
    /// Represents a tier price model
    /// </summary>
    public partial class TierPriceModel : BaseAppEntityModel
    {
        #region Ctor

        public TierPriceModel()
        {
            AvailableStores = new List<SelectListItem>();
            AvailableUserRoles = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        public int ProductId { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Products.TierPrices.Fields.UserRole")]
        public int UserRoleId { get; set; }

        public IList<SelectListItem> AvailableUserRoles { get; set; }

        public string UserRole { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Products.TierPrices.Fields.Store")]
        public int StoreId { get; set; }

        public IList<SelectListItem> AvailableStores { get; set; }

        public string Store { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Products.TierPrices.Fields.Quantity")]
        public int Quantity { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Products.TierPrices.Fields.Price")]
        public decimal Price { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Products.TierPrices.Fields.StartDateTimeUtc")]
        [UIHint("DateTimeNullable")]
        public DateTime? StartDateTimeUtc { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Products.TierPrices.Fields.EndDateTimeUtc")]
        [UIHint("DateTimeNullable")]
        public DateTime? EndDateTimeUtc { get; set; }

        #endregion
    }
}