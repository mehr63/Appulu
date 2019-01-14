using System.Collections.Generic;
using Appulu.Core.Domain.Users;

namespace Appulu.Services.Cms
{
    /// <summary>
    /// Widget service interface
    /// </summary>
    public partial interface IWidgetService
    {
        /// <summary>
        /// Load active widgets
        /// </summary>
        /// <param name="user">Load records allowed only to a specified user; pass null to ignore ACL permissions</param>
        /// <param name="storeId">Load records allowed only in a specified store; pass 0 to load all records</param>
        /// <returns>Widgets</returns>
        IList<IWidgetPlugin> LoadActiveWidgets(User user = null, int storeId = 0);

        /// <summary>
        /// Load active widgets
        /// </summary>
        /// <param name="widgetZone">Widget zone</param>
        /// <param name="user">Load records allowed only to a specified user; pass null to ignore ACL permissions</param>
        /// <param name="storeId">Load records allowed only in a specified store; pass 0 to load all records</param>
        /// <returns>Widgets</returns>
        IList<IWidgetPlugin> LoadActiveWidgetsByWidgetZone(string widgetZone, User user = null, int storeId = 0);

        /// <summary>
        /// Load widget by system name
        /// </summary>
        /// <param name="systemName">System name</param>
        /// <returns>Found widget</returns>
        IWidgetPlugin LoadWidgetBySystemName(string systemName);

        /// <summary>
        /// Load all widgets
        /// </summary>
        /// <param name="user">Load records allowed only to a specified user; pass null to ignore ACL permissions</param>
        /// <param name="storeId">Load records allowed only in a specified store; pass 0 to load all records</param>
        /// <returns>Widgets</returns>
        IList<IWidgetPlugin> LoadAllWidgets(User user = null, int storeId = 0);

        /// <summary>
        /// Is widget active
        /// </summary>
        /// <param name="widget">Widget</param>
        /// <returns>Result</returns>
        bool IsWidgetActive(IWidgetPlugin widget);
    }
}