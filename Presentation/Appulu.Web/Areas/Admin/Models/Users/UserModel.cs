using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;
using Appulu.Core.Domain.Catalog;
using Appulu.Web.Areas.Admin.Validators.Users;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;

namespace Appulu.Web.Areas.Admin.Models.Users
{
    /// <summary>
    /// Represents a user model
    /// </summary>
    [Validator(typeof(UserValidator))]
    public partial class UserModel : BaseAppEntityModel, IAclSupportedModel
    {
        #region Ctor

        public UserModel()
        {
            this.AvailableTimeZones = new List<SelectListItem>();
            this.SendEmail = new SendEmailModel() { SendImmediately = true };
            this.SendPm = new SendPmModel();

            this.SelectedUserRoleIds = new List<int>();
            this.AvailableUserRoles = new List<SelectListItem>();

            this.AssociatedExternalAuthRecords = new List<UserAssociatedExternalAuthModel>();
            this.AvailableCountries = new List<SelectListItem>();
            this.AvailableStates = new List<SelectListItem>();
            this.AvailableVendors = new List<SelectListItem>();
            this.UserAttributes = new List<UserAttributeModel>();
            this.AvailableNewsletterSubscriptionStores = new List<SelectListItem>();
            this.SelectedNewsletterSubscriptionStoreIds = new List<int>();
            this.AddRewardPoints = new AddRewardPointsToUserModel();
            this.UserRewardPointsSearchModel = new UserRewardPointsSearchModel();
            this.UserAddressSearchModel = new UserAddressSearchModel();
            this.UserOrderSearchModel = new UserOrderSearchModel();
            this.UserShoppingCartSearchModel = new UserShoppingCartSearchModel();
            this.UserActivityLogSearchModel = new UserActivityLogSearchModel();
            this.UserBackInStockSubscriptionSearchModel = new UserBackInStockSubscriptionSearchModel();
        }

        #endregion

        #region Properties

