using System.Collections.Generic;
using FluentValidation.Attributes;
using Appulu.Web.Areas.Admin.Validators.Directory;
using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Directory
{
    /// <summary>
    /// Represents a state and province model
    /// </summary>
    [Validator(typeof(StateProvinceValidator))]
    public partial class StateProvinceModel : BaseAppEntityModel, ILocalizedModel<StateProvinceLocalizedModel>
    {
        #region Ctor

        public StateProvinceModel()
        {
            Locales = new List<StateProvinceLocalizedModel>();
        }

        #endregion

        #region Properties

        public int CountryId { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Countries.States.Fields.Name")]
        public string Name { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Countries.States.Fields.Abbreviation")]
        public string Abbreviation { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Countries.States.Fields.Published")]
        public bool Published { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Countries.States.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

        public IList<StateProvinceLocalizedModel> Locales { get; set; }

        #endregion
    }

    public partial class StateProvinceLocalizedModel : ILocalizedLocaleModel
    {
        public int LanguageId { get; set; }
        
        [AppResourceDisplayName("Admin.Configuration.Countries.States.Fields.Name")]
        public string Name { get; set; }
    }
}