using System.Collections.Generic;
using FluentValidation.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;
using Appulu.Web.Areas.Admin.Validators.Localization;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Localization
{
    /// <summary>
    /// Represents a language model
    /// </summary>
    [Validator(typeof(LanguageValidator))]
    public partial class LanguageModel : BaseAppEntityModel, IStoreMappingSupportedModel
    {
        #region Ctor

        public LanguageModel()
        {
            AvailableCurrencies = new List<SelectListItem>();
            SelectedStoreIds = new List<int>();
            AvailableStores = new List<SelectListItem>();
            LocaleResourceSearchModel = new LocaleResourceSearchModel();
        }

        #endregion

        #region Properties

        [AppResourceDisplayName("Admin.Configuration.Languages.Fields.Name")]
        public string Name { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Languages.Fields.LanguageCulture")]
        public string LanguageCulture { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Languages.Fields.UniqueSeoCode")]
        public string UniqueSeoCode { get; set; }
        
        //flags
        [AppResourceDisplayName("Admin.Configuration.Languages.Fields.FlagImageFileName")]
        public string FlagImageFileName { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Languages.Fields.Rtl")]
        public bool Rtl { get; set; }

        //default currency
        [AppResourceDisplayName("Admin.Configuration.Languages.Fields.DefaultCurrency")]
        public int DefaultCurrencyId { get; set; }

        public IList<SelectListItem> AvailableCurrencies { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Languages.Fields.Published")]
        public bool Published { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Languages.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }
        
        //store mapping
        [AppResourceDisplayName("Admin.Configuration.Languages.Fields.LimitedToStores")]
        public IList<int> SelectedStoreIds { get; set; }

        public IList<SelectListItem> AvailableStores { get; set; }

        // search
        public LocaleResourceSearchModel LocaleResourceSearchModel { get; set; }

        #endregion
    }
}