using System;
using System.Collections.Generic;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;
using Appulu.Web.Models.Common;

namespace Appulu.Web.Models.Order
{
    public partial class UserRewardPointsModel : BaseAppModel
    {
        public UserRewardPointsModel()
        {
            RewardPoints = new List<RewardPointsHistoryModel>();
        }

        public IList<RewardPointsHistoryModel> RewardPoints { get; set; }
        public PagerModel PagerModel { get; set; }
        public int RewardPointsBalance { get; set; }
        public string RewardPointsAmount { get; set; }
        public int MinimumRewardPointsBalance { get; set; }
        public string MinimumRewardPointsAmount { get; set; }

        #region Nested classes

        public partial class RewardPointsHistoryModel : BaseAppEntityModel
        {
            [AppResourceDisplayName("RewardPoints.Fields.Points")]
            public int Points { get; set; }

            [AppResourceDisplayName("RewardPoints.Fields.PointsBalance")]
            public string PointsBalance { get; set; }

            [AppResourceDisplayName("RewardPoints.Fields.Message")]
            public string Message { get; set; }

            [AppResourceDisplayName("RewardPoints.Fields.CreatedDate")]
            public DateTime CreatedOn { get; set; }

            [AppResourceDisplayName("RewardPoints.Fields.EndDate")]
            public DateTime? EndDate { get; set; }
        }

        #endregion
    }
}