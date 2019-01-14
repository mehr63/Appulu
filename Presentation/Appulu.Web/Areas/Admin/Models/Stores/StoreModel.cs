using System.Collections.Generic;
using FluentValidation.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;
using Appulu.Web.Areas.Admin.Validators.Stores;
using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Stores
{
    /// <summary>
    /// Represents a store model
    /// </summary>
    [Validator(typeof(StoreValidator))]
    public partial class StoreModel : BaseAppEntityModel, ILocalizedModel<StoreLocalizedModel>
    {
        #region Ctor

        public StoreModel()
        {
            Locales = new List<StoreLocalizedModel>();
            AvailableLanguages = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [AppResourceDisplayName("Admin.Configuration.Stores.Fields.Name")]
        public string Name { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Stores.Fields.Url")]
        public string Url { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Stores.Fields.SslEnabled")]
        public virtual bool SslEnabled { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Stores.Fields.Hosts")]
        public string Hosts { get; set; }

        //default language
        [AppResourceDisplayName("Admin.Configuration.Stores.Fields.DefaultLanguage")]
        public int DefaultLanguageId { get; set; }

        public IList<SelectListItem> AvailableLanguages { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Stores.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Stores.Fields.CompanyName")]
        public string CompanyName { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Stores.Fields.CompanyAddress")]
        public string CompanyAddress { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Stores.Fields.CompanyPhoneNumber")]
        public string CompanyPhoneNumber { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Stores.Fields.CompanyVat")]
        public string CompanyVat { get; set; }
        
        public IList<StoreLocalizedModel> Locales { get; set; }

        #endregion
    }

    public partial class StoreLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Stores.Fields.Name")]
        public string Name { get; set; }
    }
}