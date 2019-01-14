using System.Collections.Generic;
using Appulu.Web.Areas.Admin.Models.Stores;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Settings
{
    /// <summary>
    /// Represents a store scope configuration model
    /// </summary>
    public partial class StoreScopeConfigurationModel : BaseAppModel
    {
        #region Ctor

        public StoreScopeConfigurationModel()
        {
            Stores = new List<StoreModel>();
        }

        #endregion

        #region Properties

        public int StoreId { get; set; }

        public IList<StoreModel> Stores { get; set; }

        #endregion
    }
}