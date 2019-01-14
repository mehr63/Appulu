using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using Appulu.Core.Domain.Catalog;
using Appulu.Web.Areas.Admin.Models.Common;
using Appulu.Web.Areas.Admin.Validators.Vendors;
using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Vendors
{
    /// <summary>
    /// Represents a vendor model
    /// </summary>
    [Validator(typeof(VendorValidator))]
    public partial class VendorModel : BaseAppEntityModel, ILocalizedModel<VendorLocalizedModel>
    {
        #region Ctor

        public VendorModel()
        {
            if (PageSize < 1)
                PageSize = 5;

            Address = new AddressModel();
            VendorAttributes = new List<VendorAttributeModel>();
            Locales = new List<VendorLocalizedModel>();
            AssociatedUsers = new List<VendorAssociatedUserModel>();
            VendorNoteSearchModel = new VendorNoteSearchModel();
        }

        #endregion

        #region Properties

        [AppResourceDisplayName("Admin.Vendors.Fields.Name")]
        public string Name { get; set; }

        [DataType(DataType.EmailAddress)]
        [AppResourceDisplayName("Admin.Vendors.Fields.Email")]
        public string Email { get; set; }

        [AppResourceDisplayName("Admin.Vendors.Fields.Description")]
        public string Description { get; set; }

        [UIHint("Picture")]
        [AppResourceDisplayName("Admin.Vendors.Fields.Picture")]
        public int PictureId { get; set; }

        [AppResourceDisplayName("Admin.Vendors.Fields.AdminComment")]
        public string AdminComment { get; set; }

        public AddressModel Address { get; set; }

        [AppResourceDisplayName("Admin.Vendors.Fields.Active")]
        public bool Active { get; set; }

        [AppResourceDisplayName("Admin.Vendors.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }        

        [AppResourceDisplayName("Admin.Vendors.Fields.MetaKeywords")]
        public string MetaKeywords { get; set; }

        [AppResourceDisplayName("Admin.Vendors.Fields.MetaDescription")]
        public string MetaDescription { get; set; }

        [AppResourceDisplayName("Admin.Vendors.Fields.MetaTitle")]
        public string MetaTitle { get; set; }

        [AppResourceDisplayName("Admin.Vendors.Fields.SeName")]
        public string SeName { get; set; }

        [AppResourceDisplayName("Admin.Vendors.Fields.PageSize")]
        public int PageSize { get; set; }

        [AppResourceDisplayName("Admin.Vendors.Fields.AllowUsersToSelectPageSize")]
        public bool AllowUsersToSelectPageSize { get; set; }

        [AppResourceDisplayName("Admin.Vendors.Fields.PageSizeOptions")]
        public string PageSizeOptions { get; set; }

        public List<VendorAttributeModel> VendorAttributes { get; set; }

        public IList<VendorLocalizedModel> Locales { get; set; }

        [AppResourceDisplayName("Admin.Vendors.Fields.AssociatedUserEmails")]
        public IList<VendorAssociatedUserModel> AssociatedUsers { get; set; }

        //vendor notes
        [AppResourceDisplayName("Admin.Vendors.VendorNotes.Fields.Note")]
        public string AddVendorNoteMessage { get; set; }

        public VendorNoteSearchModel VendorNoteSearchModel { get; set; }

        #endregion

        #region Nested classes
        
        public partial class VendorAttributeModel : BaseAppEntityModel
        {
            public VendorAttributeModel()
            {
                Values = new List<VendorAttributeValueModel>();
            }

            public string Name { get; set; }

            public bool IsRequired { get; set; }

            /// <summary>
            /// Default value for textboxes
            /// </summary>
            public string DefaultValue { get; set; }

            public AttributeControlType AttributeControlType { get; set; }

            public IList<VendorAttributeValueModel> Values { get; set; }
        }

        public partial class VendorAttributeValueModel : BaseAppEntityModel
        {
            public string Name { get; set; }

            public bool IsPreSelected { get; set; }
        }

        #endregion
    }

    public partial class VendorLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [AppResourceDisplayName("Admin.Vendors.Fields.Name")]
        public string Name { get; set; }

        [AppResourceDisplayName("Admin.Vendors.Fields.Description")]
        public string Description { get; set; }

        [AppResourceDisplayName("Admin.Vendors.Fields.MetaKeywords")]
        public string MetaKeywords { get; set; }

        [AppResourceDisplayName("Admin.Vendors.Fields.MetaDescription")]
        public string MetaDescription { get; set; }

        [AppResourceDisplayName("Admin.Vendors.Fields.MetaTitle")]
        public string MetaTitle { get; set; }

        [AppResourceDisplayName("Admin.Vendors.Fields.SeName")]
        public string SeName { get; set; }
    }
}