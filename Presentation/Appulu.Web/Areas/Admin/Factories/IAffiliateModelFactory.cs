﻿using Appulu.Core.Domain.Affiliates;
using Appulu.Web.Areas.Admin.Models.Affiliates;

namespace Appulu.Web.Areas.Admin.Factories
{
    /// <summary>
    /// Represents the affiliate model factory
    /// </summary>
    public partial interface IAffiliateModelFactory
    {
        /// <summary>
        /// Prepare affiliate search model
        /// </summary>
        /// <param name="searchModel">Affiliate search model</param>
        /// <returns>Affiliate search model</returns>
        AffiliateSearchModel PrepareAffiliateSearchModel(AffiliateSearchModel searchModel);

        /// <summary>
        /// Prepare paged affiliate list model
        /// </summary>
        /// <param name="searchModel">Affiliate search model</param>
        /// <returns>Affiliate list model</returns>
        AffiliateListModel PrepareAffiliateListModel(AffiliateSearchModel searchModel);

        /// <summary>
        /// Prepare affiliate model
        /// </summary>
        /// <param name="model">Affiliate model</param>
        /// <param name="affiliate">Affiliate</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>Affiliate model</returns>
        AffiliateModel PrepareAffiliateModel(AffiliateModel model, Affiliate affiliate, bool excludeProperties = false);

        /// <summary>
        /// Prepare paged affiliated order list model
        /// </summary>
        /// <param name="searchModel">Affiliated order search model</param>
        /// <param name="affiliate">Affiliate</param>
        /// <returns>Affiliated order list model</returns>
        AffiliatedOrderListModel PrepareAffiliatedOrderListModel(AffiliatedOrderSearchModel searchModel, Affiliate affiliate);

        /// <summary>
        /// Prepare paged affiliated user list model
        /// </summary>
        /// <param name="searchModel">Affiliated user search model</param>
        /// <param name="affiliate">Affiliate</param>
        /// <returns>Affiliated user list model</returns>
        AffiliatedUserListModel PrepareAffiliatedUserListModel(AffiliatedUserSearchModel searchModel, 
            Affiliate affiliate);
    }
}