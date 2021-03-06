using System.Collections.Generic;

namespace Appulu.Services.Themes
{
    /// <summary>
    /// Themes uploaded event
    /// </summary>
    public class ThemesUploadedEvent
    {
        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="uploadedThemes">Uploaded themes</param>
        public ThemesUploadedEvent(IList<ThemeDescriptor> uploadedThemes)
        {
            this.UploadedThemes = uploadedThemes;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Uploaded themes
        /// </summary>
        public IList<ThemeDescriptor> UploadedThemes { get; }

        #endregion
    }
}