using System.ComponentModel.DataAnnotations;
using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Settings
{
    /// <summary>
    /// Represents a user settings model
    /// </summary>
    public partial class UserSettingsModel : BaseAppModel, ISettingsModel
    {
        #region Properties

        public int ActiveStoreScopeConfiguration { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.UsernamesEnabled")]
        public bool UsernamesEnabled { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.AllowUsersToChangeUsernames")]
        public bool AllowUsersToChangeUsernames { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.CheckUsernameAvailabilityEnabled")]
        public bool CheckUsernameAvailabilityEnabled { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.UsernameValidationEnabled")]
        public bool UsernameValidationEnabled { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.UsernameValidationUseRegex")]
        public bool UsernameValidationUseRegex { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.UsernameValidationRule")]
        public string UsernameValidationRule { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.UserRegistrationType")]
        public int UserRegistrationType { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.AllowUsersToUploadAvatars")]
        public bool AllowUsersToUploadAvatars { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.DefaultAvatarEnabled")]
        public bool DefaultAvatarEnabled { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.ShowUsersLocation")]
        public bool ShowUsersLocation { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.ShowUsersJoinDate")]
        public bool ShowUsersJoinDate { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.AllowViewingProfiles")]
        public bool AllowViewingProfiles { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.NotifyNewUserRegistration")]
        public bool NotifyNewUserRegistration { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.RequireRegistrationForDownloadableProducts")]
        public bool RequireRegistrationForDownloadableProducts { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.AllowUsersToCheckGiftCardBalance")]
        public bool AllowUsersToCheckGiftCardBalance { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.HideDownloadableProductsTab")]
        public bool HideDownloadableProductsTab { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.HideBackInStockSubscriptionsTab")]
        public bool HideBackInStockSubscriptionsTab { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.UserNameFormat")]
        public int UserNameFormat { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.PasswordMinLength")]
        public int PasswordMinLength { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.UnduplicatedPasswordsNumber")]
        public int UnduplicatedPasswordsNumber { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.PasswordRecoveryLinkDaysValid")]
        public int PasswordRecoveryLinkDaysValid { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.DefaultPasswordFormat")]
        public int DefaultPasswordFormat { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.PasswordLifetime")]
        public int PasswordLifetime { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.FailedPasswordAllowedAttempts")]
        public int FailedPasswordAllowedAttempts { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.FailedPasswordLockoutMinutes")]
        public int FailedPasswordLockoutMinutes { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.NewsletterEnabled")]
        public bool NewsletterEnabled { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.NewsletterTickedByDefault")]
        public bool NewsletterTickedByDefault { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.HideNewsletterBlock")]
        public bool HideNewsletterBlock { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.NewsletterBlockAllowToUnsubscribe")]
        public bool NewsletterBlockAllowToUnsubscribe { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.StoreLastVisitedPage")]
        public bool StoreLastVisitedPage { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.StoreIpAddresses")]
        public bool StoreIpAddresses { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.EnteringEmailTwice")]
        public bool EnteringEmailTwice { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.GenderEnabled")]
        public bool GenderEnabled { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.DateOfBirthEnabled")]
        public bool DateOfBirthEnabled { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.DateOfBirthRequired")]
        public bool DateOfBirthRequired { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.DateOfBirthMinimumAge")]
        [UIHint("Int32Nullable")]
        public int? DateOfBirthMinimumAge { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.CompanyEnabled")]
        public bool CompanyEnabled { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.CompanyRequired")]
        public bool CompanyRequired { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.StreetAddressEnabled")]
        public bool StreetAddressEnabled { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.StreetAddressRequired")]
        public bool StreetAddressRequired { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.StreetAddress2Enabled")]
        public bool StreetAddress2Enabled { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.StreetAddress2Required")]
        public bool StreetAddress2Required { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.ZipPostalCodeEnabled")]
        public bool ZipPostalCodeEnabled { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.ZipPostalCodeRequired")]
        public bool ZipPostalCodeRequired { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.CityEnabled")]
        public bool CityEnabled { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.CityRequired")]
        public bool CityRequired { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.CountyEnabled")]
        public bool CountyEnabled { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.CountyRequired")]
        public bool CountyRequired { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.CountryEnabled")]
        public bool CountryEnabled { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.CountryRequired")]
        public bool CountryRequired { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.StateProvinceEnabled")]
        public bool StateProvinceEnabled { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.StateProvinceRequired")]
        public bool StateProvinceRequired { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.PhoneEnabled")]
        public bool PhoneEnabled { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.PhoneRequired")]
        public bool PhoneRequired { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.FaxEnabled")]
        public bool FaxEnabled { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.FaxRequired")]
        public bool FaxRequired { get; set; }

        [AppResourceDisplayName("Admin.Configuration.Settings.UserUser.AcceptPrivacyPolicyEnabled")]
        public bool AcceptPrivacyPolicyEnabled { get; set; }        

        #endregion
    }
}