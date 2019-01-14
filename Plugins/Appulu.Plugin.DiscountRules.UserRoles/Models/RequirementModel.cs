using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Appulu.Web.Framework.Mvc.ModelBinding;

namespace Appulu.Plugin.DiscountRules.UserRoles.Models
{
    public class RequirementModel
    {
        public RequirementModel()
        {
            AvailableUserRoles = new List<SelectListItem>();
        }

        [AppResourceDisplayName("Plugins.DiscountRules.UserRoles.Fields.UserRole")]
        public int UserRoleId { get; set; }

        public int DiscountId { get; set; }

        public int RequirementId { get; set; }

        public IList<SelectListItem> AvailableUserRoles { get; set; }
    }
}