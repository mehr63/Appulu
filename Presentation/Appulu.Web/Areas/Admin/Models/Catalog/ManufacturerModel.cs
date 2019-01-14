using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;
using Appulu.Web.Areas.Admin.Validators.Catalog;
using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Catalog
{
    /// <summary>
    /// Represents a manufacturer model
    /// </summary>
    [Validator(typeof(ManufacturerValidator))]
    public partial class ManufacturerModel : BaseAppEntityModel, IAclSupportedModel, IDiscountSupportedModel,
        ILocalizedModel<ManufacturerLocalizedModel>, IStoreMappingSupportedModel
    {
        #region Ctor

        public ManufacturerModel()
        {
            if (PageSize < 1)
            {
                PageSize = 5;
            }
            Locales = new List<ManufacturerLocalizedModel>();
            AvailableManufacturerTemplates = new List<SelectListItem>();

            AvailableDiscounts = new List<SelectListItem>();
            SelectedDiscountIds = new List<int>();

            SelectedUserRoleIds = new List<int>();
            AvailableUserRoles = new List<SelectListItem>();

            SelectedStoreIds = new List<int>();
            AvailableStores = new List<SelectListItem>();

            ManufacturerProductSearchModel = new ManufacturerProductSearchModel();
        }

        #endregion

        #region Properties

        [AppResourceDisplayName("Admin.Catalog.Manufacturers.Fields.Name")]
        public string Name { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Manufacturers.Fields.Description")]
        public string Description { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Manufacturers.Fields.ManufacturerTemplate")]
        public int ManufacturerTemplateId { get; set; }

        public IList<SelectListItem> AvailableManufacturerTemplates { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Manufacturers.Fields.MetaKeywords")]
        public string MetaKeywords { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Manufacturers.Fields.MetaDescription")]
        public string MetaDescription { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Manufacturers.Fields.MetaTitle")]
        public string MetaTitle { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Manufacturers.Fields.SeName")]
        public string SeName { get; set; }

        [UIHint("Picture")]
        [AppResourceDisplayName("Admin.Catalog.Manufacturers.Fields.Picture")]
        public int PictureId { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Manufacturers.Fields.PageSize")]
        public int PageSize { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Manufacturers.Fields.AllowUsersToSelectPageSize")]
        public bool AllowUsersToSelectPageSize { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Manufacturers.Fields.PageSizeOptions")]
        public string PageSizeOptions { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Manufacturers.Fields.PriceRanges")]
        public string PriceRanges { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Manufacturers.Fields.Published")]
        public bool Published { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Manufacturers.Fields.Deleted")]
        public bool Deleted { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Manufacturers.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }
        
        public IList<ManufacturerLocalizedModel> Locales { get; set; }

        //ACL (user roles)
        [AppResourceDisplayName("Admin.Catalog.Manufacturers.Fields.AclUserRoles")]
        public IList<int> SelectedUserRoleIds { get; set; }
        public IList<SelectListItem> AvailableUserRoles { get; set; }
        
        //store mapping
        [AppResourceDisplayName("Admin.Catalog.Manufacturers.Fields.LimitedToStores")]
        public IList<int> SelectedStoreIds { get; set; }
        public IList<SelectListItem> AvailableStores { get; set; }

        //discounts
        [AppResourceDisplayName("Admin.Catalog.Manufacturers.Fields.Discounts")]
        public IList<int> SelectedDiscountIds { get; set; }
        public IList<SelectListItem> AvailableDiscounts { get; set; }

        public ManufacturerProductSearchModel ManufacturerProductSearchModel { get; set; }

        #endregion
    }

    public partial class ManufacturerLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Manufacturers.Fields.Name")]
        public string Name { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Manufacturers.Fields.Description")]
        public string Description {get;set;}

        [AppResourceDisplayName("Admin.Catalog.Manufacturers.Fields.MetaKeywords")]
        public string MetaKeywords { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Manufacturers.Fields.MetaDescription")]
        public string MetaDescription { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Manufacturers.Fields.MetaTitle")]
        public string MetaTitle { get; set; }

        [AppResourceDisplayName("Admin.Catalog.Manufacturers.Fields.SeName")]
        public string SeName { get; set; }
    }
}