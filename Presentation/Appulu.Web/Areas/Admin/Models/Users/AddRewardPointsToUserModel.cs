using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Users
{
    /// <summary>
    /// Represents a reward points model to add to the user
    /// </summary>
    public partial class AddRewardPointsToUserModel : BaseAppModel
    {
        #region Ctor

        public AddRewardPointsToUserModel()
        {
            this.AvailableStores = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        public int UserId { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.RewardPoints.Fields.Points")]
        public int Points { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.RewardPoints.Fields.Message")]
        public string Message { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.RewardPoints.Fields.Store")]
        public int StoreId { get; set; }

        public IList<SelectListItem> AvailableStores { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.RewardPoints.Fields.ActivatePointsImmediately")]
        public bool ActivatePointsImmediately { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.RewardPoints.Fields.ActivationDelay")]
        public int ActivationDelay { get; set; }

        public int ActivationDelayPeriodId { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.RewardPoints.Fields.PointsValidity")]
        [UIHint("Int32Nullable")]
        public int? PointsValidity { get; set; }

        #endregion
    }
}