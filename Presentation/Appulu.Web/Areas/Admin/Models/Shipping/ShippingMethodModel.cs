using System.Collections.Generic;
using FluentValidation.Attributes;
using Appulu.Web.Areas.Admin.Validators.Shipping;
using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Shipping
{
    /// <summary>
    /// Represents a shipping method model
    /// </summary>
    [Validator(typeof(ShippingMethodValidator))]
    public partial class ShippingMethodModel : BaseAppEntityModel, ILocalizedModel<ShippingMethodLocalizedModel>
    {
        #region Ctor

        public ShippingMethodModel()
        {
            Locales = new List<ShippingMethodLocalizedModel>();
        }

        #endregion

        #region Properties

        [AppResourceDisplayName("Admin.Configuration.Shipping.Methods.Fields.Name")]
        public string Name { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Shipping.Methods.Fields.Description")]
        public string Description { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Shipping.Methods.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        public IList<ShippingMethodLocalizedModel> Locales { get; set; }

        #endregion
    }

    public partial class ShippingMethodLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Shipping.Methods.Fields.Name")]
        public string Name { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Shipping.Methods.Fields.Description")]
        public string Description { get; set; }
    }
}