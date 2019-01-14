using System.Collections.Generic;
using FluentValidation.Attributes;
using Appulu.Web.Areas.Admin.Validators.Common;
using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Common
{
    /// <summary>
    /// Represents an address attribute value model
    /// </summary>
    [Validator(typeof(AddressAttributeValueValidator))]
    public partial class AddressAttributeValueModel : BaseAppEntityModel, ILocalizedModel<AddressAttributeValueLocalizedModel>
    {
        #region Ctor

        public AddressAttributeValueModel()
        {
            Locales = new List<AddressAttributeValueLocalizedModel>();
        }

        #endregion

        #region Properties

        public int AddressAttributeId { get; set; }

        [AppResourceDisplayName("Admin.Address.AddressAttributes.Values.Fields.Name")]
        public string Name { get; set; }

        [AppResourceDisplayName("Admin.Address.AddressAttributes.Values.Fields.IsPreSelected")]
        public bool IsPreSelected { get; set; }

        [AppResourceDisplayName("Admin.Address.AddressAttributes.Values.Fields.DisplayOrder")]
        public int DisplayOrder {get;set;}

        public IList<AddressAttributeValueLocalizedModel> Locales { get; set; }

        #endregion
    }

    public partial class AddressAttributeValueLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [AppResourceDisplayName("Admin.Address.AddressAttributes.Values.Fields.Name")]
        public string Name { get; set; }
    }
}