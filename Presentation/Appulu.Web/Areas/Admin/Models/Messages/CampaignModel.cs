using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;
using Appulu.Web.Areas.Admin.Validators.Messages;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Messages
{
    /// <summary>
    /// Represents a campaign model
    /// </summary>
    [Validator(typeof(CampaignValidator))]
    public partial class CampaignModel : BaseAppEntityModel
    {
        #region Ctor

        public CampaignModel()
        {
            this.AvailableStores = new List<SelectListItem>();
            this.AvailableUserRoles = new List<SelectListItem>();
            this.AvailableEmailAccounts = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [AppResourceDisplayName("Admin.Promotions.Campaigns.Fields.Name")]
        public string Name { get; set; }

        [AppResourceDisplayName("Admin.Promotions.Campaigns.Fields.Subject")]
        public string Subject { get; set; }

        [AppResourceDisplayName("Admin.Promotions.Campaigns.Fields.Body")]
        public string Body { get; set; }

        [AppResourceDisplayName("Admin.Promotions.Campaigns.Fields.Store")]
        public int StoreId { get; set; }
        public IList<SelectListItem> AvailableStores { get; set; }

        [AppResourceDisplayName("Admin.Promotions.Campaigns.Fields.UserRole")]
        public int UserRoleId { get; set; }
        public IList<SelectListItem> AvailableUserRoles { get; set; }

        [AppResourceDisplayName("Admin.Promotions.Campaigns.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        [AppResourceDisplayName("Admin.Promotions.Campaigns.Fields.DontSendBeforeDate")]
        [UIHint("DateTimeNullable")]
        public DateTime? DontSendBeforeDate { get; set; }

        [AppResourceDisplayName("Admin.Promotions.Campaigns.Fields.AllowedTokens")]
        public string AllowedTokens { get; set; }

        [AppResourceDisplayName("Admin.Promotions.Campaigns.Fields.EmailAccount")]
        public int EmailAccountId { get; set; }
        public IList<SelectListItem> AvailableEmailAccounts { get; set; }

        [DataType(DataType.EmailAddress)]
        [AppResourceDisplayName("Admin.Promotions.Campaigns.Fields.TestEmail")]
        public string TestEmail { get; set; }

        #endregion
    }
}