using System.Collections.Generic;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Home
{
    /// <summary>
    /// Represents a Appulu news model
    /// </summary>
    public partial class AppuluNewsModel : BaseAppModel
    {
        #region Ctor

        public AppuluNewsModel()
        {
            Items = new List<AppuluNewsDetailsModel>();
        }

        #endregion

        #region Properties

        public List<AppuluNewsDetailsModel> Items { get; set; }

        public bool HasNewItems { get; set; }

        public bool HideAdvertisements { get; set; }

        #endregion
    }
}