using FluentValidation.Attributes;
using Appulu.Web.Areas.Admin.Validators.Settings;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;
using System.Collections.Generic;

namespace Appulu.Web.Areas.Admin.Models.Settings
{
    /// <summary>
    /// Represents a GDPR consent model
    /// </summary>
    [Validator(typeof(GdprConsentValidator))]
    public partial class GdprConsentModel : BaseAppEntityModel, ILocalizedModel<GdprConsentLocalizedModel>
    {
        #region Ctor

        public GdprConsentModel()
        {
            Locales = new List<GdprConsentLocalizedModel>();
        }

        #endregion

        #region Properties

        [AppResourceDisplayName("Admin.Configuration.Settings.Gdpr.Consent.Message")]
        public string Message { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.Gdpr.Consent.IsRequired")]
        public bool IsRequired { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.Gdpr.Consent.RequiredMessage")]
        public string RequiredMessage { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.Gdpr.Consent.DisplayDuringRegistration")]
        public bool DisplayDuringRegistration { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.Gdpr.Consent.DisplayOnUserInfoPage")]
        public bool DisplayOnUserInfoPage { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.Gdpr.Consent.DisplayOrder")]
        public int DisplayOrder { get; set; }

        public IList<GdprConsentLocalizedModel> Locales { get; set; }

        #endregion
    }
}