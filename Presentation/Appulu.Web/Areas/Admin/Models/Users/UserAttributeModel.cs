using System.Collections.Generic;
using FluentValidation.Attributes;
using Appulu.Web.Areas.Admin.Validators.Users;
using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Users
{
    /// <summary>
    /// Represents a user attribute model
    /// </summary>
    [Validator(typeof(UserAttributeValidator))]
    public partial class UserAttributeModel : BaseAppEntityModel, ILocalizedModel<UserAttributeLocalizedModel>
    {
        #region Ctor

        public UserAttributeModel()
        {
            Locales = new List<UserAttributeLocalizedModel>();
            UserAttributeValueSearchModel = new UserAttributeValueSearchModel();
        }

        #endregion

        #region Properties

        [AppResourceDisplayName("Admin.Users.UserAttributes.Fields.Name")]
        public string Name { get; set; }

        [AppResourceDisplayName("Admin.Users.UserAttributes.Fields.IsRequired")]
        public bool IsRequired { get; set; }

        [AppResourceDisplayName("Admin.Users.UserAttributes.Fields.AttributeControlType")]
        public int AttributeControlTypeId { get; set; }

        [AppResourceDisplayName("Admin.Users.UserAttributes.Fields.AttributeControlType")]
        public string AttributeControlTypeName { get; set; }

        [AppResourceDisplayName("Admin.Users.UserAttributes.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        public IList<UserAttributeLocalizedModel> Locales { get; set; }

        public UserAttributeValueSearchModel UserAttributeValueSearchModel { get; set; }

        #endregion
    }

    public partial class UserAttributeLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [AppResourceDisplayName("Admin.Users.UserAttributes.Fields.Name")]
        public string Name { get; set; }
    }
}