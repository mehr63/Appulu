using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;
using Appulu.Web.Areas.Admin.Validators.Discounts;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Discounts
{
    /// <summary>
    /// Represents a discount model
    /// </summary>
    [Validator(typeof(DiscountValidator))]
    public partial class DiscountModel : BaseAppEntityModel
    {
        #region Ctor

        public DiscountModel()
        {
            AvailableDiscountRequirementRules = new List<SelectListItem>();
            AvailableRequirementGroups = new List<SelectListItem>();
            DiscountUsageHistorySearchModel = new DiscountUsageHistorySearchModel();
            DiscountProductSearchModel = new DiscountProductSearchModel();
            DiscountCategorySearchModel = new DiscountCategorySearchModel();
            DiscountManufacturerSearchModel = new DiscountManufacturerSearchModel();
        }

        #endregion

        #region Properties

        [AppResourceDisplayName("Admin.Promotions.Discounts.Fields.Name")]
        public string Name { get; set; }

        [AppResourceDisplayName("Admin.Promotions.Discounts.Fields.DiscountType")]
        public int DiscountTypeId { get; set; }

        [AppResourceDisplayName("Admin.Promotions.Discounts.Fields.DiscountType")]
        public string DiscountTypeName { get; set; }

        //used for the list page
        [AppResourceDisplayName("Admin.Promotions.Discounts.Fields.TimesUsed")]
        public int TimesUsed { get; set; }

        [AppResourceDisplayName("Admin.Promotions.Discounts.Fields.UsePercentage")]
        public bool UsePercentage { get; set; }

        [AppResourceDisplayName("Admin.Promotions.Discounts.Fields.DiscountPercentage")]
        public decimal DiscountPercentage { get; set; }

        [AppResourceDisplayName("Admin.Promotions.Discounts.Fields.DiscountAmount")]
        public decimal DiscountAmount { get; set; }

        [AppResourceDisplayName("Admin.Promotions.Discounts.Fields.MaximumDiscountAmount")]
        [UIHint("DecimalNullable")]
        public decimal? MaximumDiscountAmount { get; set; }

        public string PrimaryStoreCurrencyCode { get; set; }

        [AppResourceDisplayName("Admin.Promotions.Discounts.Fields.StartDate")]
        [UIHint("DateTimeNullable")]
        public DateTime? StartDateUtc { get; set; }

        [AppResourceDisplayName("Admin.Promotions.Discounts.Fields.EndDate")]
        [UIHint("DateTimeNullable")]
        public DateTime? EndDateUtc { get; set; }

        [AppResourceDisplayName("Admin.Promotions.Discounts.Fields.RequiresCouponCode")]
        public bool RequiresCouponCode { get; set; }

        [AppResourceDisplayName("Admin.Promotions.Discounts.Fields.DiscountUrl")]
        public string DiscountUrl { get; set; }

        [AppResourceDisplayName("Admin.Promotions.Discounts.Fields.CouponCode")]
        public string CouponCode { get; set; }

        [AppResourceDisplayName("Admin.Promotions.Discounts.Fields.IsCumulative")]
        public bool IsCumulative { get; set; }

        [AppResourceDisplayName("Admin.Promotions.Discounts.Fields.DiscountLimitation")]
        public int DiscountLimitationId { get; set; }

        [AppResourceDisplayName("Admin.Promotions.Discounts.Fields.LimitationTimes")]
        public int LimitationTimes { get; set; }

        [AppResourceDisplayName("Admin.Promotions.Discounts.Fields.MaximumDiscountedQuantity")]
        [UIHint("Int32Nullable")]
        public int? MaximumDiscountedQuantity { get; set; }
        
        [AppResourceDisplayName("Admin.Promotions.Discounts.Fields.AppliedToSubCategories")]
        public bool AppliedToSubCategories { get; set; }

        [AppResourceDisplayName("Admin.Promotions.Discounts.Requirements.DiscountRequirementType")]
        public string AddDiscountRequirement { get; set; }

        public IList<SelectListItem> AvailableDiscountRequirementRules { get; set; }

        [AppResourceDisplayName("Admin.Promotions.Discounts.Requirements.GroupName")]
        public string GroupName { get; set; }

        [AppResourceDisplayName("Admin.Promotions.Discounts.Requirements.RequirementGroup")]
        public int RequirementGroupId { get; set; }

        public IList<SelectListItem> AvailableRequirementGroups { get; set; }

        public DiscountUsageHistorySearchModel DiscountUsageHistorySearchModel { get; set; }

        public DiscountProductSearchModel DiscountProductSearchModel { get; set; }

        public DiscountCategorySearchModel DiscountCategorySearchModel { get; set; }

        public DiscountManufacturerSearchModel DiscountManufacturerSearchModel { get; set; }

        #endregion
    }
}