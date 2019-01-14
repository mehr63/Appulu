using System.Collections.Generic;
using FluentValidation.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;
using Appulu.Web.Areas.Admin.Validators.Directory;
using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Directory
{
    /// <summary>
    /// Represents a country model
    /// </summary>
    [Validator(typeof(CountryValidator))]
    public partial class CountryModel : BaseAppEntityModel, ILocalizedModel<CountryLocalizedModel>, IStoreMappingSupportedModel
    {
        #region Ctor

        public CountryModel()
        {
            Locales = new List<CountryLocalizedModel>();
            SelectedStoreIds = new List<int>();
            AvailableStores = new List<SelectListItem>();
            StateProvinceSearchModel = new StateProvinceSearchModel();
        }

        #endregion

        #region Properties

        [AppResourceDisplayName("Admin.Configuration.Countries.Fields.Name")]
        public string Name { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Countries.Fields.AllowsBilling")]
        public bool AllowsBilling { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Countries.Fields.AllowsShipping")]
        public bool AllowsShipping { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Countries.Fields.TwoLetterIsoCode")]
        public string TwoLetterIsoCode { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Countries.Fields.ThreeLetterIsoCode")]
        public string ThreeLetterIsoCode { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Countries.Fields.NumericIsoCode")]
        public int NumericIsoCode { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Countries.Fields.SubjectToVat")]
        public bool SubjectToVat { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Countries.Fields.Published")]
        public bool Published { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Countries.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Countries.Fields.NumberOfStates")]
        public int NumberOfStates { get; set; }

        public IList<CountryLocalizedModel> Locales { get; set; }

        //store mapping
        [AppResourceDisplayName("Admin.Configuration.Countries.Fields.LimitedToStores")]
        public IList<int> SelectedStoreIds { get; set; }
        public IList<SelectListItem> AvailableStores { get; set; }

        public StateProvinceSearchModel StateProvinceSearchModel { get; set; }

        #endregion
    }

    public partial class CountryLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Countries.Fields.Name")]
        public string Name { get; set; }
    }
}