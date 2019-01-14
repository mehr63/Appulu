using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Appulu.Web.Framework.Models;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Web.Areas.Admin.Models.Users
{
    /// <summary>
    /// Represents a user search model
    /// </summary>
    public partial class UserSearchModel : BaseSearchModel, IAclSupportedModel
    {
        #region Ctor

        public UserSearchModel()
        {
            SelectedUserRoleIds = new List<int>();
            AvailableUserRoles = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [AppResourceDisplayName("Admin.Users.Users.List.UserRoles")]
        public IList<int> SelectedUserRoleIds { get; set; }

        public IList<SelectListItem> AvailableUserRoles { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.List.SearchEmail")]
        public string SearchEmail { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.List.SearchUsername")]
        public string SearchUsername { get; set; }

        public bool UsernamesEnabled { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.List.SearchFirstName")]
        public string SearchFirstName { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.List.SearchLastName")]
        public string SearchLastName { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.List.SearchDateOfBirth")]
        public string SearchDayOfBirth { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.List.SearchDateOfBirth")]
        public string SearchMonthOfBirth { get; set; }

        public bool DateOfBirthEnabled { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.List.SearchCompany")]
        public string SearchCompany { get; set; }

        public bool CompanyEnabled { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.List.SearchPhone")]
        public string SearchPhone { get; set; }

        public bool PhoneEnabled { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.List.SearchZipCode")]
        public string SearchZipPostalCode { get; set; }

        public bool ZipPostalCodeEnabled { get; set; }

        [AppResourceDisplayName("Admin.Users.Users.List.SearchIpAddress")]
        public string SearchIpAddress { get; set; }

        public bool AvatarEnabled { get; internal set; }

        #endregion
    }
}