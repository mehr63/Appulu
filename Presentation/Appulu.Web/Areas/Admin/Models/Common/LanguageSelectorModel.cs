using System.Collections.Generic;
using Appulu.Web.Areas.Admin.Models.Localization;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Common
{
    /// <summary>
    /// Represents an admin language selector model
    /// </summary>
    public partial class LanguageSelectorModel : BaseAppModel
    {
        #region Ctor

        public LanguageSelectorModel()
        {
            AvailableLanguages = new List<LanguageModel>();
        }

        #endregion

        #region Properties

        public IList<LanguageModel> AvailableLanguages { get; set; }

        public LanguageModel CurrentLanguage { get; set; }

        #endregion
    }
}