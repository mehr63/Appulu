﻿using System;
using System.Linq;
using Appulu.Core.Domain.Messages;
using Appulu.Services.Helpers;
using Appulu.Services.Messages;
using Appulu.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Appulu.Web.Areas.Admin.Models.Messages;
using Appulu.Web.Framework.Extensions;

namespace Appulu.Web.Areas.Admin.Factories
{
    /// <summary>
    /// Represents the campaign model factory implementation
    /// </summary>
    public partial class CampaignModelFactory : ICampaignModelFactory
    {
        #region Fields

        private readonly EmailAccountSettings _emailAccountSettings;
        private readonly IBaseAdminModelFactory _baseAdminModelFactory;
        private readonly ICampaignService _campaignService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly IMessageTokenProvider _messageTokenProvider;

        #endregion

        #region Ctor

        public CampaignModelFactory(EmailAccountSettings emailAccountSettings,
            IBaseAdminModelFactory baseAdminModelFactory,
            ICampaignService campaignService,
            IDateTimeHelper dateTimeHelper,
            IMessageTokenProvider messageTokenProvider)
        {
            this._emailAccountSettings = emailAccountSettings;
            this._baseAdminModelFactory = baseAdminModelFactory;
            this._campaignService = campaignService;
            this._dateTimeHelper = dateTimeHelper;
            this._messageTokenProvider = messageTokenProvider;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare campaign search model
        /// </summary>
        /// <param name="searchModel">Campaign search model</param>
        /// <returns>Campaign search model</returns>
        public virtual CampaignSearchModel PrepareCampaignSearchModel(CampaignSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare available stores
            _baseAdminModelFactory.PrepareStores(searchModel.AvailableStores);

            //prepare page parameters
            searchModel.SetGridPageSize();

            return searchModel;
        }

        /// <summary>
        /// Prepare paged campaign list model
        /// </summary>
        /// <param name="searchModel">Campaign search model</param>
        /// <returns>Campaign list model</returns>
        public virtual CampaignListModel PrepareCampaignListModel(CampaignSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));
            
            //get campaigns
            var campaigns = _campaignService.GetAllCampaigns(searchModel.StoreId);

            //prepare grid model
            var model = new CampaignListModel
            {
                Data = campaigns.PaginationByRequestModel(searchModel).Select(campaign =>
                {
                    //fill in model values from the entity
                    var campaignModel = campaign.ToModel<CampaignModel>();

                    //convert dates to the user time
                    campaignModel.CreatedOn = _dateTimeHelper.ConvertToUserTime(campaign.CreatedOnUtc, DateTimeKind.Utc);
                    if (campaign.DontSendBeforeDateUtc.HasValue)
                    {
                        campaignModel.DontSendBeforeDate = _dateTimeHelper
                            .ConvertToUserTime(campaign.DontSendBeforeDateUtc.Value, DateTimeKind.Utc);
                    }

                    return campaignModel;
                }),
                Total = campaigns.Count
            };

            return model;
        }

        /// <summary>
        /// Prepare campaign model
        /// </summary>
        /// <param name="model">Campaign model</param>
        /// <param name="campaign">Campaign</param>
        /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
        /// <returns>Campaign model</returns>
        public virtual CampaignModel PrepareCampaignModel(CampaignModel model, Campaign campaign, bool excludeProperties = false)
        {
            //fill in model values from the entity
            if (campaign != null)
            {
                model = model ?? campaign.ToModel<CampaignModel>();
                if (campaign.DontSendBeforeDateUtc.HasValue)
                    model.DontSendBeforeDate = _dateTimeHelper.ConvertToUserTime(campaign.DontSendBeforeDateUtc.Value, DateTimeKind.Utc);
            }

            model.AllowedTokens = string.Join(", ", _messageTokenProvider.GetListOfCampaignAllowedTokens());

            //whether to fill in some of properties
            if (!excludeProperties)
                model.EmailAccountId = _emailAccountSettings.DefaultEmailAccountId;

            //prepare available stores
            _baseAdminModelFactory.PrepareStores(model.AvailableStores);

            //prepare available user roles
            _baseAdminModelFactory.PrepareUserRoles(model.AvailableUserRoles);

            //prepare available email accounts
            _baseAdminModelFactory.PrepareEmailAccounts(model.AvailableEmailAccounts, false);

            return model;
        }

        #endregion
    }
}