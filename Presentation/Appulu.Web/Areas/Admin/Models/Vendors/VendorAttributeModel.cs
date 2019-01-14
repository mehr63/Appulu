using System.Collections.Generic;
using FluentValidation.Attributes;
using Appulu.Web.Areas.Admin.Validators.Vendors;
using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Vendors
{
    /// <summary>
    /// Represents a vendor attribute model
    /// </summary>
    [Validator(typeof(VendorAttributeValidator))]
    public partial class VendorAttributeModel : BaseAppEntityModel, ILocalizedModel<VendorAttributeLocalizedModel>
    {
        #region Ctor

        public VendorAttributeModel()
        {
            Locales = new List<VendorAttributeLocalizedModel>();
            VendorAttributeValueSearchModel = new VendorAttributeValueSearchModel();
        }

        #endregion

        #region Properties

        [AppResourceDisplayName("Admin.Vendors.VendorAttributes.Fields.Name")]
        public string Name { get; set; }

        [AppResourceDisplayName("Admin.Vendors.VendorAttributes.Fields.IsRequired")]
        public bool IsRequired { get; set; }

        [AppResourceDisplayName("Admin.Vendors.VendorAttributes.Fields.AttributeControlType")]
        public int AttributeControlTypeId { get; set; }

        [AppResourceDisplayName("Admin.Vendors.VendorAttributes.Fields.AttributeControlType")]
        public string AttributeControlTypeName { get; set; }

        [AppResourceDisplayName("Admin.Vendors.VendorAttributes.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        public IList<VendorAttributeLocalizedModel> Locales { get; set; }

        public VendorAttributeValueSearchModel VendorAttributeValueSearchModel { get; set; }

        #endregion
    }

    public partial class VendorAttributeLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [AppResourceDisplayName("Admin.Vendors.VendorAttributes.Fields.Name")]
        public string Name { get; set; }
    }
}