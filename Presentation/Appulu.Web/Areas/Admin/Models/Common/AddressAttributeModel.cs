using System.Collections.Generic;
using FluentValidation.Attributes;
using Appulu.Web.Areas.Admin.Validators.Common;
using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Common
{
    /// <summary>
    /// Represents an address attribute model
    /// </summary>
    [Validator(typeof(AddressAttributeValidator))]
    public partial class AddressAttributeModel : BaseAppEntityModel, ILocalizedModel<AddressAttributeLocalizedModel>
    {
        #region Ctor

        public AddressAttributeModel()
        {
            Locales = new List<AddressAttributeLocalizedModel>();
            AddressAttributeValueSearchModel = new AddressAttributeValueSearchModel();
        }

        #endregion

        #region Properties

        [AppResourceDisplayName("Admin.Address.AddressAttributes.Fields.Name")]
        public string Name { get; set; }

        [AppResourceDisplayName("Admin.Address.AddressAttributes.Fields.IsRequired")]
        public bool IsRequired { get; set; }

        [AppResourceDisplayName("Admin.Address.AddressAttributes.Fields.AttributeControlType")]
        public int AttributeControlTypeId { get; set; }

        [AppResourceDisplayName("Admin.Address.AddressAttributes.Fields.AttributeControlType")]
        public string AttributeControlTypeName { get; set; }

        [AppResourceDisplayName("Admin.Address.AddressAttributes.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        public IList<AddressAttributeLocalizedModel> Locales { get; set; }

        public AddressAttributeValueSearchModel AddressAttributeValueSearchModel { get; set; }

        #endregion
    }

    public partial class AddressAttributeLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [AppResourceDisplayName("Admin.Address.AddressAttributes.Fields.Name")]
        public string Name { get; set; }
    }
}