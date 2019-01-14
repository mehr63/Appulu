using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Appulu.Core.Plugins;

namespace Appulu.Services.Plugins
{
    /// <summary>
    /// Represents a service for uploading application extensions (plugins or themes)
    /// </summary>
    public partial interface IUploadService
    {
        /// <summary>
        /// Upload plugins and/or themes
        /// </summary>
        /// <param name="archivefile">Archive file</param>
        /// <returns>List of uploaded items descriptor</returns>
        IList<IDescriptor> UploadPluginsAndThemes(IFormFile archivefile);
    }
}