namespace Appulu.Core.Plugins
{
    /// <summary>
    /// Represents a mode to load plugins
    /// </summary>
    public enum LoadPluginsMode
    {
        /// <summary>
        /// All (Installed and Not installed)
        /// </summary>
        All = 0,

        /// <summary>
        /// Installed only
        /// </summary>
        InstalledOnly = 10,

        /// <summary>
        /// Not installed only
        /// </summary>
        NotInstalledOnly = 20
    }
}