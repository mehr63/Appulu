using System.Collections.Generic;
using FluentValidation.Attributes;
using Appulu.Web.Areas.Admin.Validators.Vendors;
using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Vendors
{
    /// <summary>
    /// Represents a vendor attribute value model
    /// </summary>
    [Validator(typeof(VendorAttributeValueValidator))]
    public partial class VendorAttributeValueModel : BaseAppEntityModel, ILocalizedModel<VendorAttributeValueLocalizedModel>
    {
        #region Ctor

        public VendorAttributeValueModel()
        {
            Locales = new List<VendorAttributeValueLocalizedModel>();
        }

        #endregion

        #region Properties

        public int VendorAttributeId { get; set; }

        [AppResourceDisplayName("Admin.Vendors.VendorAttributes.Values.Fields.Name")]
        public string Name { get; set; }

        [AppResourceDisplayName("Admin.Vendors.VendorAttributes.Values.Fields.IsPreSelected")]
        public bool IsPreSelected { get; set; }

        [AppResourceDisplayName("Admin.Vendors.VendorAttributes.Values.Fields.DisplayOrder")]
        public int DisplayOrder {get;set;}

        public IList<VendorAttributeValueLocalizedModel> Locales { get; set; }

        #endregion
    }

    public partial class VendorAttributeValueLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [AppResourceDisplayName("Admin.Vendors.VendorAttributes.Values.Fields.Name")]
        public string Name { get; set; }
    }
}