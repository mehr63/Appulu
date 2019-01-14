using System;
using System.Linq;
using Appulu.Core;
using Appulu.Core.Domain;
using Appulu.Core.Domain.Users;
using Appulu.Services.Common;
using Appulu.Services.Themes;

namespace Appulu.Web.Framework.Themes
{
    /// <summary>
    /// Represents the theme context implementation
    /// </summary>
    public partial class ThemeContext : IThemeContext
    {
        #region Fields

        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IStoreContext _storeContext;
        private readonly IThemeProvider _themeProvider;
        private readonly IWorkContext _workContext;
        private readonly StoreInformationSettings _storeInformationSettings;

        private string _cachedThemeName;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="genericAttributeService">Generic attribute service</param>
        /// <param name="storeContext">Store context</param>
        /// <param name="themeProvider">Theme provider</param>
        /// <param name="workContext">Work context</param>
        /// <param name="storeInformationSettings">Store information settings</param>
        public ThemeContext(IGenericAttributeService genericAttributeService,
            IStoreContext storeContext,
            IThemeProvider themeProvider,
            IWorkContext workContext,
            StoreInformationSettings storeInformationSettings)
        {
            this._genericAttributeService = genericAttributeService;
            this._storeContext = storeContext;
            this._themeProvider = themeProvider;
            this._workContext = workContext;
            this._storeInformationSettings = storeInformationSettings;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Get or set current theme system name
        /// </summary>
        public string WorkingThemeName
        {
            get
            {
                if (!string.IsNullOrEmpty(_cachedThemeName))
                    return _cachedThemeName;

                var themeName = string.Empty;

                //whether users are allowed to select a theme
                if (_storeInformationSettings.AllowUserToSelectTheme && _workContext.CurrentUser != null)
                {
                    themeName = _genericAttributeService.GetAttribute<string>(_workContext.CurrentUser,
                        AppUserDefaults.WorkingThemeNameAttribute, _storeContext.CurrentStore.Id);
                }

                //if not, try to get default store theme
                if (string.IsNullOrEmpty(themeName))
                    themeName = _storeInformationSettings.DefaultStoreTheme;

                //ensure that this theme exists
                if (!_themeProvider.ThemeExists(themeName))
                {
                    //if it does not exist, try to get the first one
                    themeName = _themeProvider.GetThemes().FirstOrDefault()?.SystemName
                        ?? throw new Exception("No theme could be loaded");
                }

                //cache theme system name
                this._cachedThemeName = themeName;

                return themeName;
            }
            set
            {
                //whether users are allowed to select a theme
                if (!_storeInformationSettings.AllowUserToSelectTheme || _workContext.CurrentUser == null)
                    return;

                //save selected by user theme system name
                _genericAttributeService.SaveAttribute(_workContext.CurrentUser,
                    AppUserDefaults.WorkingThemeNameAttribute, value, _storeContext.CurrentStore.Id);

                //clear cache
                this._cachedThemeName = null;
            }
        }

        #endregion
    }
}