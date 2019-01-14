using System.Collections.Generic;
using FluentValidation.Attributes;
using Appulu.Web.Areas.Admin.Validators.Users;
using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Users
{
    /// <summary>
    /// Represents a user attribute value model
    /// </summary>
    [Validator(typeof(UserAttributeValueValidator))]
    public partial class UserAttributeValueModel : BaseAppEntityModel, ILocalizedModel<UserAttributeValueLocalizedModel>
    {
        #region Ctor

        public UserAttributeValueModel()
        {
            Locales = new List<UserAttributeValueLocalizedModel>();
        }

        #endregion

        #region Properties

        public int UserAttributeId { get; set; }

        [AppResourceDisplayName("Admin.Users.UserAttributes.Values.Fields.Name")]
        public string Name { get; set; }

        [AppResourceDisplayName("Admin.Users.UserAttributes.Values.Fields.IsPreSelected")]
        public bool IsPreSelected { get; set; }

        [AppResourceDisplayName("Admin.Users.UserAttributes.Values.Fields.DisplayOrder")]
        public int DisplayOrder {get;set;}

        public IList<UserAttributeValueLocalizedModel> Locales { get; set; }

        #endregion
    }

    public partial class UserAttributeValueLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [AppResourceDisplayName("Admin.Users.UserAttributes.Values.Fields.Name")]
        public string Name { get; set; }
    }
}