        public bool UsernamesEnabled { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.Fields.Username")]
        public string Username { get; set; }

        [DataType(DataType.EmailAddress)]
        [AppResourceDisplayName("Admin.Users.Users.Fields.Email")]
        public string Email { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.Fields.Password")]
        [DataType(DataType.Password)]
        [NoTrim]
        public string Password { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.Fields.Vendor")]
        public int VendorId { get; set; }

        public IList<SelectListItem> AvailableVendors { get; set; }

        //form fields & properties
        public bool GenderEnabled { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.Fields.Gender")]
        public string Gender { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.Fields.FirstName")]
        public string FirstName { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.Fields.LastName")]
        public string LastName { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.Fields.FullName")]
        public string FullName { get; set; }

        public bool DateOfBirthEnabled { get; set; }

        [UIHint("DateNullable")]
        [AppResourceDisplayName("Admin.Users.Users.Fields.DateOfBirth")]
        public DateTime? DateOfBirth { get; set; }

        public bool CompanyEnabled { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.Fields.Company")]
        public string Company { get; set; }

        public bool StreetAddressEnabled { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.Fields.StreetAddress")]
        public string StreetAddress { get; set; }

        public bool StreetAddress2Enabled { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.Fields.StreetAddress2")]
        public string StreetAddress2 { get; set; }

        public bool ZipPostalCodeEnabled { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.Fields.ZipPostalCode")]
        public string ZipPostalCode { get; set; }

        public bool CityEnabled { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.Fields.City")]
        public string City { get; set; }

        public bool CountyEnabled { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.Fields.County")]
        public string County { get; set; }

        public bool CountryEnabled { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.Fields.Country")]
        public int CountryId { get; set; }

        public IList<SelectListItem> AvailableCountries { get; set; }

        public bool StateProvinceEnabled { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.Fields.StateProvince")]
        public int StateProvinceId { get; set; }

        public IList<SelectListItem> AvailableStates { get; set; }

        public bool PhoneEnabled { get; set; }

        [DataType(DataType.PhoneNumber)]
        [AppResourceDisplayName("Admin.Users.Users.Fields.Phone")]
        public string Phone { get; set; }

        public bool FaxEnabled { get; set; }

        [DataType(DataType.PhoneNumber)]
        [AppResourceDisplayName("Admin.Users.Users.Fields.Fax")]
        public string Fax { get; set; }

        public List<UserAttributeModel> UserAttributes { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.Fields.RegisteredInStore")]
        public string RegisteredInStore { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.Fields.AdminComment")]
        public string AdminComment { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.Fields.IsTaxExempt")]
        public bool IsTaxExempt { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.Fields.Active")]
        public bool Active { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.Fields.Affiliate")]
        public int AffiliateId { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.Fields.Affiliate")]
        public string AffiliateName { get; set; }

        //time zone
        [AppResourceDisplayName("Admin.Users.Users.Fields.TimeZoneId")]
        public string TimeZoneId { get; set; }

        public bool AllowUsersToSetTimeZone { get; set; }

        public IList<SelectListItem> AvailableTimeZones { get; set; }

        //EU VAT
        [AppResourceDisplayName("Admin.Users.Users.Fields.VatNumber")]
        public string VatNumber { get; set; }

        public string VatNumberStatusNote { get; set; }

        public bool DisplayVatNumber { get; set; }

        //registration date
        [AppResourceDisplayName("Admin.Users.Users.Fields.CreatedOn")]
        public DateTime CreatedOn { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.Fields.LastActivityDate")]
        public DateTime LastActivityDate { get; set; }

        //IP address
        [AppResourceDisplayName("Admin.Users.Users.Fields.IPAddress")]
        public string LastIpAddress { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.Fields.LastVisitedPage")]
        public string LastVisitedPage { get; set; }

        //user roles
        [AppResourceDisplayName("Admin.Users.Users.Fields.UserRoles")]
        public string UserRoleNames { get; set; }

        public IList<SelectListItem> AvailableUserRoles { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.Fields.UserRoles")]
        public IList<int> SelectedUserRoleIds { get; set; }

        //newsletter subscriptions (per store)
        [AppResourceDisplayName("Admin.Users.Users.Fields.Newsletter")]
        public IList<SelectListItem> AvailableNewsletterSubscriptionStores { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.Fields.Newsletter")]
        public IList<int> SelectedNewsletterSubscriptionStoreIds { get; set; }

        //reward points history
        public bool DisplayRewardPointsHistory { get; set; }

        public AddRewardPointsToUserModel AddRewardPoints { get; set; }

        public UserRewardPointsSearchModel UserRewardPointsSearchModel { get; set; }

        //send email model
        public SendEmailModel SendEmail { get; set; }

        //send PM model
        public SendPmModel SendPm { get; set; }

        //send the welcome message
        public bool AllowSendingOfWelcomeMessage { get; set; }

        //re-send the activation message
        public bool AllowReSendingOfActivationMessage { get; set; }

        //GDPR enabled
        public bool GdprEnabled { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.AssociatedExternalAuth")]
        public IList<UserAssociatedExternalAuthModel> AssociatedExternalAuthRecords { get; set; }

        public string AvatarUrl { get; internal set; }

        public UserAddressSearchModel UserAddressSearchModel { get; set; }

        public UserOrderSearchModel UserOrderSearchModel { get; set; }

        public UserShoppingCartSearchModel UserShoppingCartSearchModel { get; set; }

        public UserActivityLogSearchModel UserActivityLogSearchModel { get; set; }

        public UserBackInStockSubscriptionSearchModel UserBackInStockSubscriptionSearchModel { get; set; }

        #endregion

        #region Nested classes

        public partial class SendEmailModel : BaseAppModel
        {
            [AppResourceDisplayName("Admin.Users.Users.SendEmail.Subject")]
            public string Subject { get; set; }

            [AppResourceDisplayName("Admin.Users.Users.SendEmail.Body")]
            public string Body { get; set; }

            [AppResourceDisplayName("Admin.Users.Users.SendEmail.SendImmediately")]
            public bool SendImmediately { get; set; }

            [AppResourceDisplayName("Admin.Users.Users.SendEmail.DontSendBeforeDate")]
            [UIHint("DateTimeNullable")]
            public DateTime? DontSendBeforeDate { get; set; }
        }

        public partial class SendPmModel : BaseAppModel
        {
            [AppResourceDisplayName("Admin.Users.Users.SendPM.Subject")]
            public string Subject { get; set; }

            [AppResourceDisplayName("Admin.Users.Users.SendPM.Message")]
            public string Message { get; set; }
        }

        public partial class UserAttributeModel : BaseAppEntityModel
        {
            public UserAttributeModel()
            {
                Values = new List<UserAttributeValueModel>();
            }

            public string Name { get; set; }

            public bool IsRequired { get; set; }

            /// <summary>
            /// Default value for textboxes
            /// </summary>
            public string DefaultValue { get; set; }

            public AttributeControlType AttributeControlType { get; set; }

            public IList<UserAttributeValueModel> Values { get; set; }
        }

        public partial class UserAttributeValueModel : BaseAppEntityModel
        {
            public string Name { get; set; }

            public bool IsPreSelected { get; set; }
        }

        #endregion
    }
}