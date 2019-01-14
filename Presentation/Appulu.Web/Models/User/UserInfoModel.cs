using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;
using Appulu.Web.Framework.Mvc.ModelBinding;
using Appulu.Web.Framework.Models;
using Appulu.Web.Validators.User;

namespace Appulu.Web.Models.User
{
    [Validator(typeof(UserInfoValidator))]
    public partial class UserInfoModel : BaseAppModel
    {
        public UserInfoModel()
        {
            this.AvailableTimeZones = new List<SelectListItem>();
            this.AvailableCountries = new List<SelectListItem>();
            this.AvailableStates = new List<SelectListItem>();
            this.AssociatedExternalAuthRecords = new List<AssociatedExternalAuthModel>();
            this.UserAttributes = new List<UserAttributeModel>();
            this.GdprConsents = new List<GdprConsentModel>();
        }
        
        [DataType(DataType.EmailAddress)]
        [AppResourceDisplayName("Account.Fields.Email")]
        public string Email { get; set; }
        [DataType(DataType.EmailAddress)]
        [AppResourceDisplayName("Account.Fields.EmailToRevalidate")]
        public string EmailToRevalidate { get; set; }

        public bool CheckUsernameAvailabilityEnabled { get; set; }
        public bool AllowUsersToChangeUsernames { get; set; }
        public bool UsernamesEnabled { get; set; }
        [AppResourceDisplayName("Account.Fields.Username")]
        public string Username { get; set; }

        //form fields & properties
        public bool GenderEnabled { get; set; }
        [AppResourceDisplayName("Account.Fields.Gender")]
        public string Gender { get; set; }

        [AppResourceDisplayName("Account.Fields.FirstName")]
        public string FirstName { get; set; }
        [AppResourceDisplayName("Account.Fields.LastName")]
        public string LastName { get; set; }

        public bool DateOfBirthEnabled { get; set; }
        [AppResourceDisplayName("Account.Fields.DateOfBirth")]
        public int? DateOfBirthDay { get; set; }
        [AppResourceDisplayName("Account.Fields.DateOfBirth")]
        public int? DateOfBirthMonth { get; set; }
        [AppResourceDisplayName("Account.Fields.DateOfBirth")]
        public int? DateOfBirthYear { get; set; }
        public bool DateOfBirthRequired { get; set; }
        public DateTime? ParseDateOfBirth()
        {
            if (!DateOfBirthYear.HasValue || !DateOfBirthMonth.HasValue || !DateOfBirthDay.HasValue)
                return null;

            DateTime? dateOfBirth = null;
            try
            {
                dateOfBirth = new DateTime(DateOfBirthYear.Value, DateOfBirthMonth.Value, DateOfBirthDay.Value);
            }
            catch { }
            return dateOfBirth;
        }

        public bool CompanyEnabled { get; set; }
        public bool CompanyRequired { get; set; }
        [AppResourceDisplayName("Account.Fields.Company")]
        public string Company { get; set; }

        public bool StreetAddressEnabled { get; set; }
        public bool StreetAddressRequired { get; set; }
        [AppResourceDisplayName("Account.Fields.StreetAddress")]
        public string StreetAddress { get; set; }

        public bool StreetAddress2Enabled { get; set; }
        public bool StreetAddress2Required { get; set; }
        [AppResourceDisplayName("Account.Fields.StreetAddress2")]
        public string StreetAddress2 { get; set; }

        public bool ZipPostalCodeEnabled { get; set; }
        public bool ZipPostalCodeRequired { get; set; }
        [AppResourceDisplayName("Account.Fields.ZipPostalCode")]
        public string ZipPostalCode { get; set; }

        public bool CityEnabled { get; set; }
        public bool CityRequired { get; set; }
        [AppResourceDisplayName("Account.Fields.City")]
        public string City { get; set; }

        public bool CountyEnabled { get; set; }
        public bool CountyRequired { get; set; }
        [AppResourceDisplayName("Account.Fields.County")]
        public string County { get; set; }

        public bool CountryEnabled { get; set; }
        public bool CountryRequired { get; set; }
        [AppResourceDisplayName("Account.Fields.Country")]
        public int CountryId { get; set; }
        public IList<SelectListItem> AvailableCountries { get; set; }

        public bool StateProvinceEnabled { get; set; }
        public bool StateProvinceRequired { get; set; }
        [AppResourceDisplayName("Account.Fields.StateProvince")]
        public int StateProvinceId { get; set; }
        public IList<SelectListItem> AvailableStates { get; set; }

        public bool PhoneEnabled { get; set; }
        public bool PhoneRequired { get; set; }
        [DataType(DataType.PhoneNumber)]
        [AppResourceDisplayName("Account.Fields.Phone")]
        public string Phone { get; set; }

        public bool FaxEnabled { get; set; }
        public bool FaxRequired { get; set; }
        [DataType(DataType.PhoneNumber)]
        [AppResourceDisplayName("Account.Fields.Fax")]
        public string Fax { get; set; }

        public bool NewsletterEnabled { get; set; }
        [AppResourceDisplayName("Account.Fields.Newsletter")]
        public bool Newsletter { get; set; }

        //preferences
        public bool SignatureEnabled { get; set; }
        [AppResourceDisplayName("Account.Fields.Signature")]
        public string Signature { get; set; }

        //time zone
        [AppResourceDisplayName("Account.Fields.TimeZone")]
        public string TimeZoneId { get; set; }
        public bool AllowUsersToSetTimeZone { get; set; }
        public IList<SelectListItem> AvailableTimeZones { get; set; }

        //EU VAT
        [AppResourceDisplayName("Account.Fields.VatNumber")]
        public string VatNumber { get; set; }
        public string VatNumberStatusNote { get; set; }
        public bool DisplayVatNumber { get; set; }

        //external authentication
        [AppResourceDisplayName("Account.AssociatedExternalAuth")]
        public IList<AssociatedExternalAuthModel> AssociatedExternalAuthRecords { get; set; }
        public int NumberOfExternalAuthenticationProviders { get; set; }
        public bool AllowUsersToRemoveAssociations { get; set; }

        public IList<UserAttributeModel> UserAttributes { get; set; }

        public IList<GdprConsentModel> GdprConsents { get; set; }

        #region Nested classes

        public partial class AssociatedExternalAuthModel : BaseAppEntityModel
        {
            public string Email { get; set; }

            public string ExternalIdentifier { get; set; }

            public string AuthMethodName { get; set; }
        }
        
        #endregion
    }
}