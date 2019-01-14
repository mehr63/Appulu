using System;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Home
{
    /// <summary>
    /// Represents a Appulu news details model
    /// </summary>
    public partial class AppuluNewsDetailsModel : BaseAppModel
    {
        #region Properties

        public string Title { get; set; }

        public string Url { get; set; }

        public string Summary { get; set; }

        public DateTimeOffset PublishDate { get; set; }

        #endregion
    }
